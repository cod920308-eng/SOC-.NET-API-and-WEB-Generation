using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KMC_Main_WEB.Pages
{
	public class AdminDashboardModel : PageModel
	{
		public string UserName { get; set; } = string.Empty;
		public string UserRole { get; set; } = string.Empty;

		public void OnGet()
		{
			UserRole = HttpContext.Session.GetString("user_role") ?? "";
			UserName = HttpContext.Session.GetString("user_name") ?? "Admin";

			if (UserRole != "Admin")
			{
				Response.Redirect("/Login");
				return;
			}
		}
	}
}