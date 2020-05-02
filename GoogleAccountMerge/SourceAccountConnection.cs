using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Google.Contacts;
using Google.GData.Client;
using Google.Apis.Auth.OAuth2;
//using Google.Apis.Books.v1;
//using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.GData.Extensions;
using Google.GData.Contacts;

namespace GoogleAccountMerge
{
    class SourceAccountConnection
    {
        //[STAThread]
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Books API Sample: List MyLibrary");
        //    Console.WriteLine("================================");
        //    try
        //    {
        //        new BooksExample().Run().Wait();
        //    }
        //    catch (AggregateException ex)
        //    {
        //        foreach (var e in ex.InnerExceptions)
        //        {
        //            Console.WriteLine("ERROR: " + e.Message);
        //        }
        //    }
        //    Console.WriteLine("Press any key to continue...");
        //    Console.ReadKey();
        //}

        public async Task Run()
        {
            UserCredential credential;
            //string[] scopes = new string[] { "https://www.googleapis.com/auth/contacts.readonly" };
            string[] scopes = new string[] { 
                "https://www.google.com/m8/feeds/",
                "https://www.googleapis.com/auth/contacts.readonly"
            };
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user", CancellationToken.None, new FileDataStore("ContactsMergingToolOAuth_Source"));

                Console.WriteLine("Source is ready");
                //Directory.Delete("C:/Users/trais/AppData/Roaming/ContactsMergingToolOAuth", true);

                //UserCredential credential2 = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                //    GoogleClientSecrets.Load(stream).Secrets,
                //    scopes,
                //    "user", CancellationToken.None, new FileDataStore("ContactsMergingToolOAuth"));

                //UserCredential credential1 = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                //   GoogleClientSecrets.Load(stream).Secrets,
                //   scopes,
                //   "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary_second"));

                //var service = new PlusService(new BaseClientService.Initializer()
                //{
                //    HttpClientInitializer = credential,
                //    ApplicationName = "Google Plus Authentication Sample",
                //});

                //// dummy call to api to refresh the auth token if needed.
                //var refresh = service.People.Get("me").Execute();

                var res = await credential.GetAccessTokenForRequestAsync();

                // Translate the Oauth permissions to something the old client libray can read
                OAuth2Parameters parameters = new OAuth2Parameters();
                parameters.AccessToken = credential.Token.AccessToken;
                parameters.RefreshToken = credential.Token.RefreshToken;

                try
                {
                    RequestSettings settings = new RequestSettings("Google contacts tutorial", parameters);
                    ContactsRequest cr = new ContactsRequest(settings);
                    
                    ReadContacts(cr);
                    //CreateContact(cr);
                }
                catch (Exception a)
                {
                    Console.WriteLine("A Google Apps error occurred.");
                    Console.WriteLine();
                }

            }

            // Create the service.
            //var service = new BooksService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = "Books API Sample",
            //});

            //var bookshelves = await service.Mylibrary.Bookshelves.List().ExecuteAsync();
            
        }

        public static void ReadContacts(ContactsRequest cr)
        {
            Feed<Contact> f = cr.GetContacts();
            foreach (Contact c in f.Entries)
            {
                Console.WriteLine(c.ToString());
                if (c.Phonenumbers.Count > 0)
                {
                    Console.WriteLine(c.Name.FullName + "  " + c.Phonenumbers[0].Value);
                }
            }
        }

        public static Contact CreateContact(ContactsRequest cr)
        {
            Contact newEntry = new Contact();
            // Set the contact's name.
            newEntry.Name = new Name()
            {
                FullName = "Elizabeth Bennet",
                GivenName = "Elizabeth",
                FamilyName = "Bennet",
            };
            newEntry.Content = "Notes";
            // Set the contact's e-mail addresses.
            newEntry.Emails.Add(new EMail()
            {
                Primary = true,
                Rel = ContactsRelationships.IsHome,
                Address = "liz@gmail.com"
            });
            newEntry.Emails.Add(new EMail()
            {
                Rel = ContactsRelationships.IsWork,
                Address = "liz@example.com"
            });
            // Set the contact's phone numbers.
            newEntry.Phonenumbers.Add(new PhoneNumber()
            {
                Primary = true,
                Rel = ContactsRelationships.IsWork,
                Value = "(206)555-1212",
            });
            newEntry.Phonenumbers.Add(new PhoneNumber()
            {
                Rel = ContactsRelationships.IsHome,
                Value = "(206)555-1213",
            });
            // Set the contact's IM information.
            newEntry.IMs.Add(new IMAddress()
            {
                Primary = true,
                Rel = ContactsRelationships.IsHome,
                Protocol = ContactsProtocols.IsGoogleTalk,
            });
            // Set the contact's postal address.
            newEntry.PostalAddresses.Add(new StructuredPostalAddress()
            {
                Rel = ContactsRelationships.IsWork,
                Primary = true,
                Street = "1600 Amphitheatre Pkwy",
                City = "Mountain View",
                Region = "CA",
                Postcode = "94043",
                Country = "United States",
                FormattedAddress = "1600 Amphitheatre Pkwy Mountain View",
            });
            // Insert the contact.
            Uri feedUri = new Uri(ContactsQuery.CreateContactsUri("default"));
            Contact createdEntry = cr.Insert(feedUri, newEntry);
            Console.WriteLine("Contact's ID: " + createdEntry.Id);
            return createdEntry;
        }
    }
}
