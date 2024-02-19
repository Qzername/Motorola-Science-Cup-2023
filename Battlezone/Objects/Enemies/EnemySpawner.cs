using System.Diagnostics;
using VGE;
using VGE.Graphics;
using VGE.Graphics.Scenes;

namespace Battlezone.Objects.Enemies
{
    public class EnemySpawner : VectorObject
    {
        Random rng;

        //na początku przeciwnicy będą się spawnić co 5sek
        float timerMax = 2f;
        float timerDiff = -100f;

        float currentTimer = 0f;

        public override Setup Start()
        {
            rng = new Random();

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

        void SpawnEnemy()
        {
            var rotation = new Point(0, rng.Next(0, 360), 0);
            var distance = rng.Next(Settings.EnemyMinDistance, Settings.EnemyMaxDistance);

            var enemyPosition = PointManipulationTools.MovePointForward(new Transform()
            {
                Position = Scene3D.Camera.Position,
                Rotation = rotation
            }, distance);

            //Debug.WriteLine(enemyPosition);

            window.Instantiate(new Missle(new Point(0,0,100)));
        }

        public override bool OverrideRender(Canvas canvas)
        {
            //nie rysuj nic
            return true;
        }
    }
}
