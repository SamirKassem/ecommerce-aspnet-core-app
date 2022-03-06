using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ETickets.Models
{
    public class ShoppingCartItem
    {

        [Key]
        public int Id { get; set; }

        public Movie Movie { get; set; }
        public int Amount { get; set; }

        public string ShoppingCartId { get; set; }

       
    }
}
