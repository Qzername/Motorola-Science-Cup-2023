using Tempest.Objects.UI;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Tempest.Objects
{
	public class SuperZapper : VectorObject
	{
		private bool _isZPressed;
		private bool _isUsed;
		public bool IsUsed
		{
			get => _isUsed;
			private set
			{
				_isUsed = value;
				UIManager.Instance.RefreshUI();
			}
		}

		public override Setup Start()
		{
			UIManager.Instance.RefreshUI();

			return new Setup()
			{
				Name = "SuperZapper"
			};
		}

		public override void Update(float delta)
		{
			if (GameManager.Instance.StopGame || IsUsed)
				return;

			if (window.KeyDown(Key.Z) && !_isZPressed)
				_isZPressed = true;
			else if (!window.KeyDown(Key.Z) && _isZPressed)
			{
				IsUsed = true;
				_isZPressed = false;
				EnemyManager.Instance.DestroyEnemies();
			}
		}

		public override bool OverrideRender(Canvas canvas)
		{
			return true;
		}
	}
}
