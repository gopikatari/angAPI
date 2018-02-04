using System;
using System.Collections.Generic;
using DatingApp.API.Models;

namespace DatingApp.API.DTO
{
    public class UserforDetailDTO
    {
        public int Id { get; set; }
        public string UserName{ get; set; }
      
        public string Gender { get; set; }
        public int  Age { get; set; }
        public string KnownAs{set;get;}
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor {set;get;}
        public string Interests{set;get;}
        public string City{set;get;}
        public string Country{set;get;}
        public ICollection<PhotoforDetailsDTO> Photos{set;get;}
        public string PhotoUrl { get; set; }
    }
}