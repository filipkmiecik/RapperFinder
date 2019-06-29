using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Newtonsoft.Json;
using SearchingCurses;

using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();          
        }
        private void Button_Click(object sender, EventArgs e)
        {
      
            var userText = richTextBox1.Text;
            var userSong = new UserInput(userText);

            var eminemSwearStats = new Rapper("Eminem");
            eminemSwearStats.AddSong("stan");
            eminemSwearStats.AddSong("Lose Yourself");
            eminemSwearStats.AddSong("Bad Husband");
            eminemSwearStats.AddSong("So Much Better");
            eminemSwearStats.AddSong("Stepping Stone");

            var twoPacStats = new Rapper("2pac");
            twoPacStats.AddSong("changes");
            twoPacStats.AddSong("I Get Around");
            twoPacStats.AddSong("Keep Ya Head Up");
            twoPacStats.AddSong("Life Goes On");
            twoPacStats.AddSong("Ghetto Gospel");

            var akonStats = new Rapper("Akon");
            akonStats.AddSong("I'm So Paid");
            akonStats.AddSong("Locked Up");
            akonStats.AddSong("We Don't Care");
            akonStats.AddSong("Troublemaker");
            akonStats.AddSong("Give It To Em");

            var snoopDoggStats = new Rapper("Snoop Dogg");
            snoopDoggStats.AddSong("I Wanna Rock");
            snoopDoggStats.AddSong("Platinum");
            snoopDoggStats.AddSong("Lay Low");
            snoopDoggStats.AddSong("El Lay");
            snoopDoggStats.AddSong("Sweat");

            var wizKhalifaStats = new Rapper("Wiz Khalifa");
            wizKhalifaStats.AddSong("Promises");
            wizKhalifaStats.AddSong("On My Level");
            wizKhalifaStats.AddSong("KK");
            wizKhalifaStats.AddSong("No sunshine");
            wizKhalifaStats.AddSong("We Dem Boyz");


            var EminemScore = UserSongCompare(eminemSwearStats, userSong);
            var twoPacScore = UserSongCompare(twoPacStats, userSong);
            var akonScore = UserSongCompare(akonStats, userSong);
            var snoopDoggScore = UserSongCompare(snoopDoggStats, userSong);
            var wizKhalifaScore = UserSongCompare(wizKhalifaStats, userSong);

            List<MyData> compareRappers = new List<MyData>();

            compareRappers.Add(new MyData { score = EminemScore, rapper = "Eminem" });
            compareRappers.Add(new MyData { score = twoPacScore, rapper = "2Pac" });
            compareRappers.Add(new MyData { score = akonScore, rapper = "Akon" });
            compareRappers.Add(new MyData { score = snoopDoggScore, rapper = "Snoop Dogg" });
            compareRappers.Add(new MyData { score = wizKhalifaScore, rapper = "Wiz Khalifa" });

            var bubbleSortedList = rankRappers(compareRappers);

            if(bubbleSortedList[4].score == 0)
            {
                label2.Text = "There are no swears in this song. In order to create a comparision please paste in lyrics that contain at least a few swears.";
            }
            else
            {
                label2.Text = "That's most likely " + bubbleSortedList[4].rapper + " (" + bubbleSortedList[4].score + " matches)";
                label3.Text = "2. " + bubbleSortedList[3].rapper + " (" + bubbleSortedList[3].score + " matches)";
                label4.Text = "3. " + bubbleSortedList[2].rapper + " (" + bubbleSortedList[2].score + " matches)";
                label5.Text = "4. " + bubbleSortedList[1].rapper + " (" + bubbleSortedList[1].score + " matches)";
                label6.Text = "5. " + bubbleSortedList[0].rapper + " (" + bubbleSortedList[0].score + " matches)";
            }           
        }
        private static List<MyData> rankRappers(List<MyData> list)
        {
            int size = (list.Capacity) / 2;
            for (int i = 0; i < size; i++)
            {
                for (int a = 0; a < (size - i); a++)
                {
                    if (list[a].score > list[a + 1].score)
                    {
                        int b = list[a].score;
                        string c = list[a].rapper;
                        list[a].score = list[a + 1].score;
                        list[a].rapper = list[a + 1].rapper;
                        list[a + 1].score = b;
                        list[a + 1].rapper = c;
                    }

                }
            }
           return list;
        }

        private static int UserSongCompare(Rapper eminemSwearStats, UserInput song)
        {

            int score = 0;
            foreach (var myWords in song.userSwears)
            {

                if (eminemSwearStats.swears.ContainsKey(myWords.Key))
                {
                    score++;
                }
            }
            return score;
        }

        public class SwearStats : Censor
        {
            public Dictionary<string, int> swears = new Dictionary<string, int>();
            public void AddSwearFrom(Song song)
            {
                foreach (var word in badWords)
                {
                    var occurances = song.CountOccurances(word);

                    if (occurances > 0)
                    {
                        if (!swears.ContainsKey(word))
                            swears.Add(word, 0);
                        swears[word] += occurances;
                    }
                }
            }

            public void ShowSummary()
            {
                foreach (var item in swears)
                {
                    Console.WriteLine(item.Key + " " + item.Value);
                }
            }

            public int CompareWith(SwearStats eminemSwearStats)
            {
                int score = 0;
                foreach (var myWords in swears)
                {
                    if (eminemSwearStats.swears.ContainsKey(myWords.Key))
                    {
                        score++;
                    }
                }
                return score;
            }
        }

        public class Song
        {
            public string title;
            public string artist;
            public string lyrics;
            public Song(string band, string songName)
            {
                var browser = new WebClient();
                var url = "https://api.lyrics.ovh/v1/" + band + "/" + songName;
                var json = WebCache.GetOrDownload(url);
                var lyricsData = JsonConvert.DeserializeObject<LyricovhAnwser>(json);

                title = songName;
                artist = band;
                lyrics = lyricsData.lyrics;
            }

            public int CountOccurances(string word)
            {
                var pattern = "\\b" + word + "\\b";
                return Regex.Matches(lyrics, pattern, RegexOptions.IgnoreCase).Count;
            }
        }

        public class Rapper : SwearStats
        {
            public string name;

            public Rapper(string name)
            {
                this.name = name;
            }

            public void AddSong(string title)
            {
                var song = new Song(band: name, songName: title);
                AddSwearFrom(song);
            }
        }
        public class LyricovhAnwser
        {
            public string lyrics;
            public string error;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
        }
    }

    internal class MyData
    {
        public int score;
        public string rapper;
    }
}