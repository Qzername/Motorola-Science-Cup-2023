using Microsoft.Win32.SafeHandles;
using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Resources;

namespace Battlezone.Objects.Enemies
{
	public class Tank : Enemy
	{
		EnemyCollider front;

		const float colliderDistance = 5f;

		protected virtual Point defaultRotation =>  new Point(0, 0,0);

		protected virtual PredefinedShape shape => (PredefinedShape)ResourcesHandler.Get3DShape("tank");

        public override int Score => 1000;

		protected virtual float Speed => 10f;
		const float bulletFrequency = 4f;
		float currentTimer = 0f;

		public Tank(Point startPosition) : base(startPosition)
		{
		}

		public override int PhysicsLayer => 1;

		public override void OnCollisionEnter(PhysicsObject other)
		{
		}

		public override Setup Start()
		{
			front = new EnemyCollider();
			window.Instantiate(front);

			transform = new Transform()
			{
				Position = startPosition
			};

			front.UpdatePosition(PointManipulationTools.MovePointForward(RecalculateRotation(), colliderDistance));

			return new Setup()
			{
				Name = "Enemy_TANK",
				Position = startPosition,
				Rotation = new Point(0,-90,0) + defaultRotation,
				Shape = shape
			};
		}

		public override void Update(float delta)
		{
			front.UpdatePosition(PointManipulationTools.MovePointForward(RecalculateRotation(), colliderDistance));
			
			if (!front.IsColliding)
			{
				//position
				var pos = PositionCalculationTools.NextPositionTowardsPlayer(transform, Speed, delta);

				transform.Position = pos.Item1;

                Rotate(new Point(0, pos.Item2, 0));

				//shooting
				currentTimer += delta;
			}

			if (currentTimer < bulletFrequency)
				return;

			window.Instantiate(new EnemyBullet(RecalculateRotation()));

			currentTimer = 0f;
		}

		public override void OnDestroy()
		{
			//gdyby nie ta linijka ten obiekt nadal by instniał mimo że jego posiadacz już nie istnieje
			IsDead = true;
			window.Destroy(front);
		}

		Transform RecalculateRotation()
		{
			var offset = Scene3D.Camera.Position - transform.Position;

			return new Transform()
			{
				Position = transform.Position + new Point(0,-10,0),
				Rotation = new Point(0, 180 - transform.Rotation.Y - 90 + (offset.X < 0 && offset.Z < 0 ? 180 : 0) + defaultRotation.Y, 0)
			};
		}
	}
}
