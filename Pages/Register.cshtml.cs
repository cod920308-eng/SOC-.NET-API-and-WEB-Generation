using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KMC_Main_WEB.Pages
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
		[BindProperty] public string Role { get; set; }
		[BindProperty] public string PhoneNumber { get; set; }
		[BindProperty] public string OrganizationName { get; set; }
		public string ErrorMessage { get; set; }
		public string SuccessMessage { get; set; }

		public void OnGet() { }

		public async Task<IActionResult> OnPostAsync()
		{
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

			object payload;
			string endpoint;

			if (Role == "Organizer")
			{
				payload = new
				{
					fullName = FullName,
					email = Email,
					password = Password,
					phone = PhoneNumber,
					organizationName = OrganizationName
				};
				endpoint = "https://localhost:7096/api/organizer/register";
			}
			else
			{
				payload = new
				{
					fullName = FullName,
					email = Email,
					password = Password,
					phone = PhoneNumber
				};
				endpoint = "https://localhost:7096/api/participant/register";
			}

			var res = await _http.PostAsJsonAsync(endpoint, payload);

			if (res.IsSuccessStatusCode)
			{
				SuccessMessage = Role + " " +  FullName + " Registration Successful!";
				ModelState.Clear(); 
				return Page();
			}

			ErrorMessage = "Registration failed. Email may already be registered.";
			return Page();
		}
	}
}