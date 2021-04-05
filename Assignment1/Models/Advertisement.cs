using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    public class Advertisement
    {
        public int ImageID { get; set; }

        [Required]
        [DisplayName("Ads Name")]
        public string FileName { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
