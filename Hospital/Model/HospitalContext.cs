using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Model
{
    public class HospitalContext : IdentityDbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
            :base(options)
        {
        }

        public DbSet<Dose> Doses { get; set; }  
        public DbSet<Dosagem> Dosagens { get; set; }  
        public DbSet<Prescricao> Prescricoes { get; set; }   

        //modelBuilder
        /*protected override void OnModelCreating(DbContextOptions modelBuilder)
        {
            modelBuilder.Entity<Prescricao>()
                        .HasMany(p => p.Dosagens)
                        .WithOne()
                        .HasForeignKey(dos => dos.PrescricaoId);
            modelBuilder.Entity<Dosagem>()
                        .HasKey(dos => new { dos.ID, dos.PrescricaoId });
        }*/

    }
}
