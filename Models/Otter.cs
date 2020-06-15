using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OtterTravels.Models
{
    public class Otter
    {
        [Key]
        public int OtterId {get; set;}

        [Required(ErrorMessage="Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage="Birthday is required")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage="Type is required")]
        public string Type { get; set; }

        [Required(ErrorMessage="HasKids is required")]
        public bool HasKids {get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // An Otter can plan many vacations
        // Not stored in database.
        public List<Vacation> PlannedVacations { get; set; }
    }
}