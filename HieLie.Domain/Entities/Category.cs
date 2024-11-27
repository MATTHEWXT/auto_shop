using HieLie.Domain.Core;
using HieLie.Domain.Core.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace HieLie.Domain.Entities
{
    public class Category : BaseEnity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Product>? Products { get; set; } = new List<Product> { };
        public Category(string name)
        {
            Name = name;
        }
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Name is required");
        }
    }
}
