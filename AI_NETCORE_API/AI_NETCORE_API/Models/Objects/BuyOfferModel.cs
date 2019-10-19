﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AI_NETCORE_API.Models.Objects
{
    public class BuyOfferModel
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ResourceId { get; set; }
        public decimal MaxPrice { get; set; }
        public DateTime Date { get; set; }
        public bool IsValid { get; set; }
    }
}
