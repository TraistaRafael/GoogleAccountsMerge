﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Collections.Concurrent;
using Google.Apis.People.v1;
using Google.Apis.People.v1.Data;

namespace GoogleAccountMerge
{
    public delegate void OnConnectionSuccessDelegate(string connectionName, string gmail);
    public delegate void OnConnectionFailDelegate(string connectionName);
    public delegate void OnDisconnectedDelegate(string connectionName);
    public delegate void OnReadContactsReadyDelegate(string connectionName, List<Contact> contacts);
    public delegate void OnContactsMergingSuccessDelegate(string connectionName);
    public delegate void OnContactsMergingFailDelegate(string connectionName);

    class GoogleConnectionTask
    {

    }

    class GoogleConnectionTask_MergeContacts : GoogleConnectionTask
    {
        public List<Contact> SourceContacts;
    }

    class GoogleConnectionTask_ReadContacts : GoogleConnectionTask
    {

    }

    class GoogleConnection
    {
        private static int THREAD_SLEEP_MILLIS = 500;
        string ConnectionName { get; set; }

        // Thread utils
        bool KeepAlive = false;
        List<GoogleConnectionTask> TaskQueue = new List<GoogleConnectionTask>();

        // Google utils
        UserCredential UserCredential;
        OAuth2Parameters OAuth2Parameters;
        ContactsRequest contactsRequest;

        //Delegates
        OnConnectionSuccessDelegate OnConnectionSuccessDelegateInstance;
        OnConnectionFailDelegate OnConnectionFailDelegateInstance;
        OnDisconnectedDelegate OnDisconnectedDelegateInstance;
        OnReadContactsReadyDelegate OnReadContactsReadyDelegateInstance;
        OnContactsMergingSuccessDelegate OnContactsMergingSuccessDelegateInstance;
        OnContactsMergingFailDelegate OnContactsMergingFailDelegateInstance;

        public GoogleConnection(
            string connectionName,
            OnConnectionSuccessDelegate onConnectionSuccessDelegateInstance,
            OnConnectionFailDelegate onConnectionFailDelegateInstance,
            OnDisconnectedDelegate onDisconnectedDelegateInstance,
            OnReadContactsReadyDelegate onReadContactsReadyDelegateInstance,
            OnContactsMergingSuccessDelegate onContactsMergingSuccessDelegateInstance,
            OnContactsMergingFailDelegate onContactsMergingFailDelegateInstance
            )
        {
            ConnectionName = connectionName;
            OnConnectionSuccessDelegateInstance = onConnectionSuccessDelegateInstance;
            OnConnectionFailDelegateInstance = onConnectionFailDelegateInstance;
            OnDisconnectedDelegateInstance = onDisconnectedDelegateInstance;
            OnReadContactsReadyDelegateInstance = onReadContactsReadyDelegateInstance;
            OnContactsMergingSuccessDelegateInstance = onContactsMergingSuccessDelegateInstance;
            OnContactsMergingFailDelegateInstance = onContactsMergingFailDelegateInstance;
        }

        public void AddTask(GoogleConnectionTask task)
        {
            TaskQueue.Add(task);    
        }

        public void InitReadContacts()
        {

        }

        public void InitMergeContacts(List<Contact> sourceContacts)
        {

        }

        public void Connect()
        {
            if (IsConnected()) return;

            try
            {
                KeepAlive = true;
                TaskQueue.Clear();
                Console.WriteLine("Before call");
                //Task longRunningTask = Run();
                Run();
                Console.WriteLine("After call");
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
                KeepAlive = false;
                OnConnectionFailDelegateInstance(this.ConnectionName);
            }
        }

        public void Disconnect()
        {
            if (IsConnected())
            {
                KeepAlive = false;
                try
                {
                    Directory.Delete("C:/Users/trais/AppData/Roaming/ContactsMergingToolOAuth" + ConnectionName, true);
                }
                catch (Exception e)
                {
                    //Continua, probabil ca folderul nu a fost creat inca de catre o instanta anterioara
                }
                OnDisconnectedDelegateInstance(this.ConnectionName);
            }
        }

        public bool IsConnected()
        {
            return KeepAlive;
        }

        private async Task Run()
        {
            string[] scopes = new string[] {
                "https://www.google.com/m8/feeds/",
                "https://www.googleapis.com/auth/contacts.readonly",
                "profile",
                "email"
            };
            //"https://www.googleapis.com/auth/user.addresses.read",
            //    "https://www.googleapis.com/auth/gmail.readonly",
            //    "https://www.googleapis.com/auth/plus.profile.emails.read",
            //    "https://mail.google.com/",
            //    "https://www.googleapis.com/auth/user.emails.read",
            //"https://www.googleapis.com/auth/user.emails.read",
            //"https://www.googleapis.com/auth/plus.profile.emails.read",
            //"https://mail.google.com/",

            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                UserCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user", CancellationToken.None, new FileDataStore("ContactsMergingToolOAuth" + ConnectionName));

                    PeopleService peopleService = new PeopleService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = UserCredential,
                        ApplicationName = "contactsmergingtool",
                    });

