using HieLie.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HieLie.Domain.Entities
{
    public class Customer : BaseEnity
    {
        public Guid Id { get; set; }                   
        public string FirstName { get; set; }          
        public string LastName { get; set; }           
        public string PhoneNumber { get; set; }          
        public string Address { get; set; }
        public Guid UserId { get; set; }
        public ICollection<Order>? Orders { get; set; } 

        public Customer(string firstName, string lastName, string phoneNumber, string address, Guid userId)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
            UserId = userId;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new ArgumentException("First Name is required");
            else if (string.IsNullOrWhiteSpace(LastName))
                throw new ArgumentException("Last Name is required");
            else if (string.IsNullOrWhiteSpace(PhoneNumber))
                throw new ArgumentException("Phone Number is required");
            else if (string.IsNullOrWhiteSpace(Address))
                throw new ArgumentException("Address is required");
            else if (UserId == Guid.Empty)
                throw new ArgumentException("User Id can't be undefined");
        }
    }
}
