﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VGE.Graphics;

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

        public VectorObject(string name, Shape shape)
        {
            this.name = name;
            this.shape = shape;
            transform = new Transform();

            guid = Guid.NewGuid();
        }
        public VectorObject(string name, Shape shape, Transform transform)
        {
            this.name = name;
            this.shape = shape;
            this.transform = transform;

            if (transform.Rotation != 0f)
                shape.Rotate(transform.Rotation);

            guid = Guid.NewGuid();
        }
        public VectorObject(string name, Shape shape, SKPoint position, float rotation)
        {
            this.name = name;
            this.shape = shape;
            transform = new Transform()
            {
                Position = position,
                Rotation = rotation
            };

            if (rotation != 0f)
                shape.Rotate(transform.Rotation);

            guid = Guid.NewGuid();
		}

        public void Initialize(Window window)
        {
            this.window = window;
        }

        //OBSOLETE
        /*      public void SetPosition(float x, float y) => transform.Position = new SKPoint(x, y);
                public void SetPosition(SKPoint position) => transform.Position = position;

                public void AddPosition(float x, float y) => transform.Position += new SKPoint(x, y);
                public void AddPosition(SKPoint position) => transform.Position += position;*/

        /// <summary>
        /// Wywołuje się przy utworzeniu obiektu, po zarejestrowaniu go w oknie.
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// Wywołuje się co każdą klatke
        /// </summary>
        public abstract void Update(float delta);

        public void RefreshGraphics(Canvas canvas)
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

    }
}
