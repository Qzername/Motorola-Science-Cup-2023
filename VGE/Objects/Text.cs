using SkiaSharp;
using VGE.Graphics;
using VGE.Resources;

namespace VGE.Objects
{
    public class Text : VectorObject
    {
        ShapeSet alphabet;
        string currentText;
        float fontSize;

        public override Setup Start()
        {
            alphabet = ResourcesHandler.GetShapeSet("alphabet");

            return new Setup()
            {
                Name = "Text",
                Shape = null,
                Position = new Point(0, 0),
                Rotation = Point.Zero,
            };
        }

        public void Setup(string text, float fontSize, Point position)
        {
            SetText(text);
            this.fontSize = fontSize;
            transform.Position = position;
        }

        public void SetText(string text) => currentText = text.ToUpper();

        public override void Update(float delta)
        {
        }

        public override bool OverrideRender(Canvas canvas)
        {
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
