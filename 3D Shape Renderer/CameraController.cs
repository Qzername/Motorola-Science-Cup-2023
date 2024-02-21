using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;
using VGE.Windows;

namespace ShapeRenderer
{
	internal class CameraController : VectorObject
	{
		const float cameraSpeed = 40f, cameraRotationSpeed = 80f;
		float distanceFromObject = 100f;

		public override Setup Start()
		{
			UpdateCameraPosition();

			return new Setup()
			{
				Name = "CameraController"
			};
		}

		public override void Update(float delta)
		{
			bool isWpressed = window.KeyDown(Key.W);
			bool isApressed = window.KeyDown(Key.A);
			bool isSpressed = window.KeyDown(Key.S);
			bool isDpressed = window.KeyDown(Key.D);

			if (!(isWpressed || isApressed || isSpressed || isDpressed))
				return;//żaden przycisk nie jest wciśnięty

			if (isApressed)
				Scene3D.Camera.Rotation.Y += cameraRotationSpeed * delta;
			else if (isDpressed)
				Scene3D.Camera.Rotation.Y -= cameraRotationSpeed * delta;

			if (isWpressed)
				distanceFromObject -= cameraSpeed * delta;
			else if (isSpressed)
				distanceFromObject += cameraSpeed * delta;

			UpdateCameraPosition();
		}

		void UpdateCameraPosition()
		{
			Scene3D.Camera.Position = PointManipulationTools.MovePointForward(new Transform()
			{
				Position = Point.Zero,
				Rotation = new Point(0, Scene3D.Camera.Rotation.Y, 0),
			}, -distanceFromObject);
		}

		public override bool OverrideRender(Canvas canvas)
		{
			return true;
		}
	}
}
