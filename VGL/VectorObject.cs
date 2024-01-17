﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VGL.Graphics;

namespace VGL
{
    public class VectorObject
    {
        Shape shape;

        Transform transform;
        public Transform Transform
        {
            get => transform;
        }

        public VectorObject(Shape shape)
        {
            this.shape = shape;
            transform = new Transform();
        }
        public VectorObject(Shape shape, Transform transform)
        {
            this.shape = shape;
            this.transform = transform;

            if (transform.Rotation != 0f)
                shape.Rotate(transform.Rotation);
        }

        public VectorObject(Shape shape, SKPoint position, float rotation)
        {
            this.shape = shape;
            transform = new Transform()
            {
                Position = position,
                Rotation = rotation
            };

            if (rotation != 0f)
                shape.Rotate(transform.Rotation);
        }

        public void Draw(Canvas canvas)
        {
            foreach (var l in shape.CompiledShape)
                canvas.DrawLine(new Line(l.StartPosition + transform.Position, l.EndPosition + transform.Position));
        }

        public void SetPosition(float x, float y) => transform.Position = new SKPoint(x, y);
        public void SetPosition(SKPoint position) => transform.Position = position;

        public void AddPosition(float x, float y) => transform.Position += new SKPoint(x, y);
        public void AddPosition(SKPoint position) => transform.Position += position;

        public void Rotate(float angle)
        {
            shape.Rotate(angle);
            transform.Rotation += angle;
        }
    }
}
