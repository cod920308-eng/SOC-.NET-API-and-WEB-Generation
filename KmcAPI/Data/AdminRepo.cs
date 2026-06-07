using KmcAPI.Model;
namespace KmcAPI.Data
{
	public class AdminRepo
	{
		private AppDbContext db;
		public AdminRepo(AppDbContext _db)
		{
			this.db = _db;
		}

		public Admin? GetByEmail(string email)
		{
			return db.Admins.FirstOrDefault(a => a.Email == email);
		}

		public List<Admin> GetAll()
		{
			return db.Admins.ToList();
		}
	}
}