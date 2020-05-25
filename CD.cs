using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace practicum4
{
    class CD
    {
        public string name { get; set; }
        public string artist { get; set; }
        public List<Track> tracks { get; set; }

        public XDocument createXml()
        {
            /*

            // De CD element (root)
            var cdXml = new XDocument();
            var rootElem = new XElement("CD", new XElement("title", this.title), new XElement("artist", this.artist)); // XAttribute ziet er mooier uit...

            // Tracks element
            var tracksElem = new XElement("tracks");

            // De Linq query
            tracksElem.Add(this.tracks.Select(track =>
                new XElement("track",
                    new XElement("title", track.title),
                    new XElement("artist", track.artist),
                    new XElement("length", track.length.ToString())
                    )));

            rootElem.Add(tracksElem);
            cdXml.Add(rootElem);

            */

            var cdXml = new XDocument();
            var rootElem = new XElement("CD",
                new XAttribute("artist", this.artist),
                 new XAttribute("name", this.name),
                 new XElement("tracks",
                this.tracks.Select(track =>
                new XElement("track",
                    new XElement("title", track.title),
                    new XElement("artist", track.artist),
                    new XElement("length", track.length.ToString())
                    )))
                );

            cdXml.Add(rootElem);

            return cdXml;
        }
    }
}