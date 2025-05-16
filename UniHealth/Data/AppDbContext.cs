using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniHealth.Models;

namespace UniHealth.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<StudentDetail> StudentDetails { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<DoctorId> DoctorIds { get; set; }
        public DbSet<PasswordReset> PasswordReset { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Roles 
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Student", NormalizedName = "STUDENT" },
                new IdentityRole { Name = "Doctor", NormalizedName = "DOCTOR" },
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }
            );

            builder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Student" },
            new Role { Id = 2, Name = "Doctor" },
            new Role { Id = 3, Name = "Admin" }
        );

            // User Table 
            builder.Entity<User>(entity =>
            {
                entity.Property(u => u.FName).HasColumnName("FirstName");
                entity.Property(u => u.LName).HasColumnName("LastName");

                entity.HasOne(u => u.Role)
                    .WithMany()
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // Student Table

            builder.Entity<StudentDetail>(entity =>
            {
                entity.HasOne(s => s.User)
                    .WithOne()
                    .HasForeignKey<StudentDetail>(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Univeristy)
                    .WithMany()
                    .HasForeignKey(s => s.UniveristyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.College)
                    .WithMany()
                    .HasForeignKey(s => s.CollegeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Doctor Table
            builder.Entity<Doctor>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithOne(u => u.Doctor)
                    .HasForeignKey<Doctor>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.University)
                    .WithMany()
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Doctor Id
            builder.Entity<DoctorId>(entity =>
            {
                entity.HasIndex(d => d.DoctorUniversityId).IsUnique();
            });

            // Report Table 
            builder.Entity<Report>(entity =>
            {
                entity.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Doctor)
                    .WithMany()
                    .HasForeignKey(r => r.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<MedicalRecord>().HasIndex(r => r.createdAt);
            builder.Entity<University>().HasIndex(u => u.Name);
            builder.Entity<College>().HasIndex(c => c.Name);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

    }
}
