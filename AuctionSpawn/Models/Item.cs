using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AuctionSpawn.Models
{
    public class Item : ModelBase<Item> 
    {
        public int? ItemID { get; set; } 

        public int AuctionID { get; set; }

        [Required(ErrorMessage = "Item Title is required")]
        public string Title { get; set; }

        //[StartPriceValidation]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than 0!")]
        public int StartPrice { get; set; }

        public Item()
        {
           
        }

        public Item(string _title, string _description, int _startPrice)
        {
            Title = _title;
            Description = _description;
            StartPrice = _startPrice;
        }
    }
}