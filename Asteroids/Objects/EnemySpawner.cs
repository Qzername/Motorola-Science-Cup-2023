using Microsoft.VisualBasic.FileIO;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using VGE;
using VGE.Graphics;
using VGE.Windows;

namespace Asteroids.Objects
{
    public class EnemySpawner : VectorObject
    {
        public static EnemySpawner Instance;

        bool spawningAllowed = true;

        Random rand = new();

        // obstacleRotationOffset nie moze byc wiekszy niz 45, poniewaz wtedy przeszkoda moze byc poza ekranem
        const int obstacleRotationOffset = 30;

        float ufoMax = 15;
        float sinusMax = 10;
        float asteroidsPerLevel = 2;
        float speedboost = 0f;

        float ufoTimer = 0f;
        float sinusTimer = 0f;

        List<Obstacle> obstacles;
        List<UFO> ufos;
        List<Sinus> sinuses;

        bool isWaveGenerating;

        public override Setup Start()
        {
            obstacles = new();
            ufos = new();
            sinuses = new();

            Instance = this;

            return new()
            {
                Name = "EnemySpawner"
            };
        }

        public override void Update(float delta)
        {
            if (!spawningAllowed)
                return;

            ufoTimer += delta;
            sinusTimer += delta;

            if (obstacles.Count == 0 && ufos.Count == 0 && sinuses.Count == 0 && !isWaveGenerating &&
                GameManager.Instance.CurrentScreen != Screen.GameOver &&
                GameManager.Instance.CurrentScreen != Screen.Highscore)
                NextWave();

            if(ufoTimer > ufoMax)
            {
                ufoTimer = 0f;
                TimerSpawnUfo();
            }

            if(sinusTimer > sinusMax)
            {
                sinusTimer = 0f;
                TimerSpawnSinus();
            }
        }

        public void RefreshSpawner(bool spawningAllowed)
        {
            asteroidsPerLevel = 5f;
            speedboost = 0f;
            ufoTimer = 15f;

            this.spawningAllowed = spawningAllowed;
            ufoTimer = 0f;

            for(int i = 0; i < obstacles.Count;i++)
                if (obstacles[i] is not null)
                {
                    window.Destroy(obstacles[i]);
                    obstacles[i] = null;
                }

            for(int i = 0; i < ufos.Count;i++)
                if (ufos[i] is not null)
                {
                    window.Destroy(ufos[i]);
                    ufos[i] = null;
                }

            for(int i = 0; i < sinuses.Count;i++)
                if (sinuses[i] is not null)
                {
                    window.Destroy(sinuses[i]);
                    sinuses[i] = null;
                }

            obstacles.Clear();
            ufos.Clear();
            sinuses.Clear();
        }

        public void RemoveObstacle(Obstacle obs) => obstacles.Remove(obs);
        public void RemoveUFO(UFO ufo) => ufos.Remove(ufo);
        public void RemoveSinus(Sinus sinus) => sinuses.Remove(sinus);

        public void RegisterObstacle(Obstacle obs) => obstacles.Add(obs);

        void NextWave()
        {
            isWaveGenerating = true;

            if (GameManager.Instance.CurrentScreen == Screen.MainMenu)
            {
                for (int i = 0; i < 10; i++)
                    SpawnObstacle();

                isWaveGenerating = false;
                return;
            }

            for (int i = 0; i < asteroidsPerLevel; i++)
                SpawnObstacle();

            asteroidsPerLevel += 2;
            speedboost += 10f;

            if (ufoTimer > 5f)
                ufoTimer -= 1f;

            isWaveGenerating = false;
        }

        void SpawnObstacle()
        {
            int[] pos = GetRandomPosition();

            var obstacle = new Obstacle(speedboost);
            window.Instantiate(obstacle);
            obstacle.Setup(new SKPoint(pos[0], pos[1]), pos[2]);
            // 0 -> x, 1 -> y, 2 -> rotation

            obstacles.Add(obstacle);
        }

        void TimerSpawnSinus()
        {
            if (obstacles.Count == 0)
                return;

            if (GameManager.Instance.CurrentScreen == Screen.GameOver || GameManager.Instance.CurrentScreen == Screen.Highscore)
                return;

            window.Instantiate(new Sinus());
        }

        void TimerSpawnUfo()
        {
            if (obstacles.Count == 0)
                return;

            if (GameManager.Instance.CurrentScreen == Screen.GameOver || GameManager.Instance.CurrentScreen == Screen.Highscore)
                return;

            int[] pos = GetRandomPosition();
            var ufo = new UFO((GameManager.Instance.Score > 10000 ? UFOType.Small : UFOType.Random));
            window.Instantiate(ufo);
            ufo.Setup(new SKPoint(pos[0], pos[1]), pos[2]);
            // 0 -> x, 1 -> y, 2 -> rotation

            ufos.Add(ufo);
        }

        int[] GetRandomPosition()
        {
            // Strony
            // 0 -> Lewo
            // 1 -> Prawo
            // 2 -> Gora
            // 3 -> Dol

            // Rotacja
            // 0 -> w Prawo
            // 180 -> w Lewo
            // 90 -> w Dol			
            // 270 -> w Gore

            // maxValue w rand.Next musi byc +1, aby oryginalny maxValue tez byl brany pod uwage
            int x, y, rotation, side = rand.Next(0, 3 + 1);

            Resolution res = window.GetResolution();

            switch (side)
            {
                case 0:
                    x = 0;
                    y = rand.Next(0, res.Height);
                    rotation = rand.Next(0 - obstacleRotationOffset, 0 + obstacleRotationOffset + 1);
                    break;
                case 1:
                    x = res.Width;
                    y = rand.Next(0, res.Height);
                    rotation = rand.Next(180 - obstacleRotationOffset, 180 + obstacleRotationOffset + 1);
                    break;
                case 2:
                    x = rand.Next(0, res.Width);
                    y = 0;
                    rotation = rand.Next(90 - obstacleRotationOffset, 90 + obstacleRotationOffset + 1);
                    break;
                case 3:
                    x = rand.Next(0, res.Width);
                    y = res.Height;
                    rotation = rand.Next(270 - obstacleRotationOffset, 270 + obstacleRotationOffset + 1);
                    break;
                default:
                    x = y = rotation = 0;
                    break;
            }

            return [x, y, rotation];
        }

        public override bool OverrideRender(Canvas canvas)
        {
            return true;
        }
    }
}
