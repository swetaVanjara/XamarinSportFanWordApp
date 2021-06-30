using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Shared.Models
{
    public class MenuItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }

        public MenuItem(string id, string title, string icon)
        {
            Id = id;
            Title = title;
            Icon = icon;
           
        }

        public MenuItem(string title)
        {
            Title = title;
        }

        public static List<MenuItem> GetMenuItems()
        {
            
            List<MenuItem> items = new List<MenuItem>();
            items.Add(new MenuItem("GENERAL"));
            items.Add(new MenuItem("Home", "Home", "IconHome"));
            items.Add(new MenuItem("Profile", "Profile", "IconUser"));
            //items.Add(new MenuItem("BecomeAdvertiser", "Become an Advertiser", "IconAdvertiser"));
            //items.Add(new MenuItem("CreateContentSource", "Create a Content Source", "IconContentSource"));

            items.Add(new MenuItem("FOLLOW"));
            items.Add(new MenuItem("Teams", "Teams", "IconTeams"));
            items.Add(new MenuItem("Schools", "Schools", "IconSchools"));
            items.Add(new MenuItem("Sports", "Sports", "IconSports"));
            items.Add(new MenuItem("ContentSources", "Content Sources", "IconContentSource"));
            items.Add(new MenuItem("Users", "Users", "IconUser"));

            items.Add(new MenuItem("SETTINGS"));
            items.Add(new MenuItem("Settings", "Settings", "IconSettings"));
            items.Add(new MenuItem("Logout", "Logout", "IconLogout"));
            items.Add(new MenuItem(""));
            return items;
        }
    }
}
