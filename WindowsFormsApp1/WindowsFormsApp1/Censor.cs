﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Censor
    {
        protected string[] badWords;
        public Censor()
        {
            var profanities = File.ReadAllText("profanities.txt");
            profanities = profanities.Replace("*", "");
            profanities = profanities.Replace("(", "");
            profanities = profanities.Replace(")", "");
            profanities = profanities.Replace("\"", "");

            badWords = profanities.Split(',');


        }

        public string Fix(string tekst)
        {
            foreach (var word in badWords)
            {
                tekst = RepalaceBadWords(tekst, word);
            }

            return tekst;
        }

        private static string RepalaceBadWords(string tekst, string word)
        {
            var pattern = "\\b" + word + "\\b";
            return Regex.Replace(tekst, pattern, "_____", RegexOptions.IgnoreCase);
        }
    }
}
