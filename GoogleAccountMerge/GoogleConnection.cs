using System;
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
using PhoneNumber = Google.GData.Extensions.PhoneNumber;

namespace GoogleAccountMerge
{
    public delegate void OnConnectionSuccessDelegate(string connectionName, string gmail);
    public delegate void OnConnectionFailDelegate(string connectionName);
    public delegate void OnDisconnectedDelegate(string connectionName);
    public delegate void OnReadContactsReadyDelegate(string connectionName, List<Contact> contacts);
    public delegate void OnContactsMergingSuccessDelegate(string connectionName, int successAlterations, int failAlterations);
    public delegate void OnContactsMergingFailDelegate(string connectionName);
    public delegate void OnTaskFailDelegate(string connectionName);

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

        // Thread
        bool KeepAlive = false;
        List<GoogleConnectionTask> TaskQueue = new List<GoogleConnectionTask>();

        // Google
        UserCredential UserCredential;
        OAuth2Parameters OAuth2Parameters;
        ContactsRequest ContactsRequest;

        // UI
        public List<Contact> LastContactsList = new List<Contact>();
        string CurrentUserEmail;

        //Delegates
        OnConnectionSuccessDelegate OnConnectionSuccessDelegateInstance;
        OnConnectionFailDelegate OnConnectionFailDelegateInstance;
        OnDisconnectedDelegate OnDisconnectedDelegateInstance;
        OnReadContactsReadyDelegate OnReadContactsReadyDelegateInstance;
        OnContactsMergingSuccessDelegate OnContactsMergingSuccessDelegateInstance;
        OnContactsMergingFailDelegate OnContactsMergingFailDelegateInstance;
        OnTaskFailDelegate OnTaskFailDelegateInstance;

        public GoogleConnection(
            string connectionName,
            OnConnectionSuccessDelegate onConnectionSuccessDelegateInstance,
            OnConnectionFailDelegate onConnectionFailDelegateInstance,
            OnDisconnectedDelegate onDisconnectedDelegateInstance,
            OnReadContactsReadyDelegate onReadContactsReadyDelegateInstance,
            OnContactsMergingSuccessDelegate onContactsMergingSuccessDelegateInstance,
            OnContactsMergingFailDelegate onContactsMergingFailDelegateInstance,
            OnTaskFailDelegate onTaskFailDelegateInstance
            )
        {
            ConnectionName = connectionName;
            OnConnectionSuccessDelegateInstance = onConnectionSuccessDelegateInstance;
            OnConnectionFailDelegateInstance = onConnectionFailDelegateInstance;
            OnDisconnectedDelegateInstance = onDisconnectedDelegateInstance;
            OnReadContactsReadyDelegateInstance = onReadContactsReadyDelegateInstance;
            OnContactsMergingSuccessDelegateInstance = onContactsMergingSuccessDelegateInstance;
            OnContactsMergingFailDelegateInstance = onContactsMergingFailDelegateInstance;
            OnTaskFailDelegateInstance = onTaskFailDelegateInstance;
        }

        public void AddTask(GoogleConnectionTask task)
        {
            if (IsConnected())
            {
                TaskQueue.Add(task); 
            }
            else
            {
                OnTaskFailDelegateInstance.Invoke(ConnectionName);
            }
        }

        public void InitReadContacts()
        {
            AddTask(new GoogleConnectionTask_ReadContacts());
        }

        public void InitMergeContacts(List<Contact> sourceContacts)
        {
            GoogleConnectionTask_MergeContacts task = new GoogleConnectionTask_MergeContacts();
            // duplicare deoarece lista va fi folosta in alt thread. 
            // eventual sa se folsoeasca un tip de lista ThreadSafe.
            task.SourceContacts = new List<Contact>(sourceContacts);
            AddTask(task);
        }

        public void Connect()
        {
            if (IsConnected()) return;

            try
            {
                KeepAlive = true;
                TaskQueue.Clear();
                Run();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
                KeepAlive = false;
                OnConnectionFailDelegateInstance(this.ConnectionName + ex.Message);
            }
        }

