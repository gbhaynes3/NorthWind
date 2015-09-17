﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Northwind.Api.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipName { get; set; }
    }
}