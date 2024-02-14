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
                element.Setup(new Point(i * 100 + offset, 150, 750));
                elements.Add(element);
            }
        }

        public override void Update(float delta)
        {
            var currentWindowResolution = window.GetResolution();

            if(baseResolution.Height != currentWindowResolution.Height || baseResolution.Width != currentWindowResolution.Width)
            {
                TempestScene.Instance.ChangePerspectivePoint(PerspectivePoint);
                baseResolution = window.GetResolution();
            }
        }

        public override bool OverrideRender(Canvas canvas)
        {
            //nie rysuj nic
            return true;
        }

        /// <summary>
        /// Przeliczenie pozycji na której powinien znajdować się obiekt na podanym prostokącie i koordynacie Z
        /// </summary>
        public Point GetPosition(int mapPosition, float Z)
        {
            Point p = Elements[mapPosition].GetCenterPosition();

            p.Z = Z;

            return p;
        }
    }
}
