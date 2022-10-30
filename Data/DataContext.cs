using System;
using Microsoft.EntityFrameworkCore;

namespace amusement_park.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options) : base(options) { }

        public DbSet<Customer> customer { get; set; }

        public DbSet<Employee> employee { get; set; }
    }
}

