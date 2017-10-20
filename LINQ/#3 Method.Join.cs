using System;
using System.Collections.Generic;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // data source 1
            List<Customer> customers = new List<Customer>()
            {
                new Customer { Name = "Robin", Family = "Black", Age = 26, Company = "Oculus" },
                new Customer { Name = "Kate", Family = "Walken", Age = 23, Company = "Microsoft" },
                new Customer { Name = "Alex", Family = "Usov", Age = 30, Company = "Beeline" },
                new Customer { Name = "Tom", Family = "Jones", Age = 32, Company = "Google" },
                new Customer { Name = "Elena", Family = "Scheider", Age = 28, Company = "Yandex" }
            };
            // data source 2
            List<Address> adresses = new List<Address>()
            {
                new Address { Country = "US", City = "Austin", Company = "Oculus" },
                new Address { Country = "US", City = "Redmond", Company = "Microsoft" },
                new Address { Country = "Russia", City = "Yaroslavl", Company = "Beeline" },
                new Address { Country = "US", City = "Mountain View", Company = "Google" },
                new Address { Country = "Russia", City = "Moscow", Company = "Yandex" }
            };


            // merge sources
            var result = customers

                .Select(cust => new { cust.Name, cust.Family, cust.Company })
                .Join(adresses, cust => cust.Company,
                                addr => addr.Company,

                (cust, addr) => new
                {
                    cust.Name,
                    cust.Family,
                    addr.Company,
                    addr.Country,
                    addr.City
                })
                .ToList();

            // information from both data sources
            result?.ForEach(r => Console.WriteLine(
                $"Customer: {r.Name} {r.Family} from {r.City} ({r.Country}), Company: \"{r.Company}\""));
            /*
            Customer: Robin Black from Austin (US), Company: "Oculus"
            Customer: Kate Walken from Redmond (US), Company: "Microsoft"
            Customer: Alex Usov from Yaroslavl (Russia), Company: "Beeline"
            Customer: Tom Jones from Mountain View (US), Company: "Google"
            Customer: Elena Scheider from Moscow (Russia), Company: "Yandex"
            */
        }
    }

    class Customer
    {
        public byte Age { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Company { get; set; }
    }

    class Address
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Company { get; set; }
    }
}
