using System;
using System.ComponentModel.DataAnnotations;

namespace OtterTravels.Models
{
    public class Vacation
    {
        [Key]
        public int VacationId { get; set; }

        [Required(ErrorMessage="Destination is required")]
        public string Destination { get; set; }

        [Required(ErrorMessage="Image Url is required")]
        public string ImgUrl { get; set; }

        [Required(ErrorMessage="Start Date is required")]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage="Duration is required")]
        [Range(1,90)]
        public int NumberDays { get; set;}

        // This is the foreign key
        public int OtterId { get; set; }

        // A vacation can have only one Otter that plans it.
        // Not stored in database.
        public Otter Planner { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


    }
}