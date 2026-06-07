using AutoMapper;
using KmcAPI.DTO;
using KmcAPI.Model;

public class EventProfile : Profile
{
	public EventProfile()
	{
		CreateMap<Event, EventReadDTO>()
			.ForMember(dest => dest.OrganizerName,
				opt => opt.MapFrom(src => src.Organizer!.FullName));
		
	}
}