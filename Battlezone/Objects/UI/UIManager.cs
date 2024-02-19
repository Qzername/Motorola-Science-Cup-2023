using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Objects;

namespace Battlezone.Objects.UI
{
    public class UIManager : VectorObject
    {
        public static UIManager Instance;

        Text score;

        public override Setup Start()
        {
            Instance = this;

            score = new Text("Score: 0000", 2, new SKPoint(10, 10));
            window.Instantiate(score);

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
        }

        public override bool OverrideRender(Canvas canvas)
        {
            return true;
        }
    }
}
