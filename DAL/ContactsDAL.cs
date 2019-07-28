using ContactBook.Models;
using ContactBook.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.DAL
{
    public class ContactsDAL
    {
        private string _dataPath;
        #region constructor
        public ContactsDAL(string dataFilePath)
        {
            _dataPath = dataFilePath;
        }
        #endregion

        #region private methods

        #endregion

        #region public methods
        public List<Contact> GetContacts()
        {
            var contacts = new List<Contact>();
            if (File.Exists(_dataPath))
            {
                var jContacts = JArray.Parse(File.ReadAllText(_dataPath));
                contacts = jContacts.Select(c => new Contact
                {
                    id = Guid.Parse(c["id"].ToString()),
                    firstName = c["firstName"].ToString(),
                    lastName = c["lastName"].ToString(),
                    number = c["number"].ToString()
                })
                .OrderBy(c => c.firstName)
                .ThenBy(c => c.lastName)
                .ToList();
            }
            return contacts;
        }

        public List<Contact> GetContacts(string search)
        {
            var contacts = new List<Contact>();
            if (File.Exists(_dataPath))
            {
                var jContacts = JArray.Parse(File.ReadAllText(_dataPath));
                contacts = jContacts
                    .Select(c => new Contact
                    {
                        id = Guid.Parse(c["id"].ToString()),
                        firstName = c["firstName"].ToString(),
                        lastName = c["lastName"].ToString(),
                        number = c["number"].ToString()
                    })
                    .Where(c => c.searchExpression.Contains(search))
                    .OrderBy(c => c.firstName)
                    .ThenBy(c => c.lastName)
                    .ToList();
            }
            return contacts;
        }

        public Contact GetContact(Guid id)
        {
            var contact = new Contact();
            if (File.Exists(_dataPath))
            {
                var jContacts = JArray.Parse(File.ReadAllText(_dataPath));
                contact = jContacts
                    .Select(c => new Contact
                    {
                        id = Guid.Parse(c["id"].ToString()),
                        firstName = c["firstName"].ToString(),
                        lastName = c["lastName"].ToString(),
                        number = c["number"].ToString()
                    })
                    .FirstOrDefault(c => c.id.Equals(id));
            }

            return contact;
        }

        public Guid InsertUpdateContact(Contact contact)
        {
            Guid result = Guid.Empty;
            if (File.Exists(_dataPath))
            {
                var jContacts = JArray.Parse(File.ReadAllText(_dataPath));
                var contactList = jContacts
                    .Select(c => new Contact
                    {
                        id = Guid.Parse(c["id"].ToString()),
                        firstName = c["firstName"].ToString(),
                        lastName = c["lastName"].ToString(),
                        number = c["number"].ToString()
                    }).ToList();

                if (contactList.Exists(c => c.id.Equals(contact.id)))
                {
                    var contactToUpdate = contactList.FirstOrDefault(c => c.id.Equals(contact.id));
                    contactToUpdate.firstName = contact.firstName;
                    contactToUpdate.lastName = contact.lastName;
                    contactToUpdate.number = contact.number;
                }
                else
                {
                    contactList.Add(contact);
                }
                string output = JsonConvert.SerializeObject(contactList, Formatting.Indented);
                File.WriteAllText(_dataPath, output);
                result = contact.id;
            }
            return result;
        }

        public bool DeleteContact(Guid id)
        {
            bool result = false;
            if (File.Exists(_dataPath))
            {
                var jContacts = JArray.Parse(File.ReadAllText(_dataPath));
                var contactList = jContacts
                    .Select(c => new Contact
                    {
                        id = Guid.Parse(c["id"].ToString()),
                        firstName = c["firstName"].ToString(),
                        lastName = c["lastName"].ToString(),
                        number = c["number"].ToString()
                    }).ToList();

                if (contactList.Exists(c => c.id.Equals(id)))
                {
                    var contactToDelete = contactList.FirstOrDefault(c => c.id.Equals(id));
                    contactList.Remove(contactToDelete);
                }
                else
                {
                    throw new Exception("Contact with id: " + id.ToString() + "does not exist.");
                }
                string output = JsonConvert.SerializeObject(contactList, Formatting.Indented);
                File.WriteAllText(_dataPath, output);
                result = true;
            }
            return result;
        }

        public bool DeleteContact(string[] ids)
        {
            try
            {
                foreach (string id in ids)
                {
                    DeleteContact(Guid.Parse(id));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
