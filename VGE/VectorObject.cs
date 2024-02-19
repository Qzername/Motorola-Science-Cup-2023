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
using VGE.Graphics.Shapes;
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

        IShape shape;
        public IShape Shape
        {
            get => shape;
            set => shape = value;
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
                Rotation = Point.Zero,
            };

            if (setup.Rotation.X + setup.Rotation.Y + setup.Rotation.Z != 0)
                Rotate(setup.Rotation);
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
        /// Nadpisywanie renderu, wywołuje się co klatke kiedy obiekt się renderuje 
        /// </summary>
        /// <returns>Czy anulować rysowanie obiektu przez scene?</returns>
        public virtual bool OverrideRender(Canvas canvas)
        {
            return false;
        }

        /// <summary>
        /// Obrót figury wraz z obrotem kszałtu
        /// </summary>
        protected void Rotate(Point rotation)
        {
            shape.Rotate(rotation);
            transform.Rotation += rotation;
        }
        
        public struct Setup
        {
            public string Name;
            public IShape Shape;
            public Point Position; 
            public Point Rotation;

            public Setup(string name, IShape shape, Point position, Point rotation)
            {
                Name = name;
                Shape = shape;
                Position = position;
                Rotation = rotation;
            }
        }
    }
}
