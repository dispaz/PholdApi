using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models
{
    abstract public class BasePholdObject
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public string AreaCode { get; set; }        

    }

    public class PholdObject : BasePholdObject
    {
        public int ID { get; set; }
        public List<GetPhotoInfo> PhotoData { get; set; }

    }

    public class SavePholdObject : BasePholdObject
    {}
}
