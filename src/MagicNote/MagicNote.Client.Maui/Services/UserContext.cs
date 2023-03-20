using System;

namespace MagicNote.Client.Maui.Services
{
    public class UserContext
    {
        public string Name { get;  set; }
        public string UserIdentifier { get;  set; }
        public bool IsLoggedOn { get;  set; }
        public string GivenName { get;  set; }
        public string FamilyName { get;  set; }
        public string Province { get;  set; }
        public string PostalCode { get;  set; }
        public string Country { get;  set; }
        public string EmailAddress { get;  set; }
        public string JobTitle { get;  set; }
        public string StreetAddress { get;  set; }
        public string City { get;  set; }
        public string AccessToken { get;  set; }
        public bool IsAccountant { get; set; }
        public bool IsClient { get; set; }
        public string ProfilePicture { get; set; }
        public string DefaultLangauge { get; set; }
        public DateTime LoggedInDate { get; set; }
        public string Role { get; set; }
    }
}
