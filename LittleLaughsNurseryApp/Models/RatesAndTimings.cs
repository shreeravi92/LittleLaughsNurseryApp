using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LittleLaughsNurseryApp.Models
{
    public class RatesAndTimings
    {
        public List<Rating> rateDetails { get; set; }
        public List<Meal_Chart> mealDetails { get; set; }
    }
}