using KmcAPI.Model;
using Microsoft.EntityFrameworkCore;
namespace KmcAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{ }

		public DbSet<Event> Events { get; set; }
		public DbSet<Organizer> Organizers { get; set; }
		public DbSet<Participant> Participants { get; set; }
		public DbSet<EventRegistration> Registrations { get; set; }
		public DbSet<Admin> Admins { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Event>()
				.HasOne(e => e.Organizer)
				.WithMany(o => o.Events)
				.HasForeignKey(e => e.OrganizerId);

			modelBuilder.Entity<EventRegistration>()
				.HasOne(r => r.Event)
				.WithMany(e => e.Registrations)
				.HasForeignKey("EventId");

			
			modelBuilder.Entity<EventRegistration>()
				.HasOne(r => r.Participant)
				.WithMany(p => p.Registrations)
				.HasForeignKey("ParticipantId");

			modelBuilder.Entity<EventRegistration>()   
				.HasOne(r => r.Event)
				.WithMany(e => e.Registrations)        
				.HasForeignKey(r => r.Id)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<EventRegistration>()
				.HasOne(r => r.Event)
				.WithMany(e => e.Registrations)
				.HasForeignKey(r => r.EventId);

			modelBuilder.Entity<EventRegistration>()
				.HasOne(r => r.Participant)
				.WithMany(p => p.Registrations)
				.HasForeignKey(r => r.ParticipantId);

			modelBuilder.Entity<Organizer>().HasData(new Organizer
			{
				Id = 1,
				FullName = "Aloka Dewranga",
				OrganizationName = "Aloka Events",
				Email = "aloka@gmail.com",
				Password = "123456",
			
			});

			modelBuilder.Entity<Event>()
				.Property(e => e.EventImage)
				.HasColumnType("nvarchar(max)");
			modelBuilder.Entity<Admin>().HasData(
			new Admin
			{
				Id = 1,
				FullName = "KMC Admin",
				Email = "admin@kmc.lk",
				Password = "admin123"
			}
		);
		}
	}
}