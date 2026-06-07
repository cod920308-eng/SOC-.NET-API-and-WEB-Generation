using KmcAPI.Data;
using KmcAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace KmcAPI.Repos
{
	public class EventRepo
	{
		private AppDbContext db;

		public EventRepo(AppDbContext _db)
		{
			this.db = _db;
		}

		public bool Create(Event ev)
		{
			db.Events.Add(ev);
			return db.SaveChanges() > 0;
		}

		
		public List<Event> GetEvents()
		{
			return db.Events
					 .Include(e => e.Organizer)
					 .Where(e => e.EventPlatform == "Private")
					 .OrderByDescending(e => e.StartDate)
					 .ToList();
		}

		
		public List<Event> GetPublicEventsForKMC()
		{
			return db.Events
					 .Include(e => e.Organizer)
					 .Where(e => e.EventPlatform == "Private" ||
								e.EventPlatform == "KMC" ||
								e.EventPlatform == null)   
					 .OrderByDescending(e => e.StartDate)
					 .ToList();
		}

		public Event? GetEventById(int id)
		{
			return db.Events
					 .Include(e => e.Organizer)
					 .FirstOrDefault(e => e.Id == id);
		}

		public bool Update(Event ev)
		{
			db.Events.Update(ev);
			return db.SaveChanges() > 0;
		}

		public bool Remove(Event ev)
		{
			db.Events.Remove(ev);
			return db.SaveChanges() > 0;
		}

		public IEnumerable<Event> GetEventsByOrganizer(int organizerId)
		{
			return db.Events
					 .Include(e => e.Organizer)
					 .Where(e => e.OrganizerId == organizerId &&
								(e.EventPlatform == "KMC" ||
								 e.EventPlatform == "Private" ||
								 e.EventPlatform == null))   
					 .OrderByDescending(e => e.StartDate)
					 .ToList();
		}

		public List<Event> Search(string? type, string? location, DateTime? date)
		{
			var query = db.Events
						  .Include(e => e.Organizer)
						  .Where(e => e.EventPlatform == "Private")
						  .AsQueryable();

			if (!string.IsNullOrEmpty(type))
				query = query.Where(e => e.Type == type);

			if (!string.IsNullOrEmpty(location))
				query = query.Where(e => e.Location.Contains(location));

			if (date.HasValue)
				query = query.Where(e => e.StartDate.Date == date.Value.Date);

			return query.ToList();
		}
	}
}