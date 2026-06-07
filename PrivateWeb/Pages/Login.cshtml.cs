using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PrivateWeb.Pages
{
	public class LoginModel : PageModel
	{
		private readonly HttpClient _http;

		public LoginModel(IHttpClientFactory factory)
		{
			_http = factory.CreateClient();
		}

		[BindProperty] public string Email { get; set; }
		[BindProperty] public string Password { get; set; }
		[BindProperty] public string Role { get; set; }

		public string ErrorMessage { get; set; }

		public void OnGet() { }

		public async Task<IActionResult> OnPostAsync()
		{
			if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
			{
				ErrorMessage = "Please enter your email and password.";
				return Page();
			}

			
			if (Role == "Admin" && Email == "admin@private.com" && Password == "123")
			{
				HttpContext.Session.SetString("user_name", "Admin");
				HttpContext.Session.SetString("user_email", Email);
				HttpContext.Session.SetString("user_role", "Admin");
				return RedirectToPage("/AdminDashboard");
			}

			
			if (Role == "Organizer")
			{
				
				if (string.Equals(Email.Trim(), "aloka@gmail.com", StringComparison.OrdinalIgnoreCase)
					&& Password == "123456")
				{
					HttpContext.Session.SetString("user_name", "Aloka");
					HttpContext.Session.SetString("user_email", "aloka@gmail.com");
					HttpContext.Session.SetString("user_role", "Organizer");
					HttpContext.Session.SetString("user_id", "1");
					return RedirectToPage("/OrganizerDashboard");
				}

				
				var res = await _http.PostAsync(
					$"https://localhost:7096/api/organizer/login?email={Uri.EscapeDataString(Email.Trim())}&password={Uri.EscapeDataString(Password)}",
					null);

				if (res.IsSuccessStatusCode)
				{
					var json = await res.Content.ReadAsStringAsync();
					var user = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(json,
						new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					HttpContext.Session.SetString("user_name", user?.FullName ?? Email);
					HttpContext.Session.SetString("user_email", Email);
					HttpContext.Session.SetString("user_role", "Organizer");
					HttpContext.Session.SetString("user_id", user?.Id.ToString() ?? "0");
					return RedirectToPage("/OrganizerDashboard");
				}

				ErrorMessage = "Invalid email or password.";
				return Page();
			}

			
			if (Role == "Citizen")
			{
				var res = await _http.PostAsync(
					$"https://localhost:7096/api/participant/login?email={Uri.EscapeDataString(Email.Trim())}&password={Uri.EscapeDataString(Password)}",
					null);

				if (res.IsSuccessStatusCode)
				{
					var json = await res.Content.ReadAsStringAsync();
					var user = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(json,
						new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					HttpContext.Session.SetString("user_name", user?.FullName ?? Email);
					HttpContext.Session.SetString("user_email", Email);
					HttpContext.Session.SetString("user_role", "Citizen");
					HttpContext.Session.SetString("user_id", user?.Id.ToString() ?? "0");
					return RedirectToPage("/Index");
				}

				ErrorMessage = "Invalid email or password.";
				return Page();
			}

			ErrorMessage = "Please select a role.";
			return Page();
		}
	}

	public class UserResponse
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
	}
}