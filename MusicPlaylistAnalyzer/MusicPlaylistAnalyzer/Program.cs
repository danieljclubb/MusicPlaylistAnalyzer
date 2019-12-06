using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlaylistAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<MusicData> musicDataList = new List<MusicData>();
            string report = "";

            if (args.Length != 2)
            {
                Console.WriteLine("For Windows please use: MusicPlaylistAnalyzer.exe <music_playlist_file_path> <report_file_path>");
                Console.WriteLine("For macOS please use: dotnet MusicPlaylistAnalyzer.dll <music_playlist_file_path> <report_file_path>");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            string musicFilePath = args[0];
            string reportFilePath = args[1];
            if (File.Exists(musicFilePath) == false)
            {
                Console.WriteLine("Incorrect playlist file path. Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            try
            {
                var sReader = new StreamReader(musicFilePath);
                var discard = sReader.ReadLine();
                var discardValues = discard.Split('\t');

                while (!sReader.EndOfStream)
                {
                    var line = sReader.ReadLine();
                    var values = line.Split('\t');
                    string name = values[0];
                    string artist = values[1];
                    string album = values[2];
                    string genre = values[3];
                    int size = Int32.Parse(values[4]);
                    int time = Int32.Parse(values[5]);
                    int year = Int32.Parse(values[6]);
                    int plays = Int32.Parse(values[7]);
                    MusicData md = new MusicData(name, artist, album, genre, size, time, year, plays);
                    musicDataList.Add(md);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Occured");
                Console.WriteLine(e.Message);
            }


            //QUESTION 1
            var question1 = from MusicData in musicDataList where MusicData.Plays >= 200 select MusicData;
            if(question1.Count() > 0)
            {
                report += "Songs that received 200 or more plays:\n";

                foreach(MusicData song in question1)
                {
                    report += song.ToString() + "\n";
                }

                report += "\n";
            }
            else
            {
                report += "No Data Available";
            }

            //QUESTION 2
            var question2 = from MusicData in musicDataList where MusicData.Genre == "Alternative" select MusicData;
            if(question2.Count() > 0)
            {
                string altsongs = "Number of Alternative Songs: " + question2.Count() + "\n\n";
                report += altsongs;
            }
            else
            {
                report += "No Data Available \n\n";
            }

            //QUESTION 3
            var question3 = from MusicData in musicDataList where MusicData.Genre == "Hip-Hop/Rap" select MusicData;
            if (question3.Count() > 0)
            {
                string hiphop = "Number of Hip-Hop/Rap Songs: " + question3.Count() + "\n\n";
                report += hiphop;
            }
            else
            {
                report += "No Data Available \n\n";
            }

            //QUESTION 4
            var question4 = from MusicData in musicDataList where MusicData.Album == "Welcome to the Fishbowl" select MusicData;
            report += "Songs from the Album Welcome to the Fishbowl:\n";
            if (question4.Count() > 0)
            {              
                foreach(MusicData song in question4)
                {
                    report += song + "\n";
                }

                report += "\n";
            }
            else
            {
                Console.WriteLine("No Data Available \n\n");
            }

            //QUESTION 5
            var question5 = from MusicData in musicDataList where MusicData.Year < 1970 select MusicData;
            report += "Songs from before 1970:\n";
            if (question5.Count() > 0)
            {               
                foreach(MusicData song in question5)
                {
                    report += song + "\n";
                }

                report += "\n";
            }
            else
            {
                Console.WriteLine("No Data Available \n\n");
            }

            //QUESTION 6
            var question6 = from MusicData in musicDataList where MusicData.Name.Length > 85 select MusicData;
            report += "Song names longer than 85 characters:\n";
            if(question6.Count() > 0)
            {
                foreach(MusicData song in question6)
                {
                    report += song + "\n";
                }

                report += "\n";
            }
            else
            {
                Console.WriteLine("No Data Available \n\n");
            }

            //QUESTION 7
            var question7 = from MusicData in musicDataList where MusicData.Time == (from data in musicDataList select data.Time).Max() select MusicData;
            report += "Longest song: " + (question7.First()).ToString();

            //WRITE TO REPORT
            try
            {
                System.IO.File.WriteAllText(reportFilePath, report);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(3);
            }
        }
    }
}
