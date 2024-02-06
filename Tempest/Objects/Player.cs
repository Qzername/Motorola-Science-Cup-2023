using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest.Objects
{
    public class Player : VectorObject
    {
        Resolution baseResolution;
        Point centerOfScreen { get => new Point(baseResolution.Width / 2f, baseResolution.Height / 2f); }

        int mapPosition = 0;

        public override Setup Start()
        {
            baseResolution = window.GetResolution();

            return new Setup()
            {
                Name = "Player",
                Shape = new Shape(0,SKColors.Red,
                                new Point(20, 20),
                                new Point(20, -20),
                                new Point(-20, -20),
                                new Point(-20, 20)),
                Position = GetPosition(),
                PerspectiveCenter = MapManager.Instance.PerspectiveOffset,
                Rotation = 0f
            };
        }

        bool wasApressed = false, wasDpressed = false;

        public override void Update(float delta)
        {
            if (window.KeyDown(Key.A) && !wasApressed)
                wasApressed = true;
            else if (!window.KeyDown(Key.A) && wasApressed)
            {
                wasApressed = false;

                if (mapPosition != 0)
                {
                    mapPosition--;
                    transform.Position = GetPosition();
                }
            }

            if (window.KeyDown(Key.D) && !wasDpressed)
                wasDpressed = true;
            else if (!window.KeyDown(Key.D) && wasDpressed )
            {
                wasDpressed = false;

                if(mapPosition != MapManager.Instance.Elements.Count - 1)
                {
                    mapPosition++;
                    transform.Position = GetPosition();
                }
            }
        }

        Point GetPosition()
        {
            Point p = centerOfScreen + MapManager.Instance.PerspectiveOffset + MapManager.Instance.Elements[mapPosition].GetCenterPosition();

            Debug.WriteLine(p);
            return p;
        }
    }
}
