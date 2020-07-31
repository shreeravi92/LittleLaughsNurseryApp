using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LittleLaughsNurseryApp.Models
{
    public class Rating
    {
        public string timings { get; set; }
        public int rate_Monthly { get; set; }
        public int rate_Weekly { get; set; }
        public int threeDaysPerWeek { get; set; }
    }
}