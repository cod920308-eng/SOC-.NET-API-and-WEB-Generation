using KmcAPI.Data;
using KmcAPI.Model;

namespace KmcAPI.Repos
{
	public class ParticipantRepo
	{
		private AppDbContext db;
		public ParticipantRepo(AppDbContext _db)
		{
			this.db = _db;
		}

		public bool Create(Participant participant)
		{
			db.Participants.Add(participant);
			return db.SaveChanges() > 0;
		}

		public Participant? GetByEmail(string email)
		{
			return db.Participants.FirstOrDefault(p => p.Email == email);
		}

		public Participant? GetById(int id)
		{
			return db.Participants.FirstOrDefault(p => p.Id == id);
		}

		public List<Participant> GetAll()
		{
			return db.Participants.ToList();
		}

		public bool EmailExists(string email)
		{
			return db.Participants.Any(p => p.Email == email);
		}
	}
}