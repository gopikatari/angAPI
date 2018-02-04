using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
       public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded{set;get;}
        public bool IsMain{set;get;}  

        public User User { get; set; }
        public int UserId { get; set; }
    }
}