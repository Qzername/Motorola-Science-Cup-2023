using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VGE.Graphics;
using VGE.Windows;

namespace VGE
{
    public abstract class VectorObject
    {
        string name;
        public string Name
        {
            get => name;
        }

        Shape shape;
        public Shape Shape
        {
            get => shape;
        }

        protected Transform transform;
        public Transform Transform
        {
            get => transform;
        }

        Guid guid;
        public Guid Guid
		{
			get => guid;
		}

        protected Window window;

        public VectorObject()
        {
            guid = Guid.NewGuid();
        }

        public void Initialize(Window window)
        {
            this.window = window;

            var setup = Start();

            name = setup.Name;
            shape = setup.Shape;
            transform = new Transform()
            {
                Position = setup.Position,
                Rotation = setup.Rotation,
            };

            if (transform.Rotation != 0f)
                shape.Rotate(transform.Rotation);
        }

        /// <summary>
        /// Wywołuje się przy utworzeniu obiektu, po zarejestrowaniu go w oknie.
        /// </summary>
        public abstract Setup Start();
        /// <summary>
        /// Wywołuje się co każdą klatke
        /// </summary>
        public abstract void Update(float delta);

        /// <summary>
        /// Rysowanie obiektu na canvasie
        /// Używaj/overrideuj tylko jeżeli wiesz co robisz
        /// </summary>
        public virtual void RefreshGraphics(Canvas canvas)
        {
            foreach (var l in shape.CompiledShape)
                canvas.DrawLine(new Line(l.StartPosition + transform.Position, l.EndPosition + transform.Position));
        }

        /// <summary>
        /// Obrót figury wraz z obrotem kszałtu
        /// </summary>
        protected void Rotate(float angle)
        {
            shape.Rotate(angle);
            transform.Rotation = (transform.Rotation + angle) % 360;
        }
        
        public struct Setup
        {
            public string Name;
            public Shape Shape;
            public SKPoint Position; 
            public float Rotation;

            public Setup(string name, Shape shape, SKPoint position, float rotation)
            {
                Name = name;
                Shape = shape;
                Position = position;
                Rotation = rotation;
            }
        }
    }
}
