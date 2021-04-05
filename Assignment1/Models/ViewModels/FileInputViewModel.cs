using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models.ViewModels
{
    public class DeleteCommunityViewModel
    {
        public Community Community { get; set; }
        public Advertisements Advertisement { get; set; }
    }
}
