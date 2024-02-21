using SkiaSharp;
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

            float offset = 0;// textAlignment == TextAlignment.Left ? 0 : fontSize * currentText.Length;

            string tempText = textAlignment == TextAlignment.Left ? currentText : currentTextReverse;

            for(int i = 0; i < tempText.Length; i++)
            {
                float maxRight = 0;

                if (tempText[i] == ' ')
                {
                    offset += fontSize * 6f * (int)textAlignment;
                    continue;
                }

                foreach (var shape in alphabet.Set[tempText[i].ToString()])
                {
                    if(shape is null)
                        continue;

                    if (shape.BottomRight.X > maxRight)
                        maxRight = shape.BottomRight.X * fontSize;

                    foreach (var l in shape.CompiledShape)
                    {
                        var start = new Point(l.StartPosition.X * fontSize + offset, l.StartPosition.Y * fontSize);
                        var end = new Point(l.EndPosition.X * fontSize + offset, l.EndPosition.Y * fontSize);

                        canvas.DrawLine(new Line(start + transform.Position, end + transform.Position));
                    }
                }

                offset += (maxRight + fontSize/2) * (int)textAlignment;
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
