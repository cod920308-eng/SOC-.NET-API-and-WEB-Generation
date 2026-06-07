namespace KmcAPI.DTO
{


	public class EventWriteDTO
	{
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Type { get; set; }
		public string? Location { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int Capacity { get; set; }
		public string? EventImage { get; set; }
		public string? Status { get; set; }
		public int OrganizerId { get; set; }
		
	}
	public class EventReadDTO
	{
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
		public int CurrentParticipants { get; set; }
		public int OrganizerId { get; set; }
		public string? OrganizerName { get; set; } 
	}
}
