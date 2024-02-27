using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGE;
using VGE.Graphics;

namespace Battlezone.Objects.UI
{
    public class DeathAnimation : VectorObject
    {
        Player player;

        readonly Dictionary<int, Line[]> deathFrames = new Dictionary<int, Line[]>()
        {
            {0, [
                    new(new(0,0), new(0.25f, -0.3f)),
                    new(new(0,0), new(0.3f, 0.1f)),
                    new(new(0,0), new(-0.1f, 0.2f)),
                    new(new(0,0), new(-0.20f, 0f)),
                    new(new(0,0), new(-0.25f, -0.25f)),
            ] },
            {1, [
                    //1
                    new(new(0.25f, -0.3f),new(0.3f, -0.60f)),
                    new(new(0.25f, -0.3f),new(0.45f, -0.4f)),
                    //2
                    new(new(0.3f, 0.1f),new(0.45f, 0.1f)),
                    //3
                    new(new(-0.1f, 0.2f), new(0.1f,0.3f)),
                    //4
                    new(new(-0.20f, 0f), new(-0.25f, 0.1f)),
                    new(new(-0.40f, 0.15f), new(-0.25f, 0.1f)),
                    //5
                    new(new(-0.5f,-0.35f), new(-0.25f, -0.25f)),
            ] },
            {2, [
                    //1
                    new(new(0.45f, -0.4f),new(0.45f, -0.80f)),
                    new(new(0.45f, -0.4f),new(0.85f, -0.8f)),
                    //2
                    new(new(0.45f, 0.1f),new(0.60f, 0.1f)),
                    new(new(0.6f, 0.1f),new(0.60f, 0.2f)),
                    //3
                    new(new(0.1f,0.3f),new(0.2f, 0.7f)),
                    //4
                    new(new(-0.40f, 0.15f), new(-0.6f, 0.1f)),
                    new(new(-0.40f, 0.15f), new(-0.5f, 0.4f)),
                    //5
                    new(new(-0.5f,-0.35f), new(-0.55f, -0.65f)),
            ] },
            {3, [
                    //1.1
                    new(new(0.45f, -0.80f),new(0.3f, -1f)),
                    new(new(0.45f, -0.80f),new(0.6f, -1f)),
                    //1.2
                    new(new(0.85f, -0.8f),new(1f, -0.8f)),
                    //2
                    new(new(0.60f, 0.2f),new(0.80f, 0f)),
                    new(new(0.60f, 0.2f),new(0.60f, 0.8f)),
                    //3
                    new(new(0.2f, 0.7f), new(0f,1f)),
                    //4
                    //5
                    new(new(-1f,-0.45f), new(-0.55f, -0.65f)),
                    new(new(-0.6f,-0.45f), new(-0.55f, -0.65f)),
            ] },
        };

        float animationMax = 1f;
        float animationTimer;

        int frames => deathFrames.Keys.Count;

        public DeathAnimation(Player player)
        {
            this.player = player;
        }

        public override Setup Start()
        {
            return new()
            {
                Name = "DeathAnimation"
            };
        }

        public override void Update(float delta)
        {
            animationTimer += delta;
           
            if(animationTimer > animationMax)
            {
                GameManager.Instance.Lives--;

                player.Respawn();
                window.Destroy(this);
            }
        }

        public override bool OverrideRender(Canvas canvas)
        {
            var res = window.GetResolution();
            Point centerOfScreen = new Point(res.Width / 2, res.Height / 2);

            int framesToRender = Convert.ToInt32(animationTimer / (animationMax / frames));

            for(int i = 0; i<framesToRender;i++)
            {
                foreach (var line in deathFrames[i])
                    canvas.DrawLine(new Line(centerOfScreen + new Point(line.StartPosition.X * res.Width, line.StartPosition.Y * res.Height),
                                             centerOfScreen + new Point(line.EndPosition.X * res.Width, line.EndPosition.Y * res.Height), SKColors.Green));
            }

            return true;
        }

    }
}
