using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.ViewModels.Models
{
	public class User
	{

        public User(string token, string fullName, string profilePicture)
        {
			AccessToken = token;
			FullName = fullName;
			ProfilePicture = profilePicture; 
        }

        public string AccessToken { get; set; }

		public string FullName { get; set; }

		public string ProfilePicture { get; set; }

	}
}
