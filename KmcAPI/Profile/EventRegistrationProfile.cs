using AutoMapper;
using KmcAPI.DTO;
using KmcAPI.Model;

namespace KmcAPI.Profiles
{
	public class EventRegistrationProfile : Profile
	{
		public EventRegistrationProfile()
		{
			CreateMap<EventRegistration, EventRegistrationReadDTO>()
				.ForMember(dest => dest.EventTitle,
					opt => opt.MapFrom(src => src.Event!.Title))
				.ForMember(dest => dest.EventLocation,
					opt => opt.MapFrom(src => src.Event!.Location))
				.ForMember(dest => dest.EventStartDate,
					opt => opt.MapFrom(src => src.Event!.StartDate))
				.ForMember(dest => dest.ParticipantName,
					opt => opt.MapFrom(src => src.Participant!.FullName))
				.ForMember(dest => dest.ParticipantEmail,
					opt => opt.MapFrom(src => src.Participant!.Email));
		}
	}
}