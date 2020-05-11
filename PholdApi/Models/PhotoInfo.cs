using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models
{
    abstract public class BasePhotoInfo
    {
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
    }

    public class GetPhotoInfo : BasePhotoInfo
    {
        public string ImageUrl { get; set; }
    }

    public class PostPhotoInfo : BasePhotoInfo
    {
        public int Id { get; set; }
        public string Filename { get; set; }
    }

    public class PhotoInfo : BasePhotoInfo
    {
        public PhotoInfo(string filename, int? fromYear, int? toYear)
        {
            Filename = filename;
            FromYear = fromYear;
            ToYear = toYear;
        }
        public string Filename { get; set; }

    }
}
