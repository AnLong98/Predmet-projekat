
using SmartEnergy.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Users.DomainModels
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDay { get; set; }
        public UserType UserType { get; set; }
        public string ImageURL { get; set; }
        public int? CrewID { get; set; }
        public Crew Crew { get; set; }
        public int LocationID { get; set; }
        public Consumer Consumer { get; set; }
        public UserStatus UserStatus { get; set; }

    }
}
