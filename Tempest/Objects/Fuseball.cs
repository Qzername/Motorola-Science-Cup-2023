using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Tempest.Objects
{
	public class Fuseball : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition = -1;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition || GameManager.StopGame)
				return;

			if (other.Name == "Bullet")
				Die();
		}

		public override Setup Start()
		{
			if (_mapPosition == -1)
				_mapPosition = GameManager.Rand.Next(0, MapManager.Instance.Elements.Count);

			return new Setup()
			{
				Name = "Fuseball",
				Shape = new PointShape(GameManager.LevelConfig.Fuseball,
								new Point(0, 20, 0),
								new Point(20, 0, 0),
								new Point(0, -20, 0),
								new Point(-20, 0, 0)),
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z) + new Point(0, 0, GameManager.LevelConfig.Length),
				Rotation = MapManager.Instance.Elements[_mapPosition].Transform.Rotation
			};
		}

		public void Setup(int mapPosition)
		{
			_mapPosition = mapPosition;
		}

		public override void Update(float delta)
		{
			if (GameManager.StopGame)
				return;
		}

		void Die()
		{
			if (IsDead)
				return;

			((GameWindow)window).EnemyKilled(this);
			IsDead = true;

			window.Destroy(this);
		}

		public override void OnDestroy()
		{
		}
	}
}
