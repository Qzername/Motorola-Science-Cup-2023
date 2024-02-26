using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Tempest.Objects
{
	public class Fuseball : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition = -1;

		private const float ZSpeed = 400f;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition || GameManager.StopGame)
				return;

			if (other.Name == "Bullet")
				Die(true);
		}

		public override Setup Start()
		{
			if (_mapPosition == -1)
				_mapPosition = GameManager.Rand.Next(0, MapManager.Instance.Elements.Count);

			if (transform.Position.Z == 0)
				transform.Position.Z = GameManager.LevelConfig.Length;

			return new Setup()
			{
				Name = "Fuseball",
				Shape = new PointShape(GameManager.LevelConfig.Fuseball,
								new Point(0, 20, 0),
								new Point(20, 0, 0),
								new Point(0, -20, 0),
								new Point(-20, 0, 0)),
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z),
				Rotation = MapManager.Instance.Elements[_mapPosition].Transform.Rotation
			};
		}

		public void Setup(int mapPosition, float zPosition = 1600)
		{
			_mapPosition = mapPosition;
			transform.Position.Z = zPosition;
		}

		public override void Update(float delta)
		{
			if (GameManager.StopGame)
				return;

			if (transform.Position.Z > 400)
				transform.Position.Z -= ZSpeed * delta;
			else
				Die(false);
		}

		void Die(bool killedByPlayer)
		{
			if (IsDead)
				return;
			
			if (killedByPlayer)
				GameManager.Score += 500;
			
			((GameWindow)window).EnemyDestroyed(this);
			IsDead = true;				
			
			window.Destroy(this);
		}

		public override void OnDestroy()
		{
		}
	}
}
