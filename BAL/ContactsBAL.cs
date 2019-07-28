using ContactBook.DAL;
using ContactBook.Models;
using ContactBook.Util;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.BAL
{
    public class ContactsBAL
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        #region constructor
        public ContactsBAL(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            contactsDAL = new ContactsDAL(System.IO.Path.Combine(_hostingEnvironment.ContentRootPath, AppSettings.GetSetting("data.file_path")));
        }
        #endregion

        #region private members
        private ContactsDAL contactsDAL;
        #endregion

        #region public members
        public List<Contact> GetContacts(string searchKeyword = "")
        {
            if (string.IsNullOrEmpty(searchKeyword))
                return contactsDAL.GetContacts();
            else
                return contactsDAL.GetContacts(searchKeyword);
        }

        public Contact GetContact(Guid id)
        {
            return contactsDAL.GetContact(id);
        }

        public Guid InsertContact(Contact contact)
        {
            contact.id = Guid.NewGuid();
            return contactsDAL.InsertUpdateContact(contact);
        }

        public Guid UpdateContact(Contact contact)
        {
            return contactsDAL.InsertUpdateContact(contact);
        }

        public bool DeleteContact(Guid id)
        {
            return contactsDAL.DeleteContact(id);
        }

        public bool DeleteContact(string[] ids)
        {
            return contactsDAL.DeleteContact(ids);
        }
        #endregion
    }
}
