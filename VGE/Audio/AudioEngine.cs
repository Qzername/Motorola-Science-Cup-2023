using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGE.Audio
{
    public class AudioEngine
    {
        WaveOutEvent waveOutEvent;
        MixingWaveProvider32 mixer;

        public AudioEngine()
        {
            mixer = new MixingWaveProvider32();
        }

        public void RegisterSound(Sound sound) 
        {
            mixer.AddInputStream(new WaveChannel32(sound));
        }

        public void InitializeMixer()
        {
            waveOutEvent = new WaveOutEvent();
            waveOutEvent.Init(mixer);
            waveOutEvent.Play();
        }
    }
}
