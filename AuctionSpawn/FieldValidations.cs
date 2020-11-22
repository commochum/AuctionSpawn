using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AuctionSpawn
{
    public class StartPriceValidation : ValidationAttribute 
    {
        //<TODO> NOT USE
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int startPrice = Convert.ToInt32(value.ToString());

            if(startPrice == 0)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Start Price must not be 0");
        }
    }
}