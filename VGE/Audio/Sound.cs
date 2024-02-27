using NAudio.Wave;
using System.IO;

namespace VGE.Audio
{
    //https://markheath.net/post/mixing-and-looping-with-naudio
    public class Sound : WaveStream
    {
        public bool IsLooped;
        bool isPaused;

        WaveStream sourceStream;

        //wavestream
        public override WaveFormat WaveFormat { get => sourceStream.WaveFormat; }
        public override long Length { get => sourceStream.Length; }
        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public Sound(string soundName)
        {
            sourceStream = new WaveFileReader($"./Resources/{soundName}");

            isPaused = true;
        }

        public void PlayFromStart()
        {
            Position = 0;
            Play();
        }
        public void Play() => isPaused = false;
        public void Pause() => isPaused = true;

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            if (isPaused)
                return totalBytesRead;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !IsLooped)
                        break;

                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }

            return totalBytesRead;
        }
    }
}
