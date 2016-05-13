using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GaptWebsite.Models
{
    public class Place
    {
        public string name { get; set; } 
        public Where location { get; set; }
        public Place(string nameIn)
        {
            name = nameIn;

        }
    }
}