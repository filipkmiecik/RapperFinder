using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    internal class UserInput:Censor
    {
        private string userLyrics;

        public Dictionary<string, int> userSwears = new Dictionary<string, int>();
        public UserInput(string lyrics)
        {
            this.userLyrics = lyrics;


            foreach (var word in badWords)
            {
                var times = CountOccurances(word, lyrics);

                if (times > 0)
                {
                    if (!userSwears.ContainsKey(word))
                        userSwears.Add(word, 0);
                    userSwears[word] += times;
                }
            }
        }

        public int CountOccurances(string word, string lyrics)
        {
            var pattern = "\\b" + word + "\\b";
            return Regex.Matches(lyrics, pattern, RegexOptions.IgnoreCase).Count;

        }

       
    }
}