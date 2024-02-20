using SkiaSharp;
using VGE.Graphics;
using VGE.Resources;

namespace VGE.Objects
{
    public class Text : VectorObject
    {
        public bool IsEnabled = true;

        ShapeSet alphabet;
        string currentText;
        float fontSize;

        Point startPosition;

        public Text(string text, float fontSize, Point startPosition)
        {
            SetText(text);
            this.fontSize = fontSize;
            this.startPosition = startPosition;
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

        public void SetText(string text) => currentText = text.ToUpper();

        public override void Update(float delta)
        {
        }

        public override bool OverrideRender(Canvas canvas)
        {
            if (!IsEnabled)
                return true;

            float offset = 0;

            for(int i = 0; i < currentText.Length; i++)
            {
                float maxRight = 0;

                if (currentText[i] == ' ')
                {
                    offset += fontSize * 6f;
                    continue;
                }

                foreach (var shape in alphabet.Set[currentText[i].ToString()])
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

                offset += maxRight + fontSize/2;
            }

            return true;
        }
    }
}
