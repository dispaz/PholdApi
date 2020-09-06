using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models
{
    abstract public class BasePhotoInfo
    {
        public int Id { get; set; }
        public int PholdObjectId { get; set; }

        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
    }

    public class GetPhotoInfo : BasePhotoInfo
    {
        public string ImageUrl { get; set; }
    }

    public class PostPhotoInfo : BasePhotoInfo
    {
        public string Filename { get; set; }
    }

    public class PhotoInfo : BasePhotoInfo
    {
        public string Filename { get; set; }
        public PhotoInfo(int id, int pholdObjectId, string filename, int? fromYear, int? toYear)
        {
            Id = id;
            PholdObjectId = pholdObjectId;
            Filename = filename;
            FromYear = fromYear;
            ToYear = toYear;
        }

    }
}
