using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PrivateWeb.Pages
{
	public class OrganizerDashboardModel : PageModel
	{
		
		public string UserName { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;
		public string UserRole { get; set; } = string.Empty;

		public void OnGet()
		{
			
			UserRole = HttpContext.Session.GetString("user_role") ?? "";
			UserId = HttpContext.Session.GetString("user_id") ?? "0";
			UserName = HttpContext.Session.GetString("user_name") ?? "Organizer";

			
			if (UserRole != "Organizer")
			{
				Response.Redirect("/Login");
				return;
			}
		}
	}
}