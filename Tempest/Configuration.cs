using SkiaSharp;

namespace Tempest
{
    public class Configuration(SKColor? tunnel = null, SKColor? player = null, SKColor? superZapper = null,
        SKColor? flipper = null, SKColor? tanker = null, SKColor? spike = null, SKColor? pulsar = null)
    {
        // https://www.arcade-history.com/?n=tempest-upright-model&page=detail&id=2865
        // Domyślne kolory to kolory dla pierwszych 16 poziomów
        public SKColor Tunnel = tunnel != null ? (SKColor)tunnel : SKColors.Blue;
        public SKColor Player = player != null ? (SKColor)player : SKColors.Yellow;
        public SKColor SuperZapper = superZapper != null ? (SKColor)superZapper : SKColors.Yellow;
        public SKColor Flipper = flipper != null ? (SKColor)flipper : SKColors.Red;
        public SKColor Tanker = tanker != null ? (SKColor)tanker : SKColors.Purple;
        public SKColor Spike = spike != null ? (SKColor)spike : SKColors.Green;
        public SKColor Pulsar = pulsar != null ? (SKColor)pulsar : SKColors.Empty; // Pojawiają się dopiero później w grze
    }
}
