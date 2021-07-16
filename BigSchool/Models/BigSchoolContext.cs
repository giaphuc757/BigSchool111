using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BigSchool.Models
{
    public partial class BigSchoolContext : DbContext
    {
        public BigSchoolContext()
            : base("name=BigSchoolContext1")
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<course> courses { get; set; }
        public virtual DbSet<Following> Followings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.courses)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<course>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.course)
                .WillCascadeOnDelete(false);
        }
    }
}
