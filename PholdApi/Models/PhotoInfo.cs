using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models
{
    public class BasePhotoInfo
    {
        public int PholdObjectId { get; set; }

        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
    }
}
