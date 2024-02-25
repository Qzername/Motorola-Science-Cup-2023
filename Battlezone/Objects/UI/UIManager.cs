using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Objects;
using VGE.Windows;

namespace Battlezone.Objects.UI
{
	public class UIManager : VectorObject
	{
		public static UIManager Instance;

		Text score;
		WeaponScope reloading;
		LiveCounter lives; 

		Resolution currentResolution;

		public override Setup Start()
		{
			Instance = this;

			score = new Text("Score: 0000", 2, new SKPoint(10, 42));
			window.Instantiate(score);

			lives = new LiveCounter();
			window.Instantiate(lives);

			reloading = new WeaponScope();
			window.Instantiate(reloading);

			RefreshUI();

			return new()
			{
				Name = "UIManager"
			};
		}

		public override void Update(float delta)
		{
			var resolution = window.GetResolution();

			if (resolution != currentResolution)
			{
				currentResolution = resolution;
				RefreshUI();
			}
		}

		/// <summary>
		/// Odświeżenie UI, wszystkich statystyk
		/// </summary>
		public void RefreshUI()
		{
			score.SetText($"Score      {GameManager.Score}");

			reloading.IsReloading = GameManager.IsReloading;

			//ustawianie pozycji
			score.SetPosition(new Point(currentResolution.Width * 0.75f-104, score.Transform.Position.Y));
			lives.SetPosition(new Point(currentResolution.Width * 0.75f-104, lives.Transform.Position.Y));
		}

		public override bool OverrideRender(Canvas canvas)
		{
			return true;
		}
	}
}
