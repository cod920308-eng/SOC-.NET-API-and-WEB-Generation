namespace KmcAPI.DTO
{
	public class EventRegistrationWriteDTO
	{
		public int EventId { get; set; }
		public int ParticipantId { get; set; }
	}

	public class EventRegistrationReadDTO
	{
		public int Id { get; set; }
		public DateTime RegisteredAt { get; set; }
		public string? EventTitle { get; set; }
		public string? EventLocation { get; set; }
		public DateTime EventStartDate { get; set; }
		public string? ParticipantName { get; set; }
		public string? ParticipantEmail { get; set; }
	}
}