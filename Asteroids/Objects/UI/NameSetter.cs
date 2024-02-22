using VGE;
using VGE.Graphics;
using VGE.Objects;
using VGE.Windows;

namespace Asteroids.Objects.UI
{
    public class NameSetter : VectorObject
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        string name;

        int currentLetter = 0, currentIndex = 0;

        Text text, underline;

        public override Setup Start()
        {
            var res = window.GetResolution();

            text = new Text("A", 5f, new Point(res.Width / 2, res.Height / 2));
            underline = new Text("___", 5f, new Point(res.Width / 2, res.Height / 2 + 5f));

            window.Instantiate(text);
            window.Instantiate(underline);

            return new()
            {
                Name = "NameSetter",
            };
        }

        bool lastIsSpacePressed, lastIsWpressed, lastIsSpressed;

        public override void Update(float delta)
        {
            var res = window.GetResolution();

            text.SetPosition(new Point(res.Width / 2-92, res.Height / 2+30f));
            underline.SetPosition(new Point(res.Width / 2-92, res.Height / 2+30f + 5f));

            bool currentIsSpacePressed = window.KeyDown(Key.Space);

            if (!currentIsSpacePressed && lastIsSpacePressed)
            {
                name += alphabet[currentLetter];
                currentIndex++;

                if(currentIndex == 3)
                {
                    GameManager.Instance.FinishedWritingName(name);
                    window.Destroy(underline);
                    window.Destroy(text);
                    window.Destroy(this);
                    return;
                }

                text.SetText(name + alphabet[0]);
            }

            bool currentIsWpressed = window.KeyDown(Key.W);
            bool currentIsSpressed = window.KeyDown(Key.S);

            if(!currentIsWpressed && lastIsWpressed)
            {
                currentLetter++;

                if (currentLetter == alphabet.Length)
                    currentLetter = 0;

                text.SetText(name + alphabet[currentLetter]);
            }
            else if(!currentIsSpressed && lastIsSpressed)
            {
                currentLetter--;

                if(currentLetter == -1)
                    currentLetter = alphabet.Length-1;

                text.SetText(name + alphabet[currentLetter]);
            }

            lastIsSpacePressed = currentIsSpacePressed;
            lastIsWpressed = currentIsWpressed;
            lastIsSpressed = currentIsSpressed;
        }

        public override bool OverrideRender(Canvas canvas)
        {
            return true;
        }
    }
}
