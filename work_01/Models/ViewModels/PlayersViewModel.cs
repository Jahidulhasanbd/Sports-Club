using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace work_01.Models.ViewModels
{
    public class PlayersViewModel
    {
        public int PlayerId { get; set; }
        [Required, StringLength(50), Display(Name = "Player Name")]
        public string PlayerName { get; set; }
        [Required, Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; }
        [EnumDataType(typeof(PlayerGrade))]
        public PlayerGrade? Grade { get; set; }
        public string PicturePath { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        //fk
        [ForeignKey("Sports")]
        public int SportsId { get; set; }
        //nev
        public virtual Sports Sports { get; set; }
    }
}