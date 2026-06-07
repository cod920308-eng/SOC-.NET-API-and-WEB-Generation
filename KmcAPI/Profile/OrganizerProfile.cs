using AutoMapper;
using KmcAPI.DTO;
using KmcAPI.Model;

namespace KmcAPI.Profiles
{
	public class OrganizerProfile : Profile
	{
		public OrganizerProfile()
		{
			CreateMap<OrganizerWriteDTO, Organizer>();
			CreateMap<Organizer, OrganizerReadDTO>();
		}
	}
}