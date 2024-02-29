namespace VGE.Audio
{
    public abstract class AudioEngine
    {

        /// <summary>
        /// Zarejestrowanie dźwięku do systemu
        /// </summary>
        public abstract void RegisterSound(ISound sound);
        /// <summary>
        /// Inicjalizacja sytemu dźwięku
        /// </summary>
        public abstract void Initialize();
    }
}
