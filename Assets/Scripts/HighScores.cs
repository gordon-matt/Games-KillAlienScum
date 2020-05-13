using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour
{
    public Text ScoresLeft;

    public Text ScoresRight;

    private string fileName;

    private void Start()
    {
        fileName = Path.Combine(Application.dataPath, "highScores.txt");

        EnsureFileExists();
        var highScores = ReadFile();

        // If the current player score is higher than ANY of the existing scores..
        if (highScores.Any(x => x.Score <= GameManager.Instance.Points))
        {
            //.. then it's safe to just remove the lowest score and place the new one in the list..
            var minScore = highScores.OrderBy(x => x.Score).First();
            highScores.Remove(minScore);
            highScores.Add(new HighScore { Name = GameManager.Instance.PlayerName, Score = GameManager.Instance.Points });

            SaveFile(highScores);
        }

        var top5 = highScores.OrderByDescending(x => x.Score).Take(5);
        var bottom5 = highScores.OrderByDescending(x => x.Score).Skip(5).Take(5);

        string leftText = PrepareForDisplay(top5);
        string rightText = PrepareForDisplay(bottom5);

        ScoresLeft.text = leftText;
        ScoresRight.text = rightText;
    }

    private void EnsureFileExists()
    {
        if (!File.Exists(fileName))
        {
            var highScores = new List<HighScore>
            {
                new HighScore { Name = "AAA", Score = 1200 },
                new HighScore { Name = "AAA", Score = 1000 },
                new HighScore { Name = "AAA", Score = 900 },
                new HighScore { Name = "AAA", Score = 700 },
                new HighScore { Name = "AAA", Score = 600 },
                new HighScore { Name = "AAA", Score = 500 },
                new HighScore { Name = "AAA", Score = 400 },
                new HighScore { Name = "AAA", Score = 350 },
                new HighScore { Name = "AAA", Score = 200 },
                new HighScore { Name = "AAA", Score = 90 }
            };

            SaveFile(highScores);
        }
    }

    private string PrepareForDisplay(IEnumerable<HighScore> fiveHighScores)
    {
        var sb = new StringBuilder(32);

        for (int i = 0; i < 5; i++)
        {
            var highScore = fiveHighScores.ElementAt(i);

            // The font is not good for keeping things inline.. number 1 needs to be shifted a bit.
            //  TODO: Better to get a monospaced font
            sb.AppendLine($"{(i == 0 ? " 1" : (i + 1).ToString())}. {highScore.Name}: {highScore.Score:N0}");
        }

        return sb.ToString();
    }

    private ICollection<HighScore> ReadFile()
    {
        var lines = File.ReadAllText(fileName).ToLines();
        var results = new List<HighScore>();

        for (int i = 0; i < lines.Count(); i++)
        {
            string line = lines.ElementAt(i);

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] columns = line.Split(',');

            results.Add(new HighScore
            {
                Name = columns[0].Replace("\"", string.Empty),
                Score = Convert.ToInt32(columns[1].Replace("\"", string.Empty))
            });
        }

        return results;
    }

    private void SaveFile(ICollection<HighScore> highScores)
    {
        string csv = highScores.ToCsv(outputColumnNames: false);
        using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        using (var streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.Write(csv);
        }
    }

    public class HighScore
    {
        public string Name { get; set; }

        public int Score { get; set; }
    }
}