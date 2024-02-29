using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest.Objects
{
	public class MapManager : VectorObject
	{
		public static MapManager Instance;

		private Resolution _baseResolution = new(0, 0);
		private Point CenterOfScreen => new(_baseResolution.Width / 2f, _baseResolution.Height / 2f);

		private List<MapElement> _elements = new();
		public List<MapElement> Elements => _elements;

		private Point _perspectiveOffset;
		public Point PerspectiveOffset => _perspectiveOffset;

		/// <summary>
		/// Pełny obliczony punkt perspektywiczny
		/// </summary>
		public Point PerspectivePoint => CenterOfScreen + PerspectiveOffset;

		public override Setup Start()
		{
			_elements = new List<MapElement>();
			_perspectiveOffset = new Point(0, 0, 300);

			Instance = this;

			LoadMap(GameManager.Instance.CurrentLevel);

			return new Setup()
			{
				Name = "Map",
			};
		}

		void LoadMap(Level level)
		{
			for (int i = 0; i < level.Layout.Length; i++)
			{
				if (!level.IsClosed && i + 1 == level.Layout.Length)
					continue;

				// Nastepny punkt, jezeli nie istnieje - uzyj pierwszego (aby figura miala koniec)
				Point nextPoint = i + 1 < level.Layout.Length ? level.Layout[i + 1] : level.Layout[0];

				// Oblicz odleglosc i kat miedzy punktami
				float distance = MathF.Sqrt(MathF.Pow((nextPoint.X - level.Layout[i].X), 2) + MathF.Pow((nextPoint.Y - level.Layout[i].Y), 2));
				float tan = MathF.Atan2(nextPoint.Y - level.Layout[i].Y, nextPoint.X - level.Layout[i].X);
				float rotation = -1 * ((tan * MathTools.Rad2deg) % 360);

				MapElement element = new MapElement();
				element.Setup(level.Layout[i], distance, rotation);
				window.Instantiate(element);
				_elements.Add(element);
			}

			TempestScene.Instance.ChangePerspectivePoint(PerspectivePoint);
		}

		public override void Update(float delta)
		{
			var currentWindowResolution = window.GetResolution();

			if (_baseResolution.Height != currentWindowResolution.Height || _baseResolution.Width != currentWindowResolution.Width)
			{
				_baseResolution = window.GetResolution();
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

		public override void OnDestroy()
		{
			foreach (var element in Elements)
				window.Destroy(element);

			Elements.Clear();
		}
	}
}
