using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BSshop.Models
{
    public class Reservation
    {

        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }  
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int FromMinutes { get; set; }
        public int ToMinutes { get; set; }
        public int PeopleNumber { get; set; }
    }
}
