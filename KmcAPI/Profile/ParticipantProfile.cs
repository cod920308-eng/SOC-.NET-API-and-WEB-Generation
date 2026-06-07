using AutoMapper;
using KmcAPI.DTO;
using KmcAPI.Model;

namespace KmcAPI.Profiles
{
	public class ParticipantProfile : Profile
	{
		public ParticipantProfile()
		{
			CreateMap<ParticipantWriteDTO, Participant>();
			CreateMap<Participant, ParticipantReadDTO>();
		}
	}
}