using KmcAPI.Data;
using KmcAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KmcAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private AdminRepo repo;
		private AppDbContext db;

		public AdminController(AdminRepo _repo, AppDbContext _db)
		{
			this.repo = _repo;
			this.db = _db;
		}

		
		[HttpPost("login")]
		public ActionResult Login(string email, string password)
		{
			var admin = repo.GetByEmail(email);
			if (admin == null || admin.Password != password)
				return Unauthorized("Invalid email or password!");
			return Ok(admin);
		}

		
		[HttpPost("addorganizer")]
		public ActionResult AddOrganizer(Organizer organizer)
		{
			var existing = db.Organizers
				.FirstOrDefault(o => o.Email == organizer.Email);
			if (existing != null)
				return BadRequest("Email already registered!");
			db.Organizers.Add(organizer);
			if (db.SaveChanges() > 0) return Ok(organizer);
			return BadRequest();
		}

		
		[HttpPost("addparticipant")]
		public ActionResult AddParticipant(Participant participant)
		{
			var existing = db.Participants
				.FirstOrDefault(p => p.Email == participant.Email);
			if (existing != null)
				return BadRequest("Email already registered!");
			db.Participants.Add(participant);
			if (db.SaveChanges() > 0) return Ok(participant);
			return BadRequest();
		}

		
		[HttpGet("registrations")]
		public ActionResult GetAllRegistrations()
		{
			var registrations = db.Registrations
				.Include(r => r.Event)
				.Include(r => r.Participant)
				.ToList();
			return Ok(registrations);
		}

		
		[HttpGet("organizers")]
		public ActionResult GetAllOrganizers()
		{
			return Ok(db.Organizers.ToList());
		}

		
		[HttpGet("participants")]
		public ActionResult GetAllParticipants()
		{
			return Ok(db.Participants.ToList());
		}

		
		[HttpDelete("organizer/{id}")]
		public ActionResult DeleteOrganizer(int id)
		{
			var organizer = db.Organizers.FirstOrDefault(o => o.Id == id);
			if (organizer == null) return NotFound();
			db.Organizers.Remove(organizer);
			if (db.SaveChanges() > 0) return Ok();
			return BadRequest();
		}

	
		[HttpDelete("participant/{id}")]
		public ActionResult DeleteParticipant(int id)
		{
			var participant = db.Participants
				.FirstOrDefault(p => p.Id == id);
			if (participant == null) return NotFound();
			db.Participants.Remove(participant);
			if (db.SaveChanges() > 0) return Ok();
			return BadRequest();
		}
	}
}