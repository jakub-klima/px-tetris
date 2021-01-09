using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PxTetris.Core
{
    public class TopScores
    {
        private static readonly string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PxTetris");
        private static readonly string filePath = Path.Combine(directoryPath, "TopScores.txt");

        private readonly List<TopScore> items;
        public IReadOnlyList<TopScore> Items => items;

        public TopScores()
        {
            if (File.Exists(filePath))
            {
                items = File.ReadAllLines(filePath)
                    .Select(line => new TopScore(line))
                    .ToList();
            }
            else
            {
                items = new List<TopScore>
                {
                    new TopScore("JHN", 4000),
                    new TopScore("MIA", 3000),
                    new TopScore("BOB", 2000),
                    new TopScore("EVE", 1500),
                    new TopScore("JOE", 1000)
                };

                Save();
            }
        }

        private void Save()
        {
            Directory.CreateDirectory(directoryPath);
            File.WriteAllLines(filePath, items.Select(item => item.ToString()));
        }

        public bool IsTopScore(int score)
        {
            return Items.Last().Score < score;
        }

        public void InsertTopScore(string player, int score)
        {
            Debug.Assert(IsTopScore(score));

            TopScore firstSmallerScore = items.First(record => record.Score < score);
            items.Insert(
                items.IndexOf(firstSmallerScore),
                new TopScore(player, score));

            items.Remove(Items.Last());

            Save();
        }
    }
}
