using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battlezone
{
    /// <summary>
    /// Główne ustawienia do gry.
    /// 
    /// Nie wszystkie tutaj się znajdują
    /// </summary>
    public static class Settings
    {
        public const float RenderDistance = 500f;

        //Obstacle generator settings
        public const float ChunkSize = 250f;
        public const float ObstacleGeneratorDistance = 3500f;

        //enemy spawner settings
        public const int EnemyMinDistance = 200;
        public const int EnemyMaxDistance = 400;
    }
}
