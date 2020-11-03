using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models
{
    public class BasePhotoInfo
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int PholdObjectId { get; set; }
        [Range(1500, 2021)]
        public int? FromYear { get; set; }
        [Range(1500, 2021)]
        public int? ToYear { get; set; }
    }
}
