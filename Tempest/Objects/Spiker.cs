using System.Diagnostics;
using System.Timers;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using Timer = System.Timers.Timer;

namespace Tempest.Objects
{
	public class Spiker : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition = -1;
		private SpikerLine _spikerLine;

		private const float ZSpeed = 350f;
		private bool _turnAround;

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition)
				return;

			if (other.Name == "Bullet" && !_turnAround)
				_turnAround = true;
		}

		public override Setup Start()
		{
			if (_mapPosition == -1)
				_mapPosition = GameManager.MapPosition;

			_spikerLine = new();
			_spikerLine.Setup(_mapPosition);
			window.Instantiate(_spikerLine);

			return new Setup()
			{
				Name = "Spiker",
				Shape = new PointShape(GameManager.Configuration.Spiker,
								new Point(0, 20, 0),
								new Point(20, 0, 0),
								new Point(0, -20, 0),
								new Point(-20, 0, 0)),
				Position = MapManager.Instance.GetPosition(_mapPosition, transform.Position.Z) + new Point(0, 0, 1600),
				Rotation = MapManager.Instance.Elements[_mapPosition].Transform.Rotation
			};
		}

		public void Setup(int mapPosition)
		{
			_mapPosition = mapPosition;
		}

		public override void Update(float delta)
		{
			if (!_turnAround)
			{
				transform.Position.Z -= ZSpeed * delta;
				_spikerLine.Length -= ZSpeed * delta;
				
				if (transform.Position.Z < 0)
				{
					Tanker tanker = new();
					tanker.Setup(_mapPosition);
					window.Instantiate(tanker);
					window.Destroy(this);
				}
			}
			else
			{
				transform.Position.Z += ZSpeed * delta;

				if (transform.Position.Z > 1600)
					window.Destroy(this);
			}
			
			_spikerLine.Shape = new PointShape(GameManager.Configuration.Spiker,
				new Point(0, 0, 0),
				new Point(0, 0,  _spikerLine.Length));
		}
	}
}
