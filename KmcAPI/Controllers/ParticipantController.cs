using AutoMapper;
using KmcAPI.DTO;
using KmcAPI.Model;
using KmcAPI.Repos;
using Microsoft.AspNetCore.Mvc;

namespace KmcAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ParticipantController : ControllerBase
	{
		private IMapper mapper;
		private ParticipantRepo repo;

		public ParticipantController(IMapper _mapper, ParticipantRepo _repo)
		{
			this.mapper = _mapper;
			this.repo = _repo;
		}

		[HttpPost("register")]
		public ActionResult Register(ParticipantWriteDTO dtoParticipant)
		{
			if (repo.EmailExists(dtoParticipant.Email!))
				return BadRequest("Email already registered!");

			var model = mapper.Map<Participant>(dtoParticipant);
			if (repo.Create(model)) return Ok(model);
			return BadRequest();
		}

		[HttpPost("login")]
		public ActionResult Login(string email, string password)
		{
			var participant = repo.GetByEmail(email);
			if (participant == null || participant.Password != password)
				return Unauthorized("Invalid email or password!");
			return Ok(participant);
		}

		[HttpGet]
		public ActionResult<List<ParticipantReadDTO>> GetParticipants()
		{
			return Ok(mapper.Map<List<ParticipantReadDTO>>(repo.GetAll()));
		}

		[HttpGet("{id}")]
		public ActionResult<ParticipantReadDTO> GetParticipant(int id)
		{
			var participant = repo.GetById(id);
			if (participant == null) return NotFound();
			return Ok(mapper.Map<ParticipantReadDTO>(participant));
		}
	}
}