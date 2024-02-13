﻿using SkiaSharp;
using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Windows;


namespace Tempest.Objects
{
    public class Obstacle : PhysicsObject
    {
        public override int PhysicsLayer => mapPosition;
        int mapPosition;

        public override void OnCollisionEnter(PhysicsObject other)
        {
            if (other.Name == "Bullet")
                window.Destroy(this);
        }

        public override Setup Start()
        {
            MapManager.Instance.ResolutionChanged += (p) =>
            {
                transform.PerspectiveCenter = p;
            };

            return new Setup()
            {
                Name = "Obstacle",
                Shape = new PointShape(SKColors.Red,
                                new Point(-20, 0, 0),
                                new Point(0, -20, 0),
                                new Point(20, 0, 0)),
                Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z) + new Point(0, 0, 400),
                PerspectiveCenter = MapManager.Instance.PerspectivePoint,
                Rotation = Point.Zero3D
            };
        }

        public void Setup(int mapPosition, float zPosition)
        {
            this.mapPosition = mapPosition;
            transform.Position.Z = zPosition;
            transform.Position = MapManager.Instance.GetPosition(mapPosition, transform.Position.Z);
        }

        public override void Update(float delta)
        {
        }
    }

}