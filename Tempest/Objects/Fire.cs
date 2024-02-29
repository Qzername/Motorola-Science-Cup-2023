using System.Windows.Forms;
using SkiaSharp;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;

namespace Tempest.Objects
{
	public class Fire : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition = -1;
		private int _lives = 3;
		private bool _takingDamage;

		private float ZSpeed = 400f;

		public override async void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition || GameManager.Instance.StopGame)
				return;

			if (other.Name == "Bullet" && !_takingDamage)
			{
				_takingDamage = true;
				
				_lives--;
				SKColor color = GameManager.Instance.LevelConfig.Fire;

				if (_lives == 2)
				{
					ZSpeed = 300f;
					color = SKColors.Yellow;
				}
				else if (_lives == 1)
				{
					ZSpeed = 200f;
					color = SKColors.Cyan;
				}
				else if (_lives == 0)
				{
					Die(true);
					return;
				}

				Shape = new PointShape(color,
					new Point(0, 20, 0),
					new Point(20, 0, 0),
					new Point(10, 10, 0),
					new Point(0, -20, 0),
					new Point(-10, 10, 0),
					new Point(-20, 0, 0)
				);
				Rotate(MapManager.Instance.Elements[_mapPosition].Transform.Rotation);
				
				await Task.Delay(500);

				_takingDamage = false;
			}
		}

		public override Setup Start()
		{
			if (_mapPosition == -1)
				_mapPosition = GameManager.Instance.Rand.Next(0, MapManager.Instance.Elements.Count);

			if (transform.Position.Z == 0)
				transform.Position.Z = GameManager.Instance.LevelConfig.Length;

			return new Setup()
			{
				Name = "Fire",
				Shape = new PointShape(GameManager.Instance.LevelConfig.Fire,
					new Point(0, 20, 0),
					new Point(20, 0, 0),
					new Point(10, 10, 0),
					new Point(0, -20, 0),
					new Point(-10, 10, 0),
					new Point(-20, 0, 0)
				),
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
			if (GameManager.Instance.StopGame)
				return;

			transform.Position.Z -= ZSpeed * delta;

			if (transform.Position.Z < 400)
				Die(false);
		}

		void Die(bool killedByPlayer)
		{
			if (IsDead)
				return;

			if (killedByPlayer)
				GameManager.Instance.Score += 300;

			EnemyManager.Instance.EnemyDestroyed(this, killedByPlayer);
			IsDead = true;

			window.Destroy(this);
		}

		public override void OnDestroy()
		{

		}
	}
}
