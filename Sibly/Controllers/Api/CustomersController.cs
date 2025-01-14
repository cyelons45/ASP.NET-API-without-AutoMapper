﻿using AutoMapper;
using Sibly.Dtos;
using Sibly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sibly.Controllers.Api
{
    public class CustomersController : ApiController
    {

       public ApplicationDbContext _context;


            public CustomersController ()
	    {
                _context = new ApplicationDbContext();
	    }
    



        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //GET/Api/customers
        public IHttpActionResult GetCustomers()
        {
          var custList=_context.Customers.ToList();
            return Ok(custList.Select(Mapper.Map<Customer,CustomerDto>));
        }

        //GET/Api/customer/id
        public IHttpActionResult GetCustomer(int id)
        {
           var customer=_context.Customers.SingleOrDefault(c => c.CustomerId == id);
           if (customer == null)
            {
                return NotFound();
            }
           return Ok(Mapper.Map<Customer, CustomerDto>(customer));
        }

        //POST/api/customers
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();
            customerDto.CustomerId = customer.CustomerId;
            return Created(new Uri(Request.RequestUri + "/" + customer.CustomerId), customerDto);
        }

        //PUT/api/customers/id
        [HttpPut]
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var customerInDB = _context.Customers.SingleOrDefault(c => c.CustomerId == id);
            if (customerInDB==null)
            {
               throw new HttpResponseException(HttpStatusCode.NotFound); 
            }
            Mapper.Map(customerDto, customerInDB);
            _context.SaveChanges();
         
        }





        //DELETE/api/customers/id
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
   

            var customerInDB = _context.Customers.SingleOrDefault(c => c.CustomerId == id);
            if (customerInDB == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _context.Customers.Remove(customerInDB);

            _context.SaveChanges();

        }

    }
}
