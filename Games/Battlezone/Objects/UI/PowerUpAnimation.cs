using VGE;
using VGE.Graphics;
using VGE.Objects;

namespace Battlezone.Objects.UI
{
    public class PowerUpAnimation : VectorObject
    {
        Text powerUpText;

        float animTimer = 0, animTimerMax = 0.2f;
        float deathTimer = 0, deathTimerMax = 1f;

        public override Setup Start()
        {
            powerUpText = new Text("POWER UP", 2f, new Point(200, 200));
            window.Instantiate(powerUpText);

            return new()
            {
                Name = "PowerUpAnimation"
            };
        }

        public override void Update(float delta)
        {
            //mryganie tekstu
            animTimer += delta;

            if (animTimer > animTimerMax / 2 && powerUpText.IsEnabled)
                powerUpText.IsEnabled = false;

            if (animTimer > animTimerMax)
            {
                powerUpText.IsEnabled = true;
                animTimer = 0f;
            }

            //śmierć animacji
            deathTimer += delta;

            if (deathTimer > deathTimerMax)
            {
                window.Destroy(powerUpText);
                window.Destroy(this);
            }

            //pozycjonowanie
            var res = window.GetResolution();

            powerUpText.SetPosition(new Point(res.Width / 2 - 80, res.Height / 2 - 15));
        }

        public override bool OverrideRender(Canvas canvas)
        {
            return true;
        }
    }
}
