using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AuctionSpawn.Models
{
    public abstract class ModelBase<T>
    {
        public virtual string Description { get; set; }
        
        //TODO: 
        public virtual string ToString(string message)
        {
            var sb = new StringBuilder();

            var timeStamp = string.Format($"Sent on {DateTime.Now: D} at {DateTime.Now:t} ");

            sb.AppendLine(timeStamp);
            sb.AppendLine("");
            sb.AppendLine("Description" + Description + ", ");
            sb.AppendLine(message);

            return sb.ToString();
        }
    }
}