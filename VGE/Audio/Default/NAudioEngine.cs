using NAudio.Wave;

namespace VGE.Audio.Default
{
    /// <summary>
    /// Implementacja silnika dźwięków oparta na bibliotece NAudio
    /// </summary>
    public class NAudioEngine : AudioEngine
    {
        WaveOutEvent waveOutEvent;
        MixingWaveProvider32 mixer;

        public NAudioEngine()
        {
            mixer = new MixingWaveProvider32();
        }

        public override void RegisterSound(ISound sound)
        {
            mixer.AddInputStream(new WaveChannel32((NASound)sound));
        }

        public override void Initialize()
        {
            waveOutEvent = new WaveOutEvent();
            waveOutEvent.Init(mixer);
            waveOutEvent.Play();
        }
    }
}
