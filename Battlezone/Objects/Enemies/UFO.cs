using SkiaSharp;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Shapes;
using VGE.Physics;
using VGE.Resources;

namespace Battlezone.Objects.Enemies
{
	public class UFO : Enemy
	{
		const float rotationSpeed = 80f;
		const float speed = 40f;
		const float maxDistance = 200f;

		public override int PhysicsLayer => 1;

		public override int Score => 5000;

		Point defaultRotatiion;

		float currentDistance;
		bool isForward;

		public UFO(Point startPosition) : base(startPosition)
		{
			defaultRotatiion = new Point(0, new Random().Next(0, 360), 0);

			isForward = true;
		}

		public override void OnCollisionEnter(PhysicsObject other)
		{
		}

		public override Setup Start()
		{
			//dla testu biorę kostkę
			var shape = ResourcesHandler.Get3DShape("ufo");

			return new Setup()
			{
				Name = "Enemy_UFO",
				Position = startPosition,
				Shape = shape,
				Rotation = defaultRotatiion
			};
		}

		public override void Update(float delta)
		{
			Rotate(new Point(0, delta * rotationSpeed, 0));

			//ufo będzie chodziło tam i spowrotem
			var step = delta * speed;
			currentDistance += step;

			if (currentDistance > maxDistance)
			{
				isForward = !isForward;
				currentDistance = 0;
			}

			transform.Position = PointManipulationTools.MovePointForward(new Transform()
			{
				Position = transform.Position,
				Rotation = defaultRotatiion
			}, step * (isForward ? 1 : -1f));
		}
	}
}
