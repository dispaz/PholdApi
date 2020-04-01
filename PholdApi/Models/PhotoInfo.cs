using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models
{
    public class PhotoInfo
    {
        public PhotoInfo(string filename, string years)
        {
            Filename = filename;
            Years = years;
        }

        public string Filename { get; set; }
        public string Years { get; set; }
    }
}
