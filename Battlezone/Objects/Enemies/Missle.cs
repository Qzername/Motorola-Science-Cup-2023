using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Resources;

namespace Battlezone.Objects.Enemies
{
	public class Missle : Enemy
	{
		public override int PhysicsLayer => 1;

		float speed = 150f;
		public override int Score => 2000;

		Point[] zigzagPositions;
		int currentPoint;

		//i tak missle będzie miało swoją pozycje
		public Missle() : base(Point.Zero)
		{
		}

		public override void OnCollisionEnter(PhysicsObject other)
		{
			//całe usuwanie obiektu jest w pocisku
		}

		public override Setup Start()
        {
			SoundRegistry.Instance.Database["alert"].Play();

            //gra sie staje szybsza wraz z progresją gracza
            speed += 50 * (GameManager.Instance.Score / 10000);

			//ustawienia
			int rotationOffset = 25;
			float distanceIncrease = 60;

			//losowanie pozycji do poruszania sie zigzag
			Random rng = new Random();

			var pos = PointManipulationTools.MovePointForward(Scene3D.Camera, 400);

            var posTowardsPlayer = PositionCalculationTools.NextPositionTowardsPoint(transform, Scene3D.Camera.Position, 1);

            zigzagPositions = new Point[rng.Next(1, 4)];

			for (int i = 0; i < zigzagPositions.Length; i++)
			{
				zigzagPositions[i] = PointManipulationTools.MovePointForward(new Transform()
				{
					Position = Scene3D.Camera.Position,
					Rotation = Scene3D.Camera.Rotation + new Point(0, rng.Next(0, rotationOffset) * (i%2==0 ? 1:-1),0),
                }, 400-distanceIncrease * (i+1));
			}

            return new Setup()
			{
				Name = "Enemy_MISSLE",
				Position = pos, //Spawn przed graczem (zawsze)
				Rotation = new Point(0, posTowardsPlayer.Item2 + rng.Next(-rotationOffset, rotationOffset), 0),
				Shape = (PredefinedShape)ResourcesHandler.Get3DShape("missle")
			};
		}

        public override void Update(float delta)
		{
			(Point, float) pos;

			if(currentPoint >= zigzagPositions.Length)
                pos = PositionCalculationTools.NextPositionTowardsPoint(transform, Scene3D.Camera.Position, speed * delta);
            else
            {
				if (MathTools.CalculateDistance(zigzagPositions[currentPoint], transform.Position) < speed * delta)
                {
                    currentPoint++;
					return;
                }

                pos = PositionCalculationTools.NextPositionTowardsPoint(transform, zigzagPositions[currentPoint], speed * delta);
            }

            transform.Position = pos.Item1;
			//nawet nie chce wiedzieć czemu czasami musze dodawać 180 stopni ale jak działa to po co narzekać
            Rotate(new Point(0, pos.Item2 +(currentPoint >= zigzagPositions.Length ? 0:180), 0));
        }
	}
}
