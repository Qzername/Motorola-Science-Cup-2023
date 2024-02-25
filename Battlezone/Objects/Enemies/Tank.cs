using Microsoft.VisualBasic.FileIO;
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

		protected virtual float Speed => 20f;
		const float bulletFrequency = 4f;
		float currentTimer = 0f;

		float passiveMax;
		float passiveCounter;

		AIType currentAIType;

		public Tank(Point startPosition) : base(startPosition)
		{
		}

		public override int PhysicsLayer => 1;

		public override void OnCollisionEnter(PhysicsObject other)
		{
		}

		public override Setup Start()
		{
			Random rng = new Random();

			currentAIType = AIType.Passive;

			//gra będzie coraz cięższa gdy gracz będzie miał więcej punktów
			//czołgi będą bardziej agresywne
			var lateGameOffset = GameManager.Score / 10000 *1.5f;

			if (20 - lateGameOffset < 0)
				lateGameOffset = 19;

			passiveMax = rng.Next(0, 20 - Convert.ToInt32(lateGameOffset));

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

		const float blockedMax = 5f;
		float blockedTimer = 0f;

        public override void Update(float delta)
		{
			front.UpdatePosition(PointManipulationTools.MovePointForward(RecalculateRotation(), colliderDistance));

			//ma czystą drogę
			if (!front.IsColliding)
			{
				if (currentAIType == AIType.Passive)
					PassiveMode(delta);
				else
					AggresiveMode(delta);
            }
            //coś mu blokuje droge, więc się cofa 
            else
            {
				blockedTimer += delta;

				if(blockedMax > blockedTimer)
				{
					front.IsColliding = false;
					blockedTimer = 0f;
				}

				transform.Position = PointManipulationTools.MovePointForward(transform, -Speed * delta);
            }
        }

        float changeAItimer;

        Point? nextDestination;

        void PassiveMode(float delta)
		{
			//timer od zmiany AI
			changeAItimer += delta;

			if(changeAItimer >= passiveMax)
			{
				currentAIType = AIType.Aggresive;
				return;
			}

			//obliczanie następnego punktu podróży
			if (nextDestination == null)
			{
				var rng = new Random();

                nextDestination = PointManipulationTools.MovePointForward(new Transform()
                {
                    Position = transform.Position,
                    Rotation = transform.Rotation + new Point(0, rng.Next(-45,45), 0)
                }, rng.Next(50,100));
            }
		
			var nextPosition = PositionCalculationTools.NextPositionTowardsPoint(Transform, nextDestination.Value, Speed * delta);

			//powolna rotacja do punktu
			if(nextPosition.Item2 > 5)
			{
				Rotate(new Point(0, Speed * delta, 0));
				return;
			}

			if(MathTools.CalculateDistance(nextDestination.Value,  transform.Position) < Speed * delta*2)
			{
				nextDestination = null;
				return;
			}

            Rotate(new Point(0, nextPosition.Item2, 0));
			transform.Position = nextPosition.Item1;
        }

		void AggresiveMode(float delta)
        {
            //position
            var pos = PositionCalculationTools.NextPositionTowardsPoint(transform, Scene3D.Camera.Position, Speed * delta);

            //powolna rotacja do gracza
            if (pos.Item2 > 5)
            {
                Rotate(new Point(0, Speed * delta * (pos.Item2 < 0 ? -1 : 1), 0));
                return;
            }

            transform.Position = pos.Item1;

            Rotate(new Point(0, pos.Item2, 0));

            //shooting
            currentTimer += delta;

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

		enum AIType
		{
			/*
			 * Agresywny czołg idzie do gracza i do niego strzela
			 * Pasywne nic wlasciwie nie robi oprócz podrózowania
			 * 
			 * Z czasem pasywne czołgi zamieniają się w agresywne
			 */
			Aggresive,
			Passive
		}
	}
}
