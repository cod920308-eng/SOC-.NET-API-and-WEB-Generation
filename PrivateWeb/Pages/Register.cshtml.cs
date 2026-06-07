using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PrivateWeb.Pages
{
	public class RegisterModel : PageModel
	{
		private readonly HttpClient _http;

		public RegisterModel(IHttpClientFactory factory)
		{
			_http = factory.CreateClient();
		}

		[BindProperty] public string FullName { get; set; }
		[BindProperty] public string Email { get; set; }
		[BindProperty] public string Password { get; set; }
		[BindProperty] public string Confirm { get; set; }
		[BindProperty] public string PhoneNumber { get; set; }

		public string ErrorMessage { get; set; }
		public string SuccessMessage { get; set; }

		public void OnGet() { }

		public async Task<IActionResult> OnPostAsync()
		{
			
			string role = "Citizen";

			if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Email) ||
				string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Confirm) ||
				string.IsNullOrEmpty(PhoneNumber))
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
				phoneNumber = PhoneNumber
			};

			var res = await _http.PostAsJsonAsync(
				"https://localhost:7096/api/participant/register", payload);

			if (res.IsSuccessStatusCode)
			{
				SuccessMessage = "Account created! You can now sign in.";
				return Page();
			}

			ErrorMessage = "Registration failed. Email may already be registered.";
			return Page();
		}
	}
}