﻿using SkiaSharp;
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
                PerspectiveCenter = setup.PerspectiveCenter,
            };

            if (transform.Position.Is3D && transform.PerspectiveCenter is null)
                throw new Exception("Object is 3D, but it does not have perspective point set");

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
            if (transform.Is3D)
            {
                var perspectivePoint = transform.PerspectiveCenter!.Value;

                foreach (var l in Shape.CompiledShape)
                {
                    //Długość Z jest inna dla każdego punktu, tutaj obliczenia na nowy:
                    var startPosition = transform.Position + l.StartPosition;
                    var endPosition = transform.Position + l.EndPosition;

                    float deltaSP = perspectivePoint.Z / startPosition.Z;
                    float deltaEP = perspectivePoint.Z / endPosition.Z;

                    Line finalLine = new Line()
                    {
                        StartPosition = new Point(startPosition.X * deltaSP, startPosition.Y * deltaSP) + perspectivePoint,
                        EndPosition = new Point(endPosition.X * deltaEP, endPosition.Y * deltaEP) + perspectivePoint,
                        LineColor = shape.CustomColor
                    };

                    canvas.DrawLine(finalLine);
                }
            }
            else
                foreach (var l in shape.CompiledShape)
                    canvas.DrawLine(new Line(l.StartPosition + transform.Position, l.EndPosition + transform.Position, shape.CustomColor));
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
            public IShape Shape;
            public Point Position; 
            public float Rotation;
            public Point? PerspectiveCenter;

            public Setup(string name, IShape shape, Point position, float rotation, Point? perspectiveCenter = null)
            {
                Name = name;
                Shape = shape;
                Position = position;
                Rotation = rotation;
                PerspectiveCenter = perspectiveCenter;
            }
        }
    }
}
