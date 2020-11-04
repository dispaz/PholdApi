using PholdApi.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models.Requests
{
    public class GetPholdObject : BasePholdObject
    {
        public int Id { get; set; }
        public List<GetPhotoInfo> PhotoData { get; set; }

        public GetPholdObject(PholdObjectDb phold)
        {
            Id = phold.Id;
            Name = phold.Name;
            Street = phold.Street;
            Latitude = phold.Latitude;
            Longitude = phold.Longitude;
            Description = phold.Description;
            AreaCode = phold.AreaCode;
        }
        public GetPholdObject(PholdObjectDb phold, List<GetPhotoInfo> photoInfos) : this(phold)
        {
            this.PhotoData = photoInfos;
        }
    }

    public class PostPholdObject : BasePholdObject
    {
    }
}
