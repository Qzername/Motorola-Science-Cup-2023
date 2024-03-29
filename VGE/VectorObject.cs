﻿using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Windows;

namespace VGE
{
    /// <summary>
    /// Obiekt w grze
    /// </summary>
    public abstract class VectorObject
    {
        string name;

        /// <summary>
        /// Nazwa obiektu
        /// </summary>
        public string Name
        {
            get => name;
        }

        IShape shape;
        /// <summary>
        /// Kształt obiektu
        /// </summary>
        public IShape Shape
        {
            get => shape;
            set => shape = value;
        }

        protected Transform transform;
        /// <summary>
        /// Pozycja wraz z rotacją obiektu
        /// </summary>
        public Transform Transform
        {
            get => transform;
        }

        Guid guid;
        /// <summary>
        /// Identifikator obiektu
        /// </summary>
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
        /// Wywołuje się kiedy obiekt ma zostać zniszczony
        /// </summary>
        public virtual void OnDestroy()
        {

        }

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
