using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;

namespace Battlezone.Objects.Enemies
{
    public class EnemySpawner : VectorObject
    {
        public static EnemySpawner Instance;

        Random rng;
        public List<Enemy> Enemies;

        //na początku przeciwnicy będą się spawnić co 5sek
        float timerMax = 6f;
        float timerDiff = 0.001f;

        float currentTimer = 0f;

        public override Setup Start()
        {
            Instance = this;

            rng = new Random();
            Enemies = new List<Enemy>();

            return new Setup()
            {
                Name = "EnemySpawner"
            };
        }

        public override void Update(float delta)
        {
            currentTimer += delta;

            if (currentTimer < timerMax)
                return;

            SpawnEnemy();

            //ustawienia timera
            timerMax -= timerDiff;
            currentTimer = 0f;
        }

        public void DestroyAllObjects()
        {
            foreach (var enemy in Enemies)
                window.Destroy(enemy);
        }

        void SpawnEnemy()
        {
            var enemyPosition = GetEnemyPosition();

            var enemy = GetRandomEnemy(new Point(0,0,100));
            Enemies.Add(enemy);

            window.Instantiate(enemy);
        }

        Point GetEnemyPosition()
        {
            var rotation = new Point(0, rng.Next(0, 360), 0);
            var distance = rng.Next(Settings.EnemyMinDistance, Settings.EnemyMaxDistance);

            return PointManipulationTools.MovePointForward(new Transform()
            {
                Position = Scene3D.Camera.Position,
                Rotation = rotation
            }, distance);
        }

        Enemy GetRandomEnemy(Point enemyPosition)
        {
            /*
             * Algorytm respienia:
             * ZAWSZE: 1/10 szansy na zrespienie UFO
             * 
             * PONIŻEJ 10K PUNKTÓW
             * - CZOŁG: 50%
             * - SZYBKI CZOŁG: 25%
             * - RAKIETA: 25%
             * 
             * POWYŻEJ 10K PUNKTÓW: 
             * Zamiast czołgu respi się szybki czołg 
             */

            var ufoChance = rng.Next(0, 10);

            if(ufoChance == 0)
                return new UFO(enemyPosition);

            var enemyChance = rng.Next(0,4);

            if (enemyChance == 2) //25% na szybki czołg
                return new FastTank(enemyPosition);
            else if(enemyChance == 3)  
                return new Missle(enemyPosition);
            else
                return GameManager.Score <= 10000 ? new Tank(enemyPosition) : new FastTank(enemyPosition);
        }

        public override bool OverrideRender(Canvas canvas)
        {
            //nie rysuj nic
            return true;
        }
    }
}
