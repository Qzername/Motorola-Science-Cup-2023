using VGE.Windows;

namespace VGE.Graphics.Scenes
{
    public interface IScene
    {
        public void UpdateResolution(Resolution resolution);
        public void DrawObject(Canvas canvas, VectorObject vectorObject);
    }
}
