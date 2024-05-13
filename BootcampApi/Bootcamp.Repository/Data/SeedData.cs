using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bootcamp.Repository.Context;
using Domain.Entities;

namespace Bootcamp.Repository.Data
{
    public class SeedData
    {
        public static void SeedDatabase(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Categories.Any())
            {
                return;
            }

            var categories = new[]
            {
                new Category { Id = Guid.NewGuid(), Name = "Electronics" },
                new Category { Id = Guid.NewGuid(), Name = "Clothing" },
                new Category { Id = Guid.NewGuid(), Name = "Grocery" },
                new Category { Id = Guid.NewGuid(), Name = "Books" },
                new Category { Id = Guid.NewGuid(), Name = "Furniture" }
            };

            context.Categories.AddRange(categories);


            context.SaveChanges();
        }
    }
}
