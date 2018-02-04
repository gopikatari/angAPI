using System;

namespace DatingApp.API.DTO
{
    public class UserforListDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { set; get; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { set; get; }
        public string Country { set; get; }
        public string PhotoUrl { get; set; }
    }
}