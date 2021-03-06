using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName{ get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs{set;get;}
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor {set;get;}
        public string Interests{set;get;}
        public string City{set;get;}
        public string Country{set;get;}

        public ICollection<Photo> Photos { get; set; }

//one user can have many photos in this case
       public User(){
           Photos=new Collection<Photo>();
        }
    }

   
}