                //Read email address
                PeopleResource.GetRequest peopleRequest = peopleService.People.Get("people/me");
                peopleRequest.RequestMaskIncludeField = "person.names,person.emailAddresses";
                Person profile = peopleRequest.Execute();
                string useremail = profile.EmailAddresses.FirstOrDefault().Value;

                //Notify that connection is ready
                OnConnectionSuccessDelegateInstance.Invoke(this.ConnectionName, useremail);

                // Translate the Oauth permissions to something the old client libray can read
                OAuth2Parameters = new OAuth2Parameters();
                OAuth2Parameters.AccessToken = UserCredential.Token.AccessToken;
                OAuth2Parameters.RefreshToken = UserCredential.Token.RefreshToken;

                //Init contact request
                RequestSettings settings = new RequestSettings("Google contacts tutorial", OAuth2Parameters);
                contactsRequest = new ContactsRequest(settings);

                try
                {
                    while(KeepAlive)
                    {
                        Console.WriteLine("While is alive");
                        //Thread.Sleep(THREAD_SLEEP_MILLIS);
                        await Task.Delay(THREAD_SLEEP_MILLIS);
                        if (TaskQueue.Count > 0)
                        {
                            ProcessGoogleConnectionTask(TaskQueue.Last());
                            TaskQueue.Remove(TaskQueue.Last());
                        }
                    }
                }
                catch (Exception a)
                {
                    Console.WriteLine("A Google Apps error occurred.");
                }

            }
        }

        private void ProcessGoogleConnectionTask(GoogleConnectionTask task)
        {
          
            if (task is GoogleConnectionTask_ReadContacts)
            {
                Console.WriteLine("Task::ReadContacts");
                List<Contact> contacts = ReadContacts(contactsRequest);
                OnReadContactsReadyDelegateInstance(this.ConnectionName, contacts);
                //OnReadContactsReady(contacts);
            }
            else if (task is GoogleConnectionTask_MergeContacts) 
            {
                Console.WriteLine("Task::MergeContacts");
                bool result = MergeContacts(((GoogleConnectionTask_MergeContacts) task).SourceContacts);
                if (result)
                {
                    OnContactsMergingSuccessDelegateInstance(this.ConnectionName);
                }
                else
                {
                    OnContactsMergingFailDelegateInstance(this.ConnectionName);
                }
            }
        }

        private List<Contact> ReadContacts(ContactsRequest cr)
        {
            Feed<Contact> f = cr.GetContacts();
            // Nu sunt sigut daca cr.GetContacts() sunt incarcate la acest moment
            // e posibil sa fie incarcate separat la iterarea foreach, de asta le pun intr-o alta lista

            List<Contact> contacts = new List<Contact>();
            foreach (Contact c in f.Entries)
            {
                if (c.Phonenumbers.Count > 0)
                {
                    contacts.Add(c);
                    //Console.WriteLine(c.Name.FullName + "  " + c.Phonenumbers[0].Value);
                }
            }

            return contacts;
        }

        bool MergeContacts(List<Contact> sourceContacts)
        {
            return true;

            //TODO
        }

        public static Contact CreateContact(ContactsRequest cr)
        {
            return null;
            //Contact newEntry = new Contact();
            //// Set the contact's name.
            //newEntry.Name = new Name()
            //{
            //    FullName = "Elizabeth Bennet",
            //    GivenName = "Elizabeth",
            //    FamilyName = "Bennet",
            //};
            //newEntry.Content = "Notes";
            //// Set the contact's e-mail addresses.
            //newEntry.Emails.Add(new EMail()
            //{
            //    Primary = true,
            //    Rel = ContactsRelationships.IsHome,
            //    Address = "liz@gmail.com"
            //});
            //newEntry.Emails.Add(new EMail()
            //{
            //    Rel = ContactsRelationships.IsWork,
            //    Address = "liz@example.com"
            //});
            //// Set the contact's phone numbers.
            //newEntry.Phonenumbers.Add(new PhoneNumber()
            //{
            //    Primary = true,
            //    Rel = ContactsRelationships.IsWork,
            //    Value = "(206)555-1212",
            //});
            //newEntry.Phonenumbers.Add(new PhoneNumber()
            //{
            //    Rel = ContactsRelationships.IsHome,
            //    Value = "(206)555-1213",
            //});
            //// Set the contact's IM information.
            //newEntry.IMs.Add(new IMAddress()
            //{
            //    Primary = true,
            //    Rel = ContactsRelationships.IsHome,
            //    Protocol = ContactsProtocols.IsGoogleTalk,
            //});
            //// Set the contact's postal address.
            //newEntry.PostalAddresses.Add(new StructuredPostalAddress()
            //{
            //    Rel = ContactsRelationships.IsWork,
            //    Primary = true,
            //    Street = "1600 Amphitheatre Pkwy",
            //    City = "Mountain View",
            //    Region = "CA",
            //    Postcode = "94043",
            //    Country = "United States",
            //    FormattedAddress = "1600 Amphitheatre Pkwy Mountain View",
            //});
            //// Insert the contact.
            //Uri feedUri = new Uri(ContactsQuery.CreateContactsUri("default"));
            //Contact createdEntry = cr.Insert(feedUri, newEntry);
            //Console.WriteLine("Contact's ID: " + createdEntry.Id);
            //return createdEntry;
        }
    }
}