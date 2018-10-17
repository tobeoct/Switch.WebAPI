using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Switch.WebAPI.Models
{
    public class Message
    {

        public int TransactionType { get; set; }
        public int Channel { get; set; }
        public double Amount { get; set; }

    }
}