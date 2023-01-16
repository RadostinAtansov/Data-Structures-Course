using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace WordCruncher
{
    class Program
    {
        static void Main()
        {
            //var syllables = "text, me, so, m, e, ran".Split(", ");
            //var targetWord = "somerandomtext";

            var syllables = Console.ReadLine().Split(", ");
            var targetWord = Console.ReadLine();

            var cruncher = new Cruncher(syllables, targetWord);

            foreach (var path in cruncher.GetSyllablePaths())
            {
                Console.WriteLine(path);
            }
        }
    }

    class Cruncher
    {
        private class Node
        {
            public Node(string syllable, List<Node> nextSyllables)
            {
                this.Syllable = syllable;
                this.NextSyllables = nextSyllables;
            }

            public string Syllable { get; set; }
            public List<Node> NextSyllables { get; set; }
        }

        private List<Node> syllablesGroups;

        public Cruncher(string[] syllables, string targetWord)
        {
            this.syllablesGroups = this.GenerateSyllableGroups(syllables, targetWord);
        }

        private List<Node> GenerateSyllableGroups(string[] syllables, string targetWord)
        {
            if (string.IsNullOrEmpty(targetWord) || syllables.Length == 0)
            {
                return null;
            }

            var resultValues = new List<Node>();

            for (int i = 0; i < syllables.Length; i++)
            {
                var syllable = syllables[i];

                if (targetWord.StartsWith(syllable))
                {
                    var nextSyllables = this.GenerateSyllableGroups(
                        syllables
                        .Where((_, index) => index != i).ToArray(),
                        targetWord.Substring(syllable.Length));

                    resultValues.Add(new Node(syllable, nextSyllables));
                }
            }
            return resultValues;
        }

        public IEnumerable<string> GetSyllablePaths()
        {
            var allPaths = new List<List<string>>();

            this.GeneratePaths(this.syllablesGroups, new List<string>(), allPaths);

            return new HashSet<string>(allPaths.Select(x => string.Join(" ", x)));
        }

        private void GeneratePaths(List<Node> syllablesGroups, List<string> currentPath, List<List<string>> allPaths)
        {
            if (syllablesGroups == null)
            {
                allPaths.Add(new List<string>(currentPath));
                return;
            }

            foreach (var node in syllablesGroups)
            {
                currentPath.Add(node.Syllable);

                this.GeneratePaths(node.NextSyllables, currentPath, allPaths);

                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }
    }
}
