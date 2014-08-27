using System;
using System.Xml.Linq;

namespace TreintijdenDashboard
{
    public static class XmlHelper
    {
        public static DateTime? GetDateTime(XElement element, string name)
        {
            DateTime? dtime = null;
            if (element.Element(name) != null)
            {
                string time = element.Element(name).Value;
                int zoneIndex = time.LastIndexOf('+');
                if (zoneIndex > 0)
                {
                    time = time.Substring(0, zoneIndex);
                }

                if (!string.IsNullOrEmpty(time))
                    dtime = DateTime.Parse(time);
            }

            return dtime;
        }


        public static string GetElementText(XElement element)
        {
            if (element != null)
                return element.Value;


            return null;
        }
    }
}