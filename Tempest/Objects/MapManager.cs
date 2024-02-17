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

        Resolution baseResolution = new(0, 0);
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
			elements = new List<MapElement>();
            perspectiveOffset = new Point(0, 0, 500);

			Instance = this;

            LoadMap(Levels.Infinity);

            return new Setup()
            {
                Name = "Map",
            };
        }

        void LoadMap(Point[] layout, bool shouldClose = true, float perspectivePointY = 0, float perspectivePointZ = 500)
        {
            for (int i = 0; i < layout.Length; i++)
            {
	            if (!shouldClose && i + 1 == layout.Length)
		            continue;
                    
                // Nastepny punkt, jezeli nie istnieje - uzyj pierwszego (aby figura miala koniec)
                Point nextPoint = i + 1 < layout.Length ? layout[i + 1] : layout[0];

                // Oblicz odleglosc i kat miedzy punktami
				float distance = MathF.Sqrt(MathF.Pow((nextPoint.X - layout[i].X), 2) + MathF.Pow((nextPoint.Y - layout[i].Y), 2));
				float tan = MathF.Atan2(nextPoint.Y - layout[i].Y, nextPoint.X - layout[i].X);
				float rotation = -1 * ((tan * MathTools.Rad2deg) % 360);

				MapElement element = new MapElement();
				element.Setup(layout[i], distance, rotation);
                window.Instantiate(element);
                elements.Add(element);
            }

            GameManager.IsLevelClosed = shouldClose;
			perspectiveOffset.Y = perspectivePointY;
			perspectiveOffset.Z = perspectivePointZ;
			TempestScene.Instance.ChangePerspectivePoint(PerspectivePoint);
		}

        public override void Update(float delta)
        {
            var currentWindowResolution = window.GetResolution();

            if(baseResolution.Height != currentWindowResolution.Height || baseResolution.Width != currentWindowResolution.Width)
            {
                baseResolution = window.GetResolution();
                TempestScene.Instance.ChangePerspectivePoint(PerspectivePoint);
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
