using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
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

        }

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Player",
                Shape = new PointShape(GameManager.Configuration.Player,
                                new Point(0, -20,0),
                                new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / 2, 0,0),
                                new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / -2, 0,0),
                                new Point(0, -20,0)),
                Position = MapManager.Instance.GetPosition(GameManager.MapPosition, transform.Position.Z) + new Point(0,0,400),
                Rotation = MapManager.Instance.Elements[GameManager.MapPosition].Transform.Rotation
            };
        }

        private bool wasLeftPressed, wasRightPressed, wasSpacePressed;

        public override void Update(float delta)
        {
            if ((window.KeyDown(Key.A) || window.KeyDown(Key.Left)) && !wasLeftPressed)
				wasLeftPressed = true;
            else if (!(window.KeyDown(Key.A) || window.KeyDown(Key.Left)) && wasLeftPressed)
            {
				wasLeftPressed = false;

                if (GameManager.MapPosition != 0)
                {
                    GameManager.MapPosition--;
                    transform.Position = MapManager.Instance.GetPosition(GameManager.MapPosition, transform.Position.Z);
                }
                else if (GameManager.IsLevelClosed)
                {
                    GameManager.MapPosition = MapManager.Instance.Elements.Count - 1;
					transform.Position = MapManager.Instance.GetPosition(GameManager.MapPosition, transform.Position.Z);
				}

				Shape = new PointShape(GameManager.Configuration.Player,
					new Point(0, -20, 0),
					new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / 2, 0, 0),
					new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / -2, 0, 0),
					new Point(0, -20, 0));
				Rotate(MapManager.Instance.Elements[GameManager.MapPosition].Transform.Rotation);
			}

            if ((window.KeyDown(Key.D) || window.KeyDown(Key.Right)) && !wasRightPressed)
				wasRightPressed = true;
            else if (!(window.KeyDown(Key.D) || window.KeyDown(Key.Right)) && wasRightPressed)
            {
				wasRightPressed = false;

                if (GameManager.MapPosition != MapManager.Instance.Elements.Count - 1)
                {
                    GameManager.MapPosition++;
                    transform.Position = MapManager.Instance.GetPosition(GameManager.MapPosition, transform.Position.Z);
                }
				else if (GameManager.IsLevelClosed)
				{
					GameManager.MapPosition = 0;
					transform.Position = MapManager.Instance.GetPosition(GameManager.MapPosition, transform.Position.Z);
				}

				Shape = new PointShape(GameManager.Configuration.Player,
					new Point(0, -20, 0),
					new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / 2, 0, 0),
					new Point(MapManager.Instance.Elements[GameManager.MapPosition].Length / -2, 0, 0),
					new Point(0, -20, 0));
				Rotate(MapManager.Instance.Elements[GameManager.MapPosition].Transform.Rotation);
			}

            if (window.KeyDown(Key.Space) && !wasSpacePressed)
                wasSpacePressed = true;
            else if (!window.KeyDown(Key.Space) && wasSpacePressed)
            {
                wasSpacePressed = false;

                Bullet bullet = new Bullet();
                window.Instantiate(bullet);
                bullet.Setup(GameManager.MapPosition, transform.Position.Z);
			}
		}
    }
}
