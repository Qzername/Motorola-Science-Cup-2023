using SkiaSharp;
using System.Diagnostics;
using VGE.Graphics;
using VGE.Resources;

namespace VGE.Objects
{
    public class Text : VectorObject
    {
        public bool IsEnabled = true;

        public TextAlignment textAlignment;

        ShapeSet alphabet;
        string currentText, currentTextReverse;
        float fontSize;

        Point startPosition;

        public Text(string text, float fontSize, Point startPosition, TextAlignment textAlignment = TextAlignment.Left)
        {
            SetText(text);
            this.fontSize = fontSize;
            this.startPosition = startPosition;
            this.textAlignment = textAlignment;
        }

        public override Setup Start()
        {
            alphabet = ResourcesHandler.GetShapeSet("alphabet");

            return new Setup()
            {
                Name = "Text",
                Shape = null,
                Position = startPosition,
                Rotation = Point.Zero,
            };
        }

        public void SetText(string text)
        {
            currentText = text.ToUpper();
            currentTextReverse = text.ToUpper();    

            var reverse = currentTextReverse.ToCharArray();
            Array.Reverse(reverse);
            currentTextReverse = new string(reverse);
        }

        public void SetPosition(Point position)
        {
            transform.Position = position;
        }

        public override void Update(float delta)
        {
        }

        public override bool OverrideRender(Canvas canvas)
        {
            if (!IsEnabled)
                return true;

            float xOffset = 0, yOffset = 0;

            string tempText = textAlignment == TextAlignment.Left ? currentText : currentTextReverse;

            for(int i = 0; i < tempText.Length; i++)
            {
                float maxRight = 0;

                if (tempText[i] == '\n')
                {
                    yOffset += fontSize * 14f;
                    xOffset = 0;
                    continue;
                }

                if (tempText[i] == ' ')
                {
                    xOffset += fontSize * 6f * (int)textAlignment;
                    continue;
                }

                foreach (var shape in alphabet.Set[tempText[i].ToString()])
                {
                    if(shape is null)
                        continue;

                    foreach (var l in shape.CompiledShape)
                    {
                        if (l.StartPosition.X > maxRight)
                            maxRight = l.StartPosition.X;

                        if(l.EndPosition.X > maxRight)
                            maxRight = l.EndPosition.X;

                        var start = new Point(l.StartPosition.X * fontSize + xOffset, l.StartPosition.Y * fontSize + yOffset);
                        var end = new Point(l.EndPosition.X * fontSize + xOffset, l.EndPosition.Y * fontSize + yOffset);

                        canvas.DrawLine(new Line(start + transform.Position, end + transform.Position));
                    }
                }

                xOffset += ((maxRight* fontSize) + fontSize/2) * (int)textAlignment;
            }

            return true;
        }

        public enum TextAlignment
        {
            Left = 1,
            Right = -1,
        }
    }
}
