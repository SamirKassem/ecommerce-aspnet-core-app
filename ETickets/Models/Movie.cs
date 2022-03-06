using ETickets.Data;
using ETickets.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETickets.Models
{
    public class Movie:IEntityBase
    {
        [Key]
        public int Id { get; set; }


        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public string ImageUrl { get; set; }

        public DateTime StartDate{ get; set; }
        public DateTime EndDate{ get; set; }

        public MovieCategory Category{ get; set; }

        //Relations
        public List<Actor_Movie> Actors_Movies{ get; set; }

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }

        public int ProducerId { get; set; }
        public Producer Producer { get; set; }



    }
}
