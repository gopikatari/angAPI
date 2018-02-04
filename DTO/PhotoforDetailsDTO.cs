using System;

namespace DatingApp.API.DTO
{
    public class PhotoforDetailsDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded{set;get;}
        public bool IsMain{set;get;}  
    }
}