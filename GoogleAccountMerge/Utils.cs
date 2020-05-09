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
                        addContact = false;
                        bool diffFound = false;
                        targetContact.Name.FullName = ContactStringAttributesDiff(sourceContact.Name.FullName, targetContact.Name.FullName, ref diffFound);
                        targetContact.Name.GivenName = ContactStringAttributesDiff(sourceContact.Name.GivenName, targetContact.Name.GivenName, ref diffFound);
                        targetContact.Name.FamilyName = ContactStringAttributesDiff(sourceContact.Name.FamilyName, targetContact.Name.FamilyName, ref diffFound);
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

        public static string ContactStringAttributesDiff(string source, string target, ref bool differencesFound)
        {
            if (source == null && target == null)
            {
                differencesFound = false;
                return "";
            }

            if (source == null)
            {
                differencesFound = true;
                return target;
            }

            if (target == null)
            {
                differencesFound = true;
                return source;
            }

            if (!source.Equals(target))
            {
                differencesFound = true;
                return source;
            }

            return target;
        }
        public static ExtensionCollection<EMail> ContactEmailsDiff(ExtensionCollection<EMail> source, ExtensionCollection<EMail> target, ref bool differencesFound)
        {
            ExtensionCollection<EMail> result = new ExtensionCollection<EMail>();

            foreach (EMail targetEmail in target)
            {
                result.Add(targetEmail);
            }

            foreach (EMail sourceEmail in source)
            {
                bool emailExists = false;
                foreach (EMail resultEmail in result)
                {
                    if (sourceEmail.Address.Equals(resultEmail.Address))
                    {
                        emailExists = true;
                        break;
                    }
                }

                if (!emailExists)
                {
                    differencesFound = true;
                    result.Add(sourceEmail);
                }
            }

            return result;
        }
        public static ExtensionCollection<PhoneNumber> ContactPhoneNumbersDiff(ExtensionCollection<PhoneNumber> source, ExtensionCollection<PhoneNumber> target, ref bool differencesFound)
        {
            ExtensionCollection<PhoneNumber> result = new ExtensionCollection<PhoneNumber>();

            foreach (PhoneNumber targetNumber in target)
            {
                result.Add(targetNumber);
            }

            foreach (PhoneNumber sourceNumber in source)
            {
                bool numberExists = false;
                foreach (PhoneNumber resultNumber in result)
                {
                    if (sourceNumber.Value.Equals(resultNumber.Value))
                    {
                        numberExists = true;
                        break;
                    }
                }

                if (!numberExists)
                {
                    differencesFound = true;
                    result.Add(sourceNumber);
                }
            }

            return result;
        }
        public static Contact CloneGoogleContact(Contact source)
        {
            Contact newEntry = new Contact();

            newEntry.Name = new Google.GData.Extensions.Name()
            {
                FullName = source.Name.FullName,
                GivenName = source.Name.GivenName,
                FamilyName = source.Name.FamilyName,
            };

            newEntry.Content = source.Content;

            foreach (PhoneNumber phoneNumber in source.Phonenumbers)
            {
                newEntry.Phonenumbers.Add(phoneNumber);
                continue;
                PhoneNumber newNumber = new PhoneNumber();
                newNumber.Primary = phoneNumber.Primary;
                newNumber.Rel = phoneNumber.Rel.Length > 0 ? phoneNumber.Rel : ContactsRelationships.IsMain;
                newNumber.Value = phoneNumber.Value;
                newEntry.Phonenumbers.Add(newNumber);
            }

            foreach (EMail email in source.Emails)
            {
                newEntry.Emails.Add(email);
                continue;
                EMail newEmail = new EMail();
                newEmail.Rel = email.Rel.Length > 0 ? email.Rel : ContactsRelationships.IsMain;
                newEmail.Address = email.Address;
                newEntry.Emails.Add(newEmail);
            }

            foreach (IMAddress item in source.IMs)
            {
                newEntry.IMs.Add(item);
            }

            foreach (StructuredPostalAddress item in source.PostalAddresses)
            {
                newEntry.PostalAddresses.Add(item);
            }

            return newEntry;
        }
    }
}
