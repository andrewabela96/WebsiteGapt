using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GaptWebsite.Models
{
    public class FbData
    {
        public string name { get; set; }
        public string description { get; set; }
        public DateTime start_time { get; set; }
        public Place place { get; set; }

        public FbData(string nameIN, string descriptionIn, DateTime start_timeIn, Place p)
        {
            name = nameIN;
            description = descriptionIn;
            start_time = start_timeIn;
            place = p;
        }

    }
}