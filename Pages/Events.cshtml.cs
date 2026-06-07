using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using KMC_Main_WEB.Code;

namespace KMC_Main_WEB.Pages
{
	public class EventsModel : PageModel
	{
		private const string ApiBase = "https://localhost:7096";

		public List<KmcMainEvent> Events { get; set; } = new List<KmcMainEvent>();
		public HashSet<int> RegisteredEventIds { get; set; } = new HashSet<int>();
		public string StatusMessage { get; set; } = "";
		public bool IsSuccess { get; set; }
		public string UserRole { get; set; } = "";
		public string UserId { get; set; } = "0";
		public string ErrorMessage { get; set; } = "";

		public async Task OnGetAsync()
		{
			UserRole = HttpContext.Session.GetString("user_role") ?? "";
			UserId = HttpContext.Session.GetString("user_id") ?? "0";

			HttpClient client = new HttpClient();

			
			try
			{
				HttpResponseMessage res = await client.GetAsync($"{ApiBase}/api/event/public/kmc");
				if (res.IsSuccessStatusCode)
				{
					string data = await res.Content.ReadAsStringAsync();
					Events = JsonSerializer.Deserialize<List<KmcMainEvent>>(data,
						new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
						?? new List<KmcMainEvent>();
				}
				else
				{
					ErrorMessage = "Could not load events. Please try again later.";
				}
			}
			catch
			{
				ErrorMessage = "Could not load events. Please try again later.";
			}

			int participantId = int.TryParse(UserId, out var uid) ? uid : 0;
			if (participantId > 0 && UserRole == "Citizen")
			{
				try
				{
					HttpResponseMessage regRes = await client.GetAsync(
						$"{ApiBase}/api/EventRegistration/participant/{participantId}");

					if (regRes.IsSuccessStatusCode)
					{
						string regData = await regRes.Content.ReadAsStringAsync();
						var regs = JsonSerializer.Deserialize<List<KmcMainEventRegistration>>(regData,
							new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

						if (regs != null)
							RegisteredEventIds = regs.Select(r => r.eventId).ToHashSet();
					}
				}
				catch { }
			}
		}

		public async Task<IActionResult> OnPostRegisterAsync(int eventId)
		{
			UserRole = HttpContext.Session.GetString("user_role") ?? "";
			UserId = HttpContext.Session.GetString("user_id") ?? "0";

			int participantId = int.TryParse(UserId, out var uid) ? uid : 0;

			if (participantId == 0 || UserRole != "Citizen")
			{
				StatusMessage = "Please log in as a Citizen to register.";
				IsSuccess = false;
				await OnGetAsync();
				return Page();
			}

			try
			{
				var dto = new { eventId = eventId, participantId = participantId };
				HttpClient client = new HttpClient();
				string json = JsonSerializer.Serialize(dto);
				StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

				HttpResponseMessage res = await client.PostAsync(
					$"{ApiBase}/api/eventregistration", content);

				string responseText = await res.Content.ReadAsStringAsync();

				if (res.IsSuccessStatusCode)
				{
					StatusMessage = "Successfully Registered to the Event";
					IsSuccess = true;
				}
				else
				{
					StatusMessage = responseText.Contains("already", StringComparison.OrdinalIgnoreCase)
						? "⚠️ You're already registered for this event."
						: "Something went wrong. Please try again.";
					IsSuccess = false;
				}
			}
			catch
			{
				StatusMessage = "Could not connect to the server.";
				IsSuccess = false;
			}

			await OnGetAsync();
			return Page();
		}
	}
}