using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using Microsoft.Framework.ConfigurationModel;
using TreintijdenDashboard.Models;

namespace TreintijdenDashboard.Services
{
    public class VertrektijdenService
    {
        private IConfiguration _config;

        public VertrektijdenService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Vertrektijd>> GetVertrektijden(string station)
        {
            string apiUser = _config["apiName"];
            string apiKey = _config["apiPassword"];

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiUser))
                return SampleData();

            Uri address = new Uri(string.Format("http://webservices.ns.nl" + "/ns-api-avt?station={0}&a={1}", station, DateTime.Now.Ticks), UriKind.Absolute);
            WebClient webclient = new WebClient();
            //webclient.MaxResponseContentBufferSize = 9000000;

            webclient.Credentials = new NetworkCredential(apiUser, apiKey);

            string response = await webclient.DownloadStringTaskAsync(address);


            XElement tijdenXmlElement = XElement.Parse(response);

            List<Vertrektijd> vertrektijdList = new List<Vertrektijd>();

            foreach (var element in tijdenXmlElement.Descendants("VertrekkendeTrein"))
            {
                Vertrektijd tijd = new Vertrektijd();

                if (element.Element("RitNummer") != null)
                    tijd.Ritnummer = int.Parse(element.Element("RitNummer").Value);

                if (element.Element("VertrekTijd") != null)
                {
                    string time = element.Element("VertrekTijd").Value;
                    int zoneIndex = time.LastIndexOf('+');
                    if (zoneIndex > 0)
                    {
                        time = time.Substring(0, zoneIndex);
                    }

                    tijd.Tijd = DateTime.Parse(time);
                }

                tijd.VertragingTekst = XmlHelper.GetElementText(element.Element("VertrekVertragingTekst"));
                tijd.Eindbestemming = XmlHelper.GetElementText(element.Element("EindBestemming"));
                tijd.TreinSoort = XmlHelper.GetElementText(element.Element("TreinSoort"));
                tijd.Route = XmlHelper.GetElementText(element.Element("RouteTekst"));
                tijd.ReisTip = XmlHelper.GetElementText(element.Element("ReisTip"));
                tijd.Vervoerder = XmlHelper.GetElementText(element.Element("Vervoerder"));
                if (string.IsNullOrEmpty(tijd.Vervoerder))
                    tijd.Vervoerder = "NS";
                tijd.VervoerderDisplay = tijd.Vervoerder;

                tijd.Vertrekspoor = XmlHelper.GetElementText(element.Element("VertrekSpoor"));

                if (element.Element("VertrekSpoor") != null
                    && element.Element("VertrekSpoor").Attribute("wijziging") != null)
                {
                    tijd.IsVertrekspoorWijziging = bool.Parse(element.Element("VertrekSpoor").Attribute("wijziging").Value);
                }


                string sep = string.Empty;
                foreach (var opm in element.Descendants("Opmerking"))
                {
                    tijd.Opmerkingen += string.Format("{0}{1}", sep, opm.Value)
                        .Replace("\r", string.Empty)
                        .Replace("\t", string.Empty)
                        .Replace("\n", string.Empty)
                        .Trim();
                    sep = ", ";
                }



                vertrektijdList.Add(tijd);
            }

            return vertrektijdList;

        }

        private List<Vertrektijd> SampleData()
        {
            return new List<Vertrektijd> {
                new Vertrektijd
                {
                    Tijd = DateTime.Now,
                    Vertraging = "+5 min",
                    VertragingTekst = "+5 min",
                    Eindbestemming = "Amsterdam",
                    Opmerkingen = "",
                    Route = "Leiden, Haarlem",
                    Vervoerder = "NS",
                    Vertrekspoor = "2a",
                    TreinSoort = "Intercity"
                },
                new Vertrektijd
                {
                    Tijd = DateTime.Now,
                    Vertraging = "",
                    Eindbestemming = "Den Haag CS",
                    Opmerkingen = "",
                    Route = "Leiden, Haarlem",
                    Vervoerder = "NS",
                    Vertrekspoor = "2a",
                    TreinSoort = "Intercity"
                },
                new Vertrektijd
                {
                    Tijd = DateTime.Now,
                    Vertraging = "",
                    Eindbestemming = "Rotterdam",
                    Opmerkingen = "Rijdt niet verder dan Delft",
                    Route = "",
                    Vervoerder = "NS",
                    Vertrekspoor = "2a",
                    TreinSoort = "Intercity"
                },
                new Vertrektijd
                {
                    Tijd = DateTime.Now,
                    Vertraging = "",
                    Eindbestemming = "Amsterdam",
                    Opmerkingen = "Rijdt vandaag niet",
                    Route = "Leiden, Haarlem",
                    Vervoerder = "NS",
                    Vertrekspoor = "2a",
                    TreinSoort = "Sprinter"
                }

            };
        }
    }
}
