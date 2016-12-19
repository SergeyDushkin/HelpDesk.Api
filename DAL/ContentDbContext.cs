using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace servicedesk.api
{
    public class ContentDbContext : DbContext
    {
        public ContentDbContext(DbContextOptions<ContentDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>().Property(p => p.Id).ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<File> Files { get; set; }

        [Table("WH_FILES")]
        public class File 
        {
            [Key]
            public Guid Id { get; set; }
            public Guid ReferenceId { get; set; }
            public string Name { get; set; }
            public string FileType { get; set; }
            public string ContentType { get; set; }
            public double Size { get; set; }
            public byte[] Content { get; set; }
        }
    }
}
