using ETickets.Data.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETickets.Models
{
    public class Cinema:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Display(Name = "Cinema Logo")]
        [Required(ErrorMessage = "Logo is required.")]

        public string Logo { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required.")]

        public string Description { get; set; }

        //Relations
        public List<NewMovieVM> Movies{ get; set; }

    }
}
