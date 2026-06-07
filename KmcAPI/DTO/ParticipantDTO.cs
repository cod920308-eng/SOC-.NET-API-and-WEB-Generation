namespace KmcAPI.DTO
{
	public class ParticipantWriteDTO
	{
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Password { get; set; }
	}

	public class ParticipantReadDTO
	{
		public int Id { get; set; }
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		
	}
}