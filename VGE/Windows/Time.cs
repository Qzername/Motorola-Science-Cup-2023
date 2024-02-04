using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGE.Windows
{
    public class Time
    {
        double deltaTime;

        TimeSpan lastFrame;

        /// <summary>
        /// Czas który minął od ostaniej klatki w sekundach
        /// </summary>
        public float DeltaTime
        {
            get => Convert.ToSingle(deltaTime);
        }

        /// <summary>
        /// Potwierdzenie pierwszej klatki (nie używać)
        /// </summary>
        public void StartCounting()
        {
            lastFrame = DateTime.Now.TimeOfDay;
        }

        /// <summary>
        /// Przeliczenie obecnego deltaTime (nie używać)
        /// </summary>
        public void NextFrame()
        {
            var currTime = DateTime.Now.TimeOfDay;

            deltaTime = (currTime - lastFrame).TotalSeconds;

            lastFrame = currTime;
        }
    }
}
