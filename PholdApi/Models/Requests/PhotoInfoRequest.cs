using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models.Requests
{
    public class GetPhotoInfo : BasePhotoInfo
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }
    
    public class PostPhotoInfo : BasePhotoInfo
    {
    }
}
