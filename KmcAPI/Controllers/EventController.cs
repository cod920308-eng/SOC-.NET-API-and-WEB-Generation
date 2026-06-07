using AutoMapper;
using KmcAPI.DTO;
using KmcAPI.Model;
using KmcAPI.Repos;
using Microsoft.AspNetCore.Mvc;

namespace KmcAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventController : ControllerBase
	{
		private IMapper mapper;
		private EventRepo repo;

		public EventController(IMapper _mapper, EventRepo _repo)
		{
			this.mapper = _mapper;
			this.repo = _repo;
		}

		
		[HttpPost]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> Create(
			[FromForm] string title,
			[FromForm] string type,
			[FromForm] int capacity,
			[FromForm] string location,
			[FromForm] DateTime startDate,
			[FromForm] DateTime? endDate,
			[FromForm] string? description,
			[FromForm] int organizerId,
			[FromForm] string? status,
			[FromForm] string? eventPlatform = null,   
			IFormFile? eventImageFile = null)
		{
			
			string? imagePath = null;
			if (eventImageFile != null && eventImageFile.Length > 0)
			{
				var uploadsFolder = Path.Combine(
					Directory.GetCurrentDirectory(), "wwwroot", "images", "events");
				Directory.CreateDirectory(uploadsFolder);
				var uniqueFileName = Guid.NewGuid().ToString()
					+ Path.GetExtension(eventImageFile.FileName);
				var filePath = Path.Combine(uploadsFolder, uniqueFileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await eventImageFile.CopyToAsync(stream);
				}
				imagePath = "/images/events/" + uniqueFileName;
			}

		
			var newEvent = new Event
			{
				Title = title,
				Type = type,
				Capacity = capacity,
				Location = location,
				StartDate = startDate,
				EndDate = endDate ?? DateTime.Now.AddDays(1),
				Description = description,
				OrganizerId = organizerId,
				EventImage = imagePath,
				CurrentParticipants = 0,
				Status = status ?? "Active",
				EventPlatform = eventPlatform ?? "Private"     
			};

			if (repo.Create(newEvent)) return Ok(newEvent);
			return BadRequest();
		}

		
		[HttpGet]
		public ActionResult<List<EventReadDTO>> GetEvents()
		{
			var events = repo.GetEvents();
			return Ok(mapper.Map<List<EventReadDTO>>(events));
		}
		
		[HttpGet("{id}")]
		public ActionResult<EventReadDTO> GetEvent(int id)
		{
			var ev = repo.GetEventById(id);
			if (ev == null) return NotFound();
			return Ok(mapper.Map<EventReadDTO>(ev));
		}

		
		[HttpGet("organizer/{organizerId}")]
		public ActionResult<IEnumerable<EventReadDTO>> GetEventsByOrganizer(int organizerId)
		{
			var events = repo.GetEventsByOrganizer(organizerId);
			return Ok(mapper.Map<List<EventReadDTO>>(events));
		}

		[HttpGet("public/kmc")]
		public ActionResult<List<EventReadDTO>> GetPublicEventsForKMC()
		{
			var events = repo.GetPublicEventsForKMC();
			return Ok(mapper.Map<List<EventReadDTO>>(events));
		}

		
		[HttpGet("search")]
		public ActionResult<List<EventReadDTO>> Search(
			string? type, string? location, DateTime? date)
		{
			var events = repo.Search(type, location, date);
			return Ok(mapper.Map<List<EventReadDTO>>(events));
		}

		[HttpPut("{id}")]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> UpdateEvent(
			int id,
			[FromForm] string title,
			[FromForm] string type,
			[FromForm] int capacity,
			[FromForm] string location,
			[FromForm] DateTime startDate,
			[FromForm] DateTime? endDate,
			[FromForm] string? description,
			[FromForm] int organizerId,
			[FromForm] string? status,
			[FromForm] string? eventPlatform = null,     
			IFormFile? eventImageFile = null)
		{
			var existing = repo.GetEventById(id);
			if (existing == null) return NotFound();

		
			if (existing.OrganizerId != organizerId)
				return Unauthorized("Only the creator can update this event!");

			
			if (eventImageFile != null && eventImageFile.Length > 0)
			{
				var uploadsFolder = Path.Combine(
					Directory.GetCurrentDirectory(), "wwwroot", "images", "events");
				Directory.CreateDirectory(uploadsFolder);
				var uniqueFileName = Guid.NewGuid().ToString()
					+ Path.GetExtension(eventImageFile.FileName);
				var filePath = Path.Combine(uploadsFolder, uniqueFileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await eventImageFile.CopyToAsync(stream);
				}
				existing.EventImage = "/images/events/" + uniqueFileName;
			}

			
			existing.Title = title;
			existing.Description = description;
			existing.Type = type;
			existing.Location = location;
			existing.StartDate = startDate;
			existing.EndDate = endDate ?? existing.EndDate;
			existing.Capacity = capacity;
			existing.Status = status ?? existing.Status;

			
			if (!string.IsNullOrEmpty(eventPlatform))
				existing.EventPlatform = eventPlatform;

			if (repo.Update(existing)) return Ok(existing);
			return BadRequest();
		}

	
		[HttpDelete("{id}")]
		public ActionResult DeleteEvent(int id)
		{
			var ev = repo.GetEventById(id);
			if (ev == null) return NotFound();
			if (repo.Remove(ev)) return Ok();
			return BadRequest();
		}

	}

}