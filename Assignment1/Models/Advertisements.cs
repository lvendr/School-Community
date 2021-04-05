using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Models
{
    public class Advertisements
    {
        public int ID { get; set; }

        public string CommunityId { get; set; }
        public string CommunityTitle { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
