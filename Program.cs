using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace practicum4
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Practicum 4: deel 1\n");

            // De 3 tracks
            Track track1 = new Track { artist = "Dream Theater", title = "6:00", length = new TimeSpan(0, 0, 5, 31, 0) }; // 00:05:31
            Track track2 = new Track { artist = "Dream Theater", title = "Caught in a Web", length = new TimeSpan(0, 0, 5, 28, 0) }; // 00:05:28
            Track track3 = new Track { artist = "Dream Theater", title = "Innocence Faded", length = new TimeSpan(0, 0, 5, 34, 0) }; // 00:05:34

            // Lijst met de 3 tracks
            List<Track> tracks = new List<Track> { track1, track2, track3 };

            // CD met de 3 tracks
            CD cd1 = new CD { name = "Awake", artist = "Dream Theater", tracks = tracks };

            XDocument cdXml = cd1.createXml();
            Console.WriteLine(cdXml.ToString());

            Console.WriteLine("\n\nPracticum 4: deel 2\n");

            // API
            String xmlString;
            using (WebClient wc = new WebClient())
            {
                xmlString = wc.DownloadString(@"https://ws.audioscrobbler.com/2.0/?method=album.getInfo&album=awake&artist=Dream%20Theater&api_key=b5cbf8dcef4c6acfc5698f8709841949");
            }
            XDocument myXMLDoc = XDocument.Parse(xmlString);

            var track_check_query =

                from tr in myXMLDoc.Descendants("track")

                where

               
                !((from tr2 in cdXml.Descendants("track")
                   // Check of de naam en artiest van de track uit de API niet gelijk zijn aan de naam en artiest van de tracks die al in de oude XML zitten
                   where tr.Element("name").Value == tr2.Element("title").Value &&
                   tr.Element("artist").Element("name").Value == tr2.Element("artist").Value

                   // Select alleen de tracks die dus nog niet in de oude cdXml zitten
                   select tr2).Any())

                // Maak een track object met de track die dus geselect zijn in de vorige query (wat dus betekend dat ze nog niet in de cdXml zaten)
                select new Track
                {
                    title = tr.Element("name").Value,
                    artist = tr.Element("artist").Element("name").Value,
                    length = TimeSpan.FromSeconds(Int32.Parse(tr.Element("duration").Value))
                };

            foreach (Track t in track_check_query)
            {
                cd1.tracks.Add(t);
                Console.WriteLine(t.title);

            }

            Console.WriteLine("\n");

            Console.WriteLine(cd1.createXml().ToString());
        }
    }
}
