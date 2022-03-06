using ETickets.Models;
using System.Collections.Generic;

namespace ETickets.Data.ViewModel
{
    public class MovieDropDownsVM
    {
        public MovieDropDownsVM()
        {
            Producers = new List<Producer>();
            Actors = new List<Actor>();
            Cinemas = new List<Cinema>();
        }
        public List<Producer> Producers  { get; set; }
        public List<Cinema> Cinemas { get; set; }

        public List<Actor> Actors { get; set; }

    }
}
