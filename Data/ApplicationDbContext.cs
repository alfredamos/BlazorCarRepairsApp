using BlazorCarRepairsApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorCarRepairsApp.Data;

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
    {
        public DbSet<AssignedTicket> AssignedTickets { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //----> Configure one-to-one relationship between Customer and ApplicationUser.
        modelBuilder.Entity<ApplicationUser>().HasOne(c => c.Customer)
            .WithOne(u => u.User)
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //----> Configure one-to-one relationship between Customer and ApplicationUser.
        modelBuilder.Entity<ApplicationUser>().HasOne(c => c.Technician)
            .WithOne(u => u.User)
            .HasForeignKey<Technician>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //----> Configure one-to-many relationship between Customer and Ticket.
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Customer)
            .WithMany(c => c.Tickets)
            .HasForeignKey(t => t.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //----> Configure one-to-many relationship between AppicationUser and Token.
        modelBuilder.Entity<Token>()
            .HasOne(t => t.ApplicationUser)       // Token has one ApplicationUser
            .WithMany(u => u.Tokens)             // ApplicationUser has many Tokens
            .HasForeignKey(t => t.UserId) // Foreign key is in Token entity
            .OnDelete(DeleteBehavior.Cascade);   // Optional: Deletes tokens if user is deleted
        
        //----> Configure Composite Primary Key for the join entity
        modelBuilder.Entity<AssignedTicket>()
            .HasKey(at => new { at.TicketId, at.TechnicianId });

        //----> Configure Ticket to AssignedTicket
        modelBuilder.Entity<AssignedTicket>()
            .HasOne(at => at.Ticket)
            .WithMany(t => t.AssignedTickets)
            .HasForeignKey(at => at.TicketId)
            .OnDelete(DeleteBehavior.Restrict); // Optional: Deletes assigned tickets if ticket is deleted

        //----> Configure Technician to AssignedTicket
        modelBuilder.Entity<AssignedTicket>()
            .HasOne(at => at.Technician)
            .WithMany(te => te.AssignedTickets)
            .HasForeignKey(at => at.TechnicianId);   
            
        
        //----> Apply to Assigned-Ticket entity.
        modelBuilder.Entity<AssignedTicket>()
            .Property(p => p.AssignAt)
            .HasDefaultValueSql("GETUTCDATE()"); // SQL Server. Use CLOCK_TIMESTAMP() for PostgreSQL.
        
        //----> Apply to Customer entity.
        modelBuilder.Entity<Customer>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()"); // SQL Server. Use CLOCK_TIMESTAMP() for PostgreSQL.
 
        //----> Apply to Token entity.
        modelBuilder.Entity<Token>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()"); // SQL Server. Use CLOCK_TIMESTAMP() for PostgreSQL.
 
        //----> Apply to Ticket entity.
        modelBuilder.Entity<Technician>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()"); // SQL Server. Use CLOCK_TIMESTAMP() for PostgreSQL.
        
        //----> Apply to Ticket entity.
        modelBuilder.Entity<Ticket>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()"); // SQL Server. Use CLOCK_TIMESTAMP() for PostgreSQL.
 
    }
        
    }

