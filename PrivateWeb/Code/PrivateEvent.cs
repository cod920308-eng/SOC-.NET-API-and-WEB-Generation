namespace PrivateWeb.Code
{
	public class PrivateEvent
	{
		public int id { get; set; }
		public string? title { get; set; }
		public string? description { get; set; }
		public string? type { get; set; }
		public string? location { get; set; }
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
		public int capacity { get; set; }
		public string? eventImage { get; set; }
		public string? status { get; set; }
		public int currentParticipants { get; set; }
		public int organizerId { get; set; }
		public string? eventPlatform { get; set; }
		public string? organizerName { get; set; }
		public PrivateEventOrganizer? organizer { get; set; }
	}

	public class PrivateEventOrganizer
	{
		public int id { get; set; }
		public string? fullName { get; set; }
		public string? organizationName { get; set; }
	}

	public class PrivateEventRegistration
	{
		public int id { get; set; }
		public int eventId { get; set; }
		public int participantId { get; set; }
		public DateTime registeredAt { get; set; }
	}
}