using Google.Contacts;
using Google.GData.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAccountMerge
{
    class Utils
    {

        public static string NormalizePhoneNumber(string input)
        {
            return input;
        }

        void ContactsAreEqual()
        {

        }
        public static bool PhoneNumbersAreEqual(string a, string b)
        {
            if (a.Equals(b) || a.Contains(b) || b.Contains(a))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ContactsAreEqual(Contact a, Contact b)
        {
            if (a.Phonenumbers.Count == 0 || b.Phonenumbers.Count == 0)
            {
                return false;
            }

            foreach (PhoneNumber aNumber in a.Phonenumbers)
            {
                foreach (PhoneNumber bNumber in b.Phonenumbers)
                {
                    if (PhoneNumbersAreEqual(aNumber.Value, bNumber.Value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static Contact MergeTwoContacts(Contact source, Contact target, bool overrideProperties)
        {
            return target;
            //if (overrideProperties)
            //{
            //    target = source;
            //} else
            //{
            //    source.Id
            //}
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
        }

        public static List<Contact> MergeContactsPreview(List<Contact> source, List<Contact> target)
        {
            List<Contact> result = new List<Contact>(target);

            foreach (Contact sourceContact in source)
            {
                if (sourceContact.Phonenumbers.Count == 0)
                {
                    continue;
                }

                bool addContact = true;

                foreach (Contact targetContact in result)
                {
                    if (targetContact.Phonenumbers.Count == 0)
                    {
                        continue;
                    }

                    if (ContactsAreEqual(sourceContact, targetContact))
                    {
                        //TODO Check contact update
                        addContact = false;
                        continue;
                    }
                }

                if (addContact)
                {
                    result.Add(sourceContact);
                }
            }

            return result;
        }

        public static List<Contact> MergeContactsGetOnlyNewContacts(List<Contact> source, List<Contact> target)
        {
            List<Contact> result = new List<Contact>();

            foreach (Contact sourceContact in source)
            {
                if (sourceContact.Phonenumbers.Count == 0)
                {
                    continue;
                }

                bool addContact = true;

                foreach (Contact targetContact in result)
                {
                    if (targetContact.Phonenumbers.Count == 0)
                    {
                        continue;
                    }

                    if (ContactsAreEqual(sourceContact, targetContact))
                    {
                        //TODO Check contact update
                        addContact = false;
                        continue;
                    }
                }

                if (addContact)
                {
                    result.Add(sourceContact);
                }
            }

            return result;
        }
    }
}
