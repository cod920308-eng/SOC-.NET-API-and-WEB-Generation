using System.ComponentModel.DataAnnotations;

namespace KmcAPI.Model
{
    public class Event
    {
		[Key]
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Type { get; set; }        
		public string? Location { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int Capacity { get; set; }
		public string? EventImage { get; set; }
		public string? Status { get; set; }
		public int CurrentParticipants { get; set; } = 0;
		public int OrganizerId { get; set; }
		public string EventPlatform { get; set; } = "Private";
		public Organizer? Organizer { get; set; }

	
		public List<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
	}
}
