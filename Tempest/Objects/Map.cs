using SkiaSharp;
using VGE;
using VGE.Graphics;

namespace Tempest.Objects
{
    public class Map : VectorObject
    {
        Resolution baseResolution;
        
        float z = 50f;
        const float zSpeed = 10f;

        public override Setup Start()
        {
            baseResolution = window.GetResolution();

            return new Setup()
            {
                Name = "Map",
                Position = new SKPoint(0, 0),
                Shape = new Shape(0, new SKPoint(0,0), new SKPoint(10,0)),
                Rotation = 0f,
            };
        }

        public override void Update(float delta)
        {
            if (window.KeyDown(Key.Up))
                z += zSpeed * delta;
            else if(window.KeyDown(Key.Down))
                z -= zSpeed * delta;
        }

        public override void RefreshGraphics(Canvas canvas)
        {
            
        }
    }
}
