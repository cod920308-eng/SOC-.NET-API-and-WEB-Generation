using KmcAPI.Data;
using KmcAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace KmcAPI.Repos
{
	public class EventRegistrationRepo
	{
		private AppDbContext db;
		public EventRegistrationRepo(AppDbContext _db)
		{
			this.db = _db;
		}

		public bool Create(EventRegistration registration)
		{
			db.Registrations.Add(registration);
			return db.SaveChanges() > 0;
		}

		public bool AlreadyRegistered(int eventId, int participantId)
		{
			return db.Registrations
				.Any(r => r.EventId == eventId
					   && r.ParticipantId == participantId); 
		}

		public int GetRegistrationCount(int eventId)
		{
			return db.Registrations.Count(r => r.EventId == eventId);
		}

		public List<EventRegistration> GetByEvent(int eventId)
		{
			return db.Registrations
				.Include(r => r.Participant)
				.Where(r => r.EventId == eventId)
				.ToList();
		}

		public List<EventRegistration> GetByParticipant(int participantId)
		{
			return db.Registrations
				.Include(r => r.Event)
				.Where(r => r.ParticipantId == participantId)
				.ToList();
		}
	}
}