using HML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGE;
using VGE.Graphics;
using VGE.Objects;
using VGE.Windows;

namespace Asteroids.Objects.UI
{
    public class Scoreboard : VectorObject
    {
        const int yLetterOffset =35, yOffset = -185, xOffset = -150;

        Text[] scoreboardTexts;

        public bool ShowScoreboard
        {
            set
            {
                for(int i = 0; i < scoreboardTexts.Length; i++)
                    scoreboardTexts[i].IsEnabled = value;
            }
        }

        Resolution resolution;

        public override Setup Start()
        {
            scoreboardTexts = new Text[11];

            for (int i = 0; i< scoreboardTexts.Length; i++)
            {
                var text = new Text("", 2f, new());
                window.Instantiate(text);
                scoreboardTexts[i] = text;
            }

            Refresh();

            return new()
            {
                Name = "Scoreboard"
            };
        }

        public override void Update(float delta)
        {
            var currRes = window.GetResolution();

            if (resolution.Width != currRes.Width || resolution.Height != currRes.Height)
            {
                resolution = window.GetResolution();
                Refresh();
            }
        }

        public void Refresh()
        {
            var highscores = HighscoreManager.GetScores();

            if (highscores.Length < 1)
                return;


            var basePosition = new Point(resolution.Width / 2 + xOffset, resolution.Height / 2 + yOffset);

            scoreboardTexts[0].SetText(" HIGH     SCORES");
            scoreboardTexts[0].SetPosition(basePosition);

            for (int i = 1; i < scoreboardTexts.Length; i++)
            {
                if (highscores.Length < i)
                    break;

                string score = highscores[i - 1].Score.ToString();

                for (int j = score.Length; j < 5; j++)
                    score += "  ";

                scoreboardTexts[i].SetText($"{i}. " + (i==10?"":"  ") + $"{score}   {highscores[i - 1].Name}");
                scoreboardTexts[i].SetPosition(basePosition + new Point(0, yLetterOffset * i, 0));
            }
        }

        public override bool OverrideRender(Canvas canvas)
        {
            return true;
        }
    }
}
