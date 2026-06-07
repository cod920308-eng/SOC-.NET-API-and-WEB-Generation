using KmcAPI.Data;
using KmcAPI.Model;
using KmcAPI.Repos;
using Microsoft.AspNetCore.Mvc;
using KmcAPI.DTO;
using Microsoft.EntityFrameworkCore;

namespace KmcAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventRegistrationController : ControllerBase
	{
		private EventRegistrationRepo repo;
		private AppDbContext db;

		public EventRegistrationController(EventRegistrationRepo _repo, AppDbContext _db)
		{
			this.repo = _repo;
			this.db = _db;
		}

		[HttpPost]
		public ActionResult RegisterForEvent([FromBody] EventRegistrationWriteDTO dto)
		{
			if (dto == null || dto.EventId <= 0 || dto.ParticipantId <= 0)
				return BadRequest("Invalid registration data");

			var ev = db.Events.FirstOrDefault(e => e.Id == dto.EventId);
			if (ev == null) return NotFound("Event not found!");

			var participant = db.Participants.FirstOrDefault(p => p.Id == dto.ParticipantId);
			if (participant == null) return NotFound("Participant not found!");

			if (repo.AlreadyRegistered(dto.EventId, dto.ParticipantId))
				return BadRequest("Already registered for this event!");

			if (repo.GetRegistrationCount(dto.EventId) >= ev.Capacity)
				return BadRequest("Event is fully booked!");

			
			var registration = new EventRegistration
			{
				EventId = dto.EventId,     
				ParticipantId = dto.ParticipantId, 
				RegisteredAt = DateTime.Now
			};

			db.Registrations.Add(registration);
			ev.CurrentParticipants += 1;

			if (db.SaveChanges() > 0)
				return Ok("Registered successfully");

			return BadRequest("Failed to create registration");
		}
		[HttpGet("registrations")]
		public ActionResult GetAllRegistrations()
		{
			var registrations = db.Registrations
				.Include(r => r.Event)
				.Include(r => r.Participant)
				.Select(r => new {
					id = r.Id,
					registeredAt = r.RegisteredAt,
					eventId = r.EventId,
					participantId = r.ParticipantId,
					eventTitle = r.Event!.Title,
					eventLocation = r.Event.Location,
					participantName = r.Participant!.FullName,
					participantEmail = r.Participant.Email
				})
				.ToList();
			return Ok(registrations);
		}
		
		[HttpGet("organizer/{organizerId}")]
		public ActionResult GetRegistrationsByOrganizer(int organizerId)
		{
			var registrations = db.Registrations
				.Include(r => r.Event)
				.Include(r => r.Participant)
				.Where(r => r.Event.OrganizerId == organizerId)   
				.Select(r => new
				{
					id = r.Id,
					registeredAt = r.RegisteredAt,
					eventId = r.EventId,
					participantId = r.ParticipantId,
					eventTitle = r.Event!.Title,
					eventLocation = r.Event.Location,
					participantName = r.Participant!.FullName,
					participantEmail = r.Participant.Email
				})
				.OrderByDescending(r => r.registeredAt)  
				.ToList();

			return Ok(registrations);
		}

		[HttpGet("event/{eventId}")]
		public ActionResult GetEventRegistrations(int eventId)
		{
			return Ok(repo.GetByEvent(eventId));
		}

		
		[HttpGet("participant/{participantId}")]
		public ActionResult GetParticipantRegistrations(int participantId)
		{
			var registrations = db.Registrations
				.Where(r => r.ParticipantId == participantId)
				.Select(r => new {
					id = r.Id,
					eventId = r.EventId,        
					registeredAt = r.RegisteredAt
				})
				.ToList();
			return Ok(registrations);
		}

	}
}