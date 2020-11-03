using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Models
{
    public class BasePholdObject
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Street { get; set; }
        [Required]
        [Range(-90.0, 90.0)]
        public double Latitude { get; set; }
        [Required]
        [Range(-180.0, 180.0)]
        public double Longitude { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string AreaCode { get; set; }        

    }
}
