using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace RechargeApp.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }

        public string Name { get; set; }   
        [Unique]
        public string PhoneNumber { get; set; }
        public string password { get; set; }
        public string Role { get; set; }

    }
}
