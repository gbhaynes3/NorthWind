using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Northwind.Api.Models
{
    public class NorthwindContext: DbContext
    {

        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<OrderDetail> OrderDetails { get; set; }

        public NorthwindContext() : base("name=NorthWind")
        {
            
        }

        
    }
}