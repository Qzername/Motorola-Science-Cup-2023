using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;

namespace Tempest.Objects
{
	public class Player : PhysicsObject
	{
		public override int PhysicsLayer => GameManager.MapPosition;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != GameManager.MapPosition || GameManager.StopGame)
				return;

			// Jesli obiekt nie jest pociskiem (czyli wrogiem lub pociskiem wroga), to gracz traci zycie i zaczyna poziom od nowa
			if (other.Name != "Bullet")
			{
				GameManager.Lives--;
				((GameWindow)window).StartLevel();
			}
		}

		public override Setup Start()
		{
			return new Setup()
			{
				Name = "Player",
				Shape = new PointShape(GameManager.LevelConfig.Player,
								new Point(0, -20, 0),
								new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / 2, 0, 0),
								new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / -2, 0, 0),
								new Point(0, -20, 0)),
				Position = MapManager.Instance.GetPosition(GameManager.MapPosition, transform.Position.Z) + new Point(0, 0, 400),
				Rotation = MapManager.Instance.Elements[GameManager.MapPosition].Transform.Rotation
			};
		}

		private bool _wasLeftPressed, _wasRightPressed, _wasSpacePressed;

		public override void Update(float delta)
		{
			if (GameManager.StopGame)
				return;

			if ((window.KeyDown(Key.A) || window.KeyDown(Key.Left)) && !_wasLeftPressed)
				_wasLeftPressed = true;
			else if (!(window.KeyDown(Key.A) || window.KeyDown(Key.Left)) && _wasLeftPressed)
			{
				_wasLeftPressed = false;

				if (GameManager.MapPosition != 0)
				{
					GameManager.MapPosition--;
					transform.Position = MapManager.Instance.GetPosition(GameManager.MapPosition, transform.Position.Z);
				}
				else if (GameManager.LevelConfig.IsClosed)
				{
					GameManager.MapPosition = MapManager.Instance.Elements.Count - 1;
					transform.Position = MapManager.Instance.GetPosition(GameManager.MapPosition, transform.Position.Z);
				}

				Shape = new PointShape(GameManager.LevelConfig.Player,
					new Point(0, -20, 0),
					new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / 2, 0, 0),
					new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / -2, 0, 0),
					new Point(0, -20, 0));
				Rotate(MapManager.Instance.Elements[GameManager.MapPosition].Transform.Rotation);
			}

			if ((window.KeyDown(Key.D) || window.KeyDown(Key.Right)) && !_wasRightPressed)
				_wasRightPressed = true;
			else if (!(window.KeyDown(Key.D) || window.KeyDown(Key.Right)) && _wasRightPressed)
			{
				_wasRightPressed = false;

				if (GameManager.MapPosition != MapManager.Instance.Elements.Count - 1)
				{
					GameManager.MapPosition++;
					transform.Position = MapManager.Instance.GetPosition(GameManager.MapPosition, transform.Position.Z);
				}
				else if (GameManager.LevelConfig.IsClosed)
				{
					GameManager.MapPosition = 0;
					transform.Position = MapManager.Instance.GetPosition(GameManager.MapPosition, transform.Position.Z);
				}

				Shape = new PointShape(GameManager.LevelConfig.Player,
					new Point(0, -20, 0),
					new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / 2, 0, 0),
					new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / -2, 0, 0),
					new Point(0, -20, 0));
				Rotate(MapManager.Instance.Elements[GameManager.MapPosition].Transform.Rotation);
			}

			if (window.KeyDown(Key.Space) && !_wasSpacePressed)
				_wasSpacePressed = true;
			else if (!window.KeyDown(Key.Space) && _wasSpacePressed)
			{
				_wasSpacePressed = false;

				Bullet bullet = new Bullet();
				window.Instantiate(bullet);
				bullet.Setup(GameManager.MapPosition, transform.Position.Z + 50);
			}
		}
	}
}
