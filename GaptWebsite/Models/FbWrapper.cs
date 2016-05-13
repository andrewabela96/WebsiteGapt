using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GaptWebsite.Models
{
    public class FbWrapper
    {
        public List<FbData> data { get; set; }
        public FbWrapper(List<FbData> dataIn)
        {
            data = dataIn;
        }
        public FbWrapper()
        {
            data = new List<FbData>();
        }
    }
}