        public void Disconnect()
        {
            if (IsConnected())
            {
                KeepAlive = false;
                try
                {
                    string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    Directory.Delete(appDataPath + "/ContactsMergingToolOAuth" + ConnectionName, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to remove Google cached connection file. Reason: " + e.Message);
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
                CurrentUserEmail = profile.EmailAddresses.FirstOrDefault().Value;

                //Notify that connection is ready
                //Notify that connection is ready
                OnConnectionSuccessDelegateInstance.Invoke(this.ConnectionName, CurrentUserEmail);

                // Translate the Oauth permissions to something the old client libray can read
                OAuth2Parameters = new OAuth2Parameters();
                OAuth2Parameters.AccessToken = UserCredential.Token.AccessToken;
                OAuth2Parameters.RefreshToken = UserCredential.Token.RefreshToken;

                //Init contact request
                RequestSettings settings = new RequestSettings("Google contacts tutorial", OAuth2Parameters);
                settings.Maximum = 10000000; //default is 25
                settings.PageSize = 10000000;
                ContactsRequest = new ContactsRequest(settings);

                //Read Contacts
                InitReadContacts();

                try
                {
                    while(KeepAlive)
                    {
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
                    OnTaskFailDelegateInstance("Error: " + a.Message);
                    Console.WriteLine("A Google Apps error occurred: " + a.Message);
                }

            }
        }

        private void ProcessGoogleConnectionTask(GoogleConnectionTask task)
        {
          
            if (task is GoogleConnectionTask_ReadContacts)
            {
                LastContactsList = ReadContacts(ContactsRequest);
                OnReadContactsReadyDelegateInstance(this.ConnectionName, LastContactsList);
            }
            else if (task is GoogleConnectionTask_MergeContacts) 
            {
                Console.WriteLine("Task::MergeContacts");
                int successAlterations = 0;
                int failAlterations = 0;
                bool result = MergeContacts(((GoogleConnectionTask_MergeContacts) task).SourceContacts, ref successAlterations, ref failAlterations);
                if (result)
                {
                    OnContactsMergingSuccessDelegateInstance(this.ConnectionName, successAlterations, failAlterations);
                }
                else
                {
                    OnContactsMergingFailDelegateInstance(this.ConnectionName);
                }
            }
        }

        private List<Contact> ReadContacts(ContactsRequest cr)
        {
            Feed<Group> groups = cr.GetGroups("default"); //
            Feed<Contact> googleContacts = cr.GetContacts("default"); //"default"
            // Nu sunt sigur daca cr.GetContacts() sunt incarcate la acest moment
            // e posibil sa fie incarcate separat la iterarea foreach, de asta le pun intr-o alta lista

            List<Contact> processedContacts = new List<Contact>();

            int totalProcessedContacts = 0;

            foreach (Contact c in googleContacts.Entries)
            {
                totalProcessedContacts++;
                if(c.GroupMembership.Count == 0)
                {
                    //daca contactul nu are nici un grup inseamna ca nu apartine grupului default si apare in "Other contacts"
                    //in aceasta lista sunt adresele cu care s-a interactionat de pe cont, 
                    //si nu apar by default in lista de pe telefoane
                    Console.WriteLine(c.Name.FullName + "Grup lipsa - ignorare");
                    continue;
                }

                if (c.Phonenumbers.Count == 0)
                {
                    //zombie contact, ignore
                    Console.WriteLine(c.Name.FullName + "Numar lipsa - ignorare");
                    continue;
                }
                processedContacts.Add(c);            
            }

            Console.WriteLine("Numarul total de contacte procesate: " + totalProcessedContacts);

            return processedContacts;
        }

        bool MergeContacts(List<Contact> sourceContacts, ref int successAlterations, ref int failAlterations)
        {
            List<Contact> newTargtContacts = new List<Contact>(sourceContacts);
            successAlterations = 0;
            failAlterations = 0;
            // Prima data cautam grupul in care sa adaugam contactele, 
            // altfel google nu le pune in lista principala de contacte 
            GroupMembership groupMembership = null;
            foreach (Contact existingContact in LastContactsList)
            {
                if(existingContact.GroupMembership.Count > 0)
                {
                    groupMembership = new GroupMembership()
                    {
                        HRef = existingContact.GroupMembership[0].HRef
                    };
                    break;
                }
            }

            if(groupMembership == null)
            {
                groupMembership = new GroupMembership()
                {
                    HRef = "http://www.google.com/m8/feeds/groups/" + CurrentUserEmail  + "/base/6"
                };
            }

            foreach (Contact sourceContact in sourceContacts)
            {
                if (sourceContact.Phonenumbers.Count == 0)
                {
                    continue;
                }

                bool addContact = true;

                foreach (Contact targetContact in LastContactsList)
                {
                    if (targetContact.Phonenumbers.Count == 0)
                    {
                        continue;
                    }

                    if (Utils.ContactsAreEqual(sourceContact, targetContact))
                    {
                        // Update existing contact
                        addContact = false;
                        bool updateWasNeeded = false;
                        if(UpdateTargetContactFromSource(targetContact, sourceContact, ref updateWasNeeded))
                        {
                            if(updateWasNeeded)
                            {
                                successAlterations++;
                            }
                        }
                        else
                        {
                            failAlterations++;
                        }
                        continue;
                    }
                }

                if (addContact)
                {
                    // Insert new contact
                    Contact clonedContact = Utils.CloneGoogleContact(sourceContact);
                    clonedContact.GroupMembership.Add(groupMembership);
                    if (InsertGoogleContact(clonedContact))
                    {
                        successAlterations++;
                    }
                    else
                    {
                        failAlterations++;
                    }
                }
            }

            return true;
        }
        private bool InsertGoogleContact(Contact newContact)
        {
            try
            {
                Uri feedUri = new Uri(ContactsQuery.CreateContactsUri("default"));
                //Uri feedUri = new Uri("https://www.google.com/m8/feeds/profiles/domain/" + test + "/full");
                Contact createdEntry = ContactsRequest.Insert(feedUri, newContact);
                return true;
            } 
            catch (Exception e)
            {
                Console.WriteLine("Failed to insert contact: " + e.Message);
                return false;
            }
        }

        public bool OverrideTargetContactProperties = true;
        private bool UpdateTargetContactFromSource(Contact targetContact, Contact sourceContact, ref bool updateWasNeeded)
        {
            try
            {
                bool needsUpdate = false;

                if (OverrideTargetContactProperties)
                {
                    bool diffsFound = false;

                    targetContact.Name.FullName = Utils.ContactStringAttributesDiff(sourceContact.Name.FullName, targetContact.Name.FullName, ref diffsFound);
                    needsUpdate = diffsFound ? true : needsUpdate;

                    targetContact.Name.GivenName = Utils.ContactStringAttributesDiff(sourceContact.Name.GivenName, targetContact.Name.GivenName, ref diffsFound);
                    needsUpdate = diffsFound ? true : needsUpdate;

                    targetContact.Name.FamilyName = Utils.ContactStringAttributesDiff(sourceContact.Name.FamilyName, targetContact.Name.FamilyName, ref diffsFound);
                    needsUpdate = diffsFound ? true : needsUpdate;

                    targetContact.Content = Utils.ContactStringAttributesDiff(sourceContact.Content, targetContact.Content, ref diffsFound);
                    needsUpdate = diffsFound ? true : needsUpdate;

                    diffsFound = false;
                    ExtensionCollection<EMail> newEmails = Utils.ContactEmailsDiff(sourceContact.Emails, targetContact.Emails, ref diffsFound);
                    if (diffsFound)
                    {
                        needsUpdate = true;
                        targetContact.Emails.Clear();
                        foreach (EMail item in newEmails)
                        {
                            targetContact.Emails.Add(item);
                        }
                    }

                    diffsFound = false;
                    ExtensionCollection<PhoneNumber> newNumbers = Utils.ContactPhoneNumbersDiff(sourceContact.Phonenumbers, targetContact.Phonenumbers, ref diffsFound);
                    if (diffsFound)
                    {
                        needsUpdate = true;
                        targetContact.Phonenumbers.Clear();
                        foreach (PhoneNumber item in newNumbers)
                        {
                            targetContact.Phonenumbers.Add(item);
                        }
                    }
                }
                else
                {
                    //TO DO
                }

                if (needsUpdate)
                {
                    ContactsRequest.Update(targetContact);
                    updateWasNeeded = true;
                }
                else
                {
                    updateWasNeeded = false; 
                }

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
  
    }
}
