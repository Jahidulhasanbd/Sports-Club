using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace work_01.Models
{
    public enum PlayerGrade
    {
        G01=1,
        G02,
        G03,
        G04
    }
    public class Sports
    {
        public Sports() 
        {
            this.Players=new List<Player>();
        }
        public int SportsId { get; set; }
        [Required,StringLength(50),Display(Name = "Sports Name")]
        public string SportsName { get; set; }
        //nev
        public virtual ICollection<Player> Players { get; set; }
    }
    public class Player
    {
        public int PlayerId { get; set; }
        [Required, StringLength(50), Display(Name = "Player Name")]
        public string PlayerName { get; set; }
        [Required,Column(TypeName ="date"),DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode = true),Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; }
        [EnumDataType(typeof(PlayerGrade))]
        public PlayerGrade? Grade { get; set; }
        public string PicturePath { get; set; }
        //fk
        [ForeignKey("Sports"), Display(Name = "Sports Name")]
        public int SportsId { get; set; }
        //nev
        public virtual Sports Sports { get; set; }
    }
    public class ClubDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Database.SetInitializer(new DbInitializer());
        }
        public DbSet<Sports> Sports { get; set; }
        public DbSet<Player> Players { get; set; }
    }
    public class DbInitializer:DropCreateDatabaseIfModelChanges<ClubDbContext>
    {
        protected override void Seed(ClubDbContext context)
        {
            Sports s1 = new Sports { SportsName = "Cricket" };
            Sports s2 = new Sports { SportsName = "Football" };

            s1.Players.Add(new Player { PlayerName = "Sakib Al Hasan", Grade = PlayerGrade.G01, JoinDate = DateTime.Now.AddYears(-4), PicturePath = "~/Images/no-image.jpg" });
            s1.Players.Add(new Player { PlayerName = "Musfiq", Grade = PlayerGrade.G01, JoinDate = DateTime.Now.AddYears(-3), PicturePath = "~/Images/no-image.jpg" });
            context.Sports.AddRange(new Sports[] { s1, s2 });
            context.SaveChanges();
            base.Seed(context);
        }
    }
}