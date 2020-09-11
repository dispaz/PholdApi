using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models
{
    public class BasePholdObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public string AreaCode { get; set; }        

    }

    public class PholdObject : BasePholdObject
    {
        public PholdObject(BasePholdObject phold, List<GetPhotoInfo> photoInfos)
        {
            ID = phold.ID;
            Name = phold.Name;
            Street = phold.Street;
            Latitude = phold.Latitude;
            Longitude = phold.Longitude;
            Description = phold.Description;
            AreaCode = phold.AreaCode;

            PhotoData = photoInfos;
        }
        public List<GetPhotoInfo> PhotoData { get; set; }
    }

    public class SavePholdObject : BasePholdObject
    {}
}
