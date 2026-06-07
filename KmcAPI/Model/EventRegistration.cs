using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KmcAPI.Model
{
	public class EventRegistration
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public DateTime RegisteredAt { get; set; } = DateTime.Now;

		
		public int EventId { get; set; }
		public Event? Event { get; set; }

		public int ParticipantId { get; set; }
		public Participant? Participant { get; set; }
	}
}