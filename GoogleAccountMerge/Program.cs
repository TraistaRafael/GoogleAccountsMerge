/*
 Traista Rafael - 2020
*/

using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Contacts;
using Google.GData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleAccountMerge
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            return;
            Console.WriteLine("Connect to Source Google Account");
            Console.WriteLine("================================");
            try
            {
                new SourceAccountConnection().Run();
                new TargetAccountConnection().Run();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            //auth();
        }

        public static void auth()
        {

            string clientId = "682405030593-ufeqalha997c4d8qthpofslk2b2jp90g.apps.googleusercontent.com";
            string clientSecret = "0oXKyOyEr422mF284w5hQ-Ls";


            string[] scopes = new string[] { "https://www.googleapis.com/auth/contacts.readonly" };     // view your basic profile info.
            try
            {
                // Use the current Google .net client library to get the Oauth2 stuff.
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , "test"
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore("test")).Result;

                Console.WriteLine("Authorization is ready");

                // Translate the Oauth permissions to something the old client libray can read
                OAuth2Parameters parameters = new OAuth2Parameters();
                parameters.AccessToken = credential.Token.AccessToken;
                parameters.RefreshToken = credential.Token.RefreshToken;

                try
                {
                    RequestSettings settings = new RequestSettings("Google contacts tutorial", parameters);
                    ContactsRequest cr = new ContactsRequest(settings);
                    Feed<Contact> f = cr.GetContacts();
                    foreach (Contact c in f.Entries)
                    {
                        Console.WriteLine(c.Name.FullName);
                    }
                }
                catch (Exception a)
                {
                    Console.WriteLine("A Google Apps error occurred.");
                    Console.WriteLine();
                }

                // Translate the Oauth permissions to something the old client libray can read
                //OAuth2Parameters parameters = new OAuth2Parameters();
                //parameters.AccessToken = credential.Token.AccessToken;
                //parameters.RefreshToken = credential.Token.RefreshToken;
                //RunContactsSample(parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
