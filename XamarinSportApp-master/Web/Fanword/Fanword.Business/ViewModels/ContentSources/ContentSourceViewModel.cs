using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.ContentSources
{
	public class ContentSourceViewModel
	{
		public string Id { get; set; }
		[Required, Display(Name = "First Name")]
		public string FirstName { get; set; }
		[Required, Display(Name = "Last Name")]
		public string LastName { get; set; }
		[Required, Display(Name = "Content Source Name")]
		public string ContentSourceName { get; set; }
		[Required, Display(Name = "Website")]
		public string WebsiteLink { get; set; }
		[Display(Name = "Content Source Description"), Required]
		public string ContentSourceDescription { get; set; }
		[Display(Name = "Facebook Link")]
		public string FacebookLink { get; set; }
		[Display(Name = "Facebook Link Show")]
		public bool FacebookShow { get; set; }
		[Display(Name ="Twitter Link")]
		public string TwitterLink { get; set; }
		[Display(Name = "Twitter Link Show")]
		public bool TwitterShow { get; set; }
		[Display(Name = "Instagram Link")]
		public string InstagramLink { get; set; }
		[Display(Name = "Instagram Link Show")]
		public bool InstagramShow { get; set; }
		[Display(Name = "Action Button Text")]
		public string ActionText { get; set; }
		[Display(Name = "Action Button Link")]
		public string ActionLink { get; set; }
		public string LogoBlob { get; set; }
		public string LogoContainer { get; set; }
		[Required(ErrorMessage = "Logo is required")]
		public string LogoUrl { get; set; }
		[Display(Name = "Is Approved"), Required]
		public bool IsApproved { get; set; }
		public string PrimaryColor { get; set; }
	}
}
