using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest.Objects
{
    public class MapManager : VectorObject
    {
        public static MapManager Instance;

        List<MapElement> elements = new List<MapElement>();
        public List<MapElement> Elements { get => elements; }

        Point perspectiveOffset = new Point(0, 0, 500f);
        public Point PerspectiveOffset { get => perspectiveOffset; }

        public override Setup Start()
        {
            Instance = this;

            LoadMap();

            return new Setup()
            {
                Name = "Map",
            };
        }

        void LoadMap()
        {
            //TODO: Map loader
            int mapLength = 5;
            int offset = mapLength/2 * -100;

            for (int i = 0; i < mapLength; i++)
            {
                MapElement element = new MapElement();
                window.Instantiate(element);
                element.Setup(new Point(i * 100 + offset, 150, 750), perspectiveOffset);
                elements.Add(element);
            }
        }

        public override void Update(float delta)
        {
        }

        public override void RefreshGraphics(Canvas canvas)
        {
            //dont draw anything
        }
    }
}
