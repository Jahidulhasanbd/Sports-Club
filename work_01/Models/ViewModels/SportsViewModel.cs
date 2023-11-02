using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace work_01.Models.ViewModels
{
    public class SportsViewModel
    {
        [Key]
        public int SportsId { get; set; }
        [Required, StringLength(50), Display(Name = "Sports Name")]
        public string SportsName { get; set; }
        public int PlayerCount { get; set; }
    }
}