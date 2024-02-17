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
        public override int PhysicsLayer => mapPosition;
        int mapPosition; // Tile na którym jest gracz

        public override void OnCollisionEnter(PhysicsObject other)
        {

        }

        public override Setup Start()
        {
            return new Setup()
            {
                Name = "Player",
                Shape = new PointShape(GameManager.Configuration.Player,
                                new Point(20, 20,0),
                                new Point(20, -20,0),
                                new Point(-20, -20,0),
                                new Point(-20, 20,0)),
                Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z) + new Point(0,0,400),
                Rotation = MapManager.Instance.Elements[mapPosition].Transform.Rotation
            };
        }

        bool wasLeftPressed, wasRightPressed, wasSpacePressed;

        public override void Update(float delta)
        {
            if ((window.KeyDown(Key.A) || window.KeyDown(Key.Left)) && !wasLeftPressed)
				wasLeftPressed = true;
            else if (!(window.KeyDown(Key.A) || window.KeyDown(Key.Left)) && wasLeftPressed)
            {
				wasLeftPressed = false;

                if (mapPosition != 0)
                {
                    mapPosition--;
                    transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
                }
                else if (GameManager.IsLevelClosed)
                {
                    mapPosition = MapManager.Instance.Elements.Count - 1;
					transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
				}

                Rotate(transform.Rotation * -1);
				Rotate(MapManager.Instance.Elements[mapPosition].Transform.Rotation);
			}

            if ((window.KeyDown(Key.D) || window.KeyDown(Key.Right)) && !wasRightPressed)
				wasRightPressed = true;
            else if (!(window.KeyDown(Key.D) || window.KeyDown(Key.Right)) && wasRightPressed)
            {
				wasRightPressed = false;

                if (mapPosition != MapManager.Instance.Elements.Count - 1)
                {
                    mapPosition++;
                    transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
                }
				else if (GameManager.IsLevelClosed)
				{
					mapPosition = 0;
					transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
				}

				Rotate(transform.Rotation * -1);
				Rotate(MapManager.Instance.Elements[mapPosition].Transform.Rotation);
			}

            if (window.KeyDown(Key.Space) && !wasSpacePressed)
                wasSpacePressed = true;
            else if (!window.KeyDown(Key.Space) && wasSpacePressed)
            {
                wasSpacePressed = false;

                Bullet bullet = new Bullet();
                window.Instantiate(bullet);
                bullet.Setup(mapPosition, transform.Position.Z);
			}
		}
    }
}
