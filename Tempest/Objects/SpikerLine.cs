using System.Diagnostics;
using System.Timers;
using VGE;
using VGE.Graphics.Shapes;
using VGE.Physics;
using Timer = System.Timers.Timer;

namespace Tempest.Objects
{
	public class SpikerLine : PhysicsObject
	{
		public override int PhysicsLayer => _mapPosition;
		private int _mapPosition;

		private float _length;
		public float Length
		{
			get => _length;
			set => _length = value;
		}

		public override void OnCollisionEnter(PhysicsObject other)
		{
			if (other.PhysicsLayer != _mapPosition)
				return;

			if (other.Name == "Bullet")
			{
				_length += 100;
				Shape = new PointShape(GameManager.Configuration.Spiker,
					new Point(0, 0, 0),
					new Point(0, 0, _length));
				
				if (_length >= 0)
					window.Destroy(this);
			}
		}

		public override Setup Start()
		{
			return new Setup()
			{
				Name = "SpikerLine",
				Shape = new PointShape(GameManager.Configuration.Spiker,
								new Point(0, 0, 0),
								new Point(0, 0, _length)),
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

		}
	}
}
