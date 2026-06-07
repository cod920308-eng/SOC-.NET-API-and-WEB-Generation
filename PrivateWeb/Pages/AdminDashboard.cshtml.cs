using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PrivateWeb.Pages
{
	public class AdminDashboardModel : PageModel
	{
		private readonly HttpClient _http;

		public AdminDashboardModel(IHttpClientFactory factory)
		{
			_http = factory.CreateClient();
		}

		[BindProperty] public string FullName { get; set; }
		[BindProperty] public string Email { get; set; }
		[BindProperty] public string Password { get; set; }
		[BindProperty] public string Confirm { get; set; }
		[BindProperty] public string Phone { get; set; }

		public string ErrorMessage { get; set; }
		public string SuccessMessage { get; set; }

		public string UserName { get; set; }
		public string UserRole { get; set; }

		public void OnGet()
		{
			UserName = HttpContext.Session.GetString("user_name");
			UserRole = HttpContext.Session.GetString("user_role");
		}

		public async Task<IActionResult> OnPostAsync()
		{
			UserRole = HttpContext.Session.GetString("user_role");

			// Security check
			if (UserRole != "Admin")
				return RedirectToPage("/Login");

			if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Email) ||
				string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Confirm))
			{
				ErrorMessage = "Please fill in all fields.";
				return Page();
			}

			if (Password.Length < 6)
			{
				ErrorMessage = "Password must be at least 6 characters.";
				return Page();
			}

			if (Password != Confirm)
			{
				ErrorMessage = "Passwords do not match.";
				return Page();
			}

			var payload = new
			{
				fullName = FullName,
				email = Email,
				password = Password,
				phone = Phone
			};

			var endpoint = "https://localhost:7096/api/participant/register";

			var res = await _http.PostAsJsonAsync(endpoint, payload);

			if (res.IsSuccessStatusCode)
			{
				SuccessMessage = "Citizen account created successfully!";
				return Page();
			}

			var err = await res.Content.ReadAsStringAsync();
			ErrorMessage = $"Failed: {err}";
			return Page();
		}
	}
}