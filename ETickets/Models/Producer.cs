using ETickets.Data.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETickets.Models
{
    public class Producer:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage ="Profile Picture is required.")]
        public string ProfilePictureURL { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 50 characters.")]
        public string FullName { get; set; }

        [Display(Name = "Biography")]
        [Required(ErrorMessage ="Biography is required.")]
        public string Bio { get; set; }

        //Relations
        public List<NewMovieVM> Movies{ get; set; }
    }
}
