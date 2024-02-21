using Newtonsoft.Json;

namespace HML
{
    public static class HighscoreManager
    {
        static List<Highscore> highscores;

        static string PathToFile => AppDomain.CurrentDomain.BaseDirectory + "highscores.json";

        static HighscoreManager()
        {
            ReadHighscores();
        }

        public static Highscore[] GetScores()
        {
            return highscores.ToArray();
        }

        public static void SetScore(Highscore score)
        {
            if (!IsNewHighscore(score))
                return;

            if(highscores.Count == 10)
                highscores.RemoveAt(9);
    
            highscores.Add(score);

            highscores = highscores.OrderByDescending(x=>x.Score).ToList();

            SaveHighscores();
        }

        public static bool IsNewHighscore(Highscore score) => IsNewHighscore(score.Score);
        public static bool IsNewHighscore(int score) => highscores.Count < 10 || highscores[^1].Score < score;

        static void SaveHighscores()
        {
            string json = JsonConvert.SerializeObject(highscores);
            File.WriteAllText(PathToFile, json);
        }

        static void ReadHighscores()
        {
            highscores = new List<Highscore>();

            if (!File.Exists(PathToFile))
            {
                var filestream = File.Create(PathToFile);
                filestream.Close();
            }

            string file = File.ReadAllText(PathToFile);

            if (string.IsNullOrEmpty(file))
                file = "[]";

            highscores.AddRange(JsonConvert.DeserializeObject<Highscore[]>(file)!.OrderByDescending(x=>x.Score).ToArray());  
        }
    }
}
