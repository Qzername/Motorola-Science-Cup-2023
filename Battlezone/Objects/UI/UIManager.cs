using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Objects;

namespace Battlezone.Objects.UI
{
	public class UIManager : VectorObject
	{
		public static UIManager Instance;

		Text score, lives, reloading;

		public override Setup Start()
		{
			Instance = this;

			score = new Text("Score: 0000", 2, new SKPoint(10, 10));
			window.Instantiate(score);

			lives = new Text("Lives: 3", 2, new SKPoint(10, 37));
			window.Instantiate(lives);

			reloading = new Text("", 2, new Point(10, 64));
			window.Instantiate(reloading);

			return new()
			{
				Name = "UIManager"
			};
		}

		public override void Update(float delta)
		{
		}


		public void RefreshUI()
		{
			score.SetText($"Score: {GameManager.Score}");
			lives.SetText($"Lives: {GameManager.Lives}");

			reloading.SetText(GameManager.IsReloading ? "RELOADING" : "");
		}

		public override bool OverrideRender(Canvas canvas)
		{
			return true;
		}
	}
}
