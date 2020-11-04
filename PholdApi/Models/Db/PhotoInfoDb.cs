using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models.Db
{
    public class PhotoInfoDb : BasePhotoInfo
    {
        public int Id { get; set; }
        public string Filename { get; set; }
    }   
}
