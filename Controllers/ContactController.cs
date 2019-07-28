using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactBook.BAL;
using ContactBook.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private ContactsBAL contactsBal;
        public ContactController(IHostingEnvironment hostingEnvironment)
        {
            contactsBal = new ContactsBAL(hostingEnvironment);
        }

        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            return contactsBal.GetContacts();
        }

        // GET: api/contact/bb3adf70-d16f-4888-bac8-f14a2f510f02
        // GET: api/contact/kunal
        [HttpGet("{search}")]
        public IEnumerable<Contact> Get(string search)
        {
            var contactsList = new List<Contact>();
            Guid guid = Guid.Empty;
            if (Guid.TryParse(search, out guid))
            {
                contactsList.Add(contactsBal.GetContact(guid));
            }
            else
            {
                contactsList.AddRange(contactsBal.GetContacts(search));
            }
            return contactsList;
        }

        // POST: api/Contact
        [HttpPost]
        public Guid Post([FromBody] Contact contact)
        {
            return contactsBal.InsertContact(contact);
        }

        // PUT: api/Contact
        [HttpPut]
        public Guid Put([FromBody] Contact contact)
        {
            return contactsBal.UpdateContact(contact);
        }

        // DELETE: api/ApiWithActions/bb3adf70-d16f-4888-bac8-f14a2f510f02
        [HttpDelete("{id}")]
        public bool Delete(string id)
        {
            Guid guid = Guid.Empty;
            if (Guid.TryParse(id, out guid))
            {
                return contactsBal.DeleteContact(guid);
            }
            else
            {
                return contactsBal.DeleteContact(id.Split(','));
            }
        }

    }
}
