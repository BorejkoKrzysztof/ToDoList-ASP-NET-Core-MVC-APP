using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;

namespace ToDoListInfrastructure.Database
{
    public class ToDoListAppDbContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        // ctor for migration
        public ToDoListAppDbContext(DbContextOptions<ToDoListAppDbContext> options) : base(options)
        {

        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDoEntry> ToDoEntries { get; set; }
        public DbSet<NotesTde> Notes_ToDoEntry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoList>()
                        .HasMany(x => x.ToDoEntries)
                        .WithOne(c => c.ToDoList);

            modelBuilder.Entity<ToDoEntry>()
                        .HasMany(x => x.AdditionalNotes)
                        .WithOne(c => c.ToDoEntry);
        }
    }
}
