using HieLie.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HieLie.Domain.Entities
{
    public class Product : BaseEnity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; } = default!;

        public Product(string name, decimal price, string imagePath, Guid categoryId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            ImagePath = imagePath;
            CategoryId = categoryId;
        }
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Name is required");
            else if (Price == 0)
                throw new ArgumentException("Price can't be zero");
            else if (string.IsNullOrWhiteSpace(ImagePath))
                throw new ArgumentException("ImagePath can't be undefined");
            else if(CategoryId == Guid.Empty)
                throw new ArgumentException("Category can't be undefined");
        }
    }
}
