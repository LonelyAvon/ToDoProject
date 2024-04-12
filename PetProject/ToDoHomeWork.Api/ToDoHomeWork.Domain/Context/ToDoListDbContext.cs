using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoHomeWork.Domain.Configurations;
using ToDoHomeWork.Domain.Models;

namespace ToDoHomeWork.Domain.Context
{
    public class ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : DbContext(options)
    {
        public DbSet<ToDoList> toDoListSet { get; set; }

        protected override void OnModelCreating(ModelBuilder optionsBuilder)
        {
            optionsBuilder.ApplyConfiguration(new ToDoListConfiguration());
            base.OnModelCreating(optionsBuilder);
        }
    }
}
