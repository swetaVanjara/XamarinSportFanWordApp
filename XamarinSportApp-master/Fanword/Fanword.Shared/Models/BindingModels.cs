using System;
using Fanword.Poco.Models;
namespace Fanword.Shared
{
	public class LoginModel
	{
		public string username { get; set; }
		public string password { get; set; }
		public string grant_type { get; set; }

		public LoginModel (string username, string password)
		{
			this.username = username;
			this.password = password;
			grant_type = "password";
		}

	}

	public class RegisterTwitterModel
	{
		public string TwitterToken { get; set; }
		public string TwitterSecret { get; set; }
	}

	public class AccessTokenResponse
	{
		public string access_token { get; set; }
		public string token_type { get; set; }
		public string expires_in { get; set; }
		public string refresh_token { get; set; }
		public string user { get; set; }
	}

	public class AddExternalLoginBindingModel
	{
		public string ExternalAccessToken { get; set; }
	}

	public class ChangePasswordBindingModel
	{
		public string OldPassword { get; set; }

		public string NewPassword { get; set; }

		public string ConfirmPassword { get; set; }
	}

	public class RegisterBindingModel
	{
		public string Email { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

	}

	public class RegisterExternalBindingModel
	{
		public string AccessToken { get; set; }
	}

	public class RemoveLoginBindingModel
	{

		public string LoginProvider { get; set; }

		public string ProviderKey { get; set; }
	}

	public class SetPasswordBindingModel
	{

		public string NewPassword { get; set; }

		public string ConfirmPassword { get; set; }
	}

	public class ForgotPasswordViewModel
	{
		//[Required]
		//[Display (Name = "Email")]
		public string Email { get; set; }
	}

	public class ExternalLoginViewModel
	{

		public string Url { get; set; }

		public string State { get; set; }
	}

	public class AzureFileData
	{
		public string BlobName { get; set; }
		public string FriendlyFileName { get; set; }
		public string Url { get; set; }
		public FileSizeType FileSizeType { get; set; }

		public AzureFileData (string blob, string friendlyName, string azureUrl, FileSizeType type)
		{
			BlobName = blob;
			FriendlyFileName = friendlyName;
			Url = azureUrl;
			FileSizeType = type;
		}
	}

	public enum FileSizeType
	{
		Mobile,
		Web,
		FullSize,
		NonImage,
		Video
	}
}
