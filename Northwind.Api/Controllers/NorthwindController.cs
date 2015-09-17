using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Breeze.ContextProvider.EF6;
using Breeze.WebApi2;
using Northwind.Api.Models;

namespace Northwind.Api.Controllers
{
    [BreezeController]
    public class NorthwindController : ApiController
    {
        private readonly EFContextProvider<NorthwindContext> _context = new EFContextProvider<NorthwindContext>();

        [HttpGet]
        public string Metadata()
        {
            return _context.Metadata();
        }

        [HttpGet]
        public IQueryable<Customer> Customers()
        {
            return _context.Context.Customers.AsQueryable();
        }

        [HttpGet]
        public IQueryable<Order> Orders()
        {
            return _context.Context.Orders.AsQueryable();
        }
        
        [HttpGet]
        public IQueryable<OrderDetail> OrderDetails()
        {
            return _context.Context.OrderDetails.AsQueryable();
        }

        [HttpGet]
        public string test()
        {
            return "this is a test";
        }
    }
}
