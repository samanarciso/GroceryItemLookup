using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GroceryItemLookup.Data;
using System;
using System.Linq;

namespace GroceryItemLookup.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GroceryItemLookupContext(
                serviceProvider.GetRequiredService<DbContextOptions<GroceryItemLookupContext>>()))
            {
                context.Database.EnsureCreated();
                if (context.Product.Any() || context.Department.Any() || context.Employee.Any())
                {
                    return;
                }

                if (!context.Department.Any())
                {
                    var departments = new[]
                    {
                        new Department { Name = "Candy" },
                        new Department { Name = "Frozen" },
                        new Department { Name = "Snacks" },
                        new Department { Name = "Grocery" },
                        new Department { Name = "Beverage" }
                    };

                    context.Department.AddRange(departments);
                    context.SaveChanges();
                }

                if (!context.Employee.OfType<Supervisor>().Any())
                {
                    var supervisors = new[]
                    {
                        new Supervisor
                        {
                            FirstName = "Kim",
                            LastName = "Abercrombie",
                            HireDate = new DateTime(2015, 3, 11),
                        },
                        new Supervisor
                        {
                            FirstName = "Fadi",
                            LastName = "Fakhouri",
                            HireDate = new DateTime(2016, 7, 6),
                        },
                        new Supervisor
                        {
                            FirstName = "Roger",
                            LastName = "Harui",
                            HireDate = new DateTime(2018, 1, 15),
                        },
                        new Supervisor
                        {
                            FirstName = "Candace",
                            LastName = "Kapoor",
                            HireDate = new DateTime(2020, 2, 10),
                        },
                        new Supervisor
                        {
                            FirstName = "Roger",
                            LastName = "Zheng",
                            HireDate = new DateTime(2019, 9, 21),
                        }
                    };

                    context.Employee.AddRange(supervisors);
                    context.SaveChanges();
                }

                if (!context.Employee.Where(e => !(e is Supervisor)).Any())
                {
                    var employees = new[]
                    {
                        new Employee
                        {
                            FirstName = "Laura",
                            LastName = "Norman",
                            HireDate = new DateTime(2021, 5, 20),
                        },
                        new Employee
                        {
                            FirstName = "Carson",
                            LastName = "Alexander",
                            HireDate = new DateTime(2022, 9, 10),
                        },
                        new Employee
                        {
                            FirstName = "Peggy",
                            LastName = "Justice",
                            HireDate = new DateTime(2023, 1, 5),
                        },
                        new Employee
                        {
                            FirstName = "Yan",
                            LastName = "Li",
                            HireDate = new DateTime(2020, 12, 14),
                        },
                        new Employee
                        {
                            FirstName = "Arturo",
                            LastName = "Anand",
                            HireDate = new DateTime(2021, 3, 30),
                        }
                    };

                    context.Employee.AddRange(employees);
                    context.SaveChanges();
                }

                var allDepartments = context.Department.ToList();
                var allSupervisors = context.Employee.OfType<Supervisor>().ToList();

                for (int i = 0; i < allDepartments.Count && i < allSupervisors.Count; i++)
                {
                    allDepartments[i].Supervisors.Add(allSupervisors[i]);
                    allSupervisors[i].Departments.Add(allDepartments[i]);
                }

                context.SaveChanges();

                if (!context.Product.Any())
                {
                    var candy = context.Department.Single(d => d.Name == "Candy");
                    var frozen = context.Department.Single(d => d.Name == "Frozen");
                    var snacks = context.Department.Single(d => d.Name == "Snacks");
                    var grocery = context.Department.Single(d => d.Name == "Grocery");
                    var beverage = context.Department.Single(d => d.Name == "Beverage");

                    context.Product.AddRange(
                        new Product
                        {
                            Name = "Dark Chocolate Almond Bark",
                            Description = "Rich dark chocolate almond bark with almond bits",
                            Price = 7.99M,
                            Department = candy,
                            Weight = "16 oz"
                        },
                        new Product
                        {
                            Name = "Mandarin Orange Chicken",
                            Description = "Pre cooked fried chicken pieces with orange sauce",
                            Price = 4.99M,
                            Department = frozen,
                            Weight = "24 oz"
                        },
                        new Product
                        {
                            Name = "Olive Oil Popcorn",
                            Description = "Popped corn seasoned with salt and olive oil",
                            Price = 2.99M,
                            Department = snacks,
                            Weight = "16 oz"
                        },
                        new Product
                        {
                            Name = "Rigatoni",
                            Description = "Italian rigatoni pasta",
                            Price = 1.99M,
                            Department = grocery,
                            Weight = "16 oz"
                        },
                        new Product
                        {
                            Name = "Spring Water 1 LT",
                            Description = "1 liter of mountain spring water",
                            Price = 1.99M,
                            Department = beverage,
                            Weight = "16 oz"
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
