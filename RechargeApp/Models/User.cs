using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RechargeApp.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int id { get; set; }

        public string Name { get; set; }
        [Key]
        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; }
        public string password { get; set; }

    }
}
