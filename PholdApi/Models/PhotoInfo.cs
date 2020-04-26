using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models
{
    abstract public class BasePhotoInfo
    {
        public string Year { get; set; }
    }

    public class GetPhotoInfo : BasePhotoInfo
    {
        public string ImageUrl { get; set; }
    }

    public class PhotoInfo : BasePhotoInfo
    {
        public PhotoInfo(string filename, string year)
        {
            Filename = filename;
            Year = year;
        }
        public string Filename { get; set; }

    }
}
