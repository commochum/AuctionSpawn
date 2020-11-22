using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuctionSpawn.Models
{

    [DisplayName("Auction")]
    public class Auction : ModelBase<Auction>
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime Date { get; set; }

        public IEnumerable<Item> AuctionItems { get; set; }

        public int ItemQuantity { get; set; }

        public Auction()
        {
            ItemQuantity = Configuration.ItemQuantity;
        }

        public Auction(int _id, int _itemQuantity)
        {
            ID = _id;
            ItemQuantity = _itemQuantity;
        }
    }
}