using SkiaSharp;
using System.Diagnostics;
using System.Printing;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest.Objects
{
    public class MapManager : VectorObject
    {
        public static MapManager Instance;

        Resolution baseResolution;
        Point centerOfScreen { get => new Point(baseResolution.Width / 2f, baseResolution.Height / 2f); }

        List<MapElement> elements;
        public List<MapElement> Elements { get => elements; }

        Point perspectiveOffset;
        public Point PerspectiveOffset { get => perspectiveOffset; }

        /// <summary>
        /// Pełny obliczony punkt perspektywiczny
        /// </summary>
        public Point PerspectivePoint => centerOfScreen + PerspectiveOffset;

        public event Action<Point> ResolutionChanged;

        public override Setup Start()
        {
            baseResolution = window.GetResolution();

            elements = new List<MapElement>();
            perspectiveOffset = new Point(0, 0, 500f);

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
            var currentWindowResolution = window.GetResolution();

            if(baseResolution != currentWindowResolution)
            {
                ResolutionChanged?.Invoke(PerspectivePoint);
                baseResolution = window.GetResolution();
            }
        }

        public override void RefreshGraphics(Canvas canvas)
        {
            //dont draw anything
        }

        public Point GetPosition(int mapPosition, float Z)
        {
            Point p = Elements[mapPosition].GetCenterPosition();

            p.Z = Z;

            return p;
        }
    }
}
