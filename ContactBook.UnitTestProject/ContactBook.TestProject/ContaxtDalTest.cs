using System;
using ContactBook.DAL;
using ContactBook.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactBook.TestProject
{
    [TestClass]
    public class ContaxtDalTest
    {
        ContactsDAL dal;
        public ContaxtDalTest()
        {
            dal = new ContactsDAL();
        }

        [TestMethod]
        public void I_TestGetContacts()
        {

        }

        [TestMethod]
        public void II_TestGetContactsWithGuidInput()
        {
            try
            {
                Guid guid = Guid.Parse("049ddd0c-6e63-4b14-b803-4ffc913c2958");
                var contacts = dal.GetContacts(guid);
                Assert.IsNotNull(contacts);
                Assert.AreNotEqual(contacts.Count, 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void III_TestGetContactsWithNameSearch()
        {
            try
            {
                var contacts = dal.GetContact("kunal");
                Assert.IsNotNull(contacts);
                Assert.AreNotEqual(contacts.Count, 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void IV_TestGetContactsWithPhoneSearch()
        {
            try
            {
                var contacts = dal.GetContacts("9999");
                Assert.IsNotNull(contacts);
                Assert.AreNotEqual(contacts.Count, 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void V_TestInsertContact()
        {
            try
            {
                var contact = new Contact
                {
                    id = Guid.Empty,
                    firstName = "Test",
                    lastName = "Name",
                    number = "+919999222919"
                };
                Guid id = dal.InsertUpdateContact(contact);
                Assert.AreNotEqual(id, Guid.Empty);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void VI_TestUpdateContact()
        {
            try
            {
                var contact = new Contact
                {
                    id = Guid.Parse("049ddd0c-6e63-4b14-b803-4ffc913c2958"),
                    firstName = "Test",
                    lastName = "Name",
                    number = "+919999222919"
                };
                Guid id = dal.InsertUpdateContact(contact);
                Assert.AreNotEqual(id, Guid.Empty);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void VI_TestDeleteContact()
        {
            try
            {
                Guid guid = Guid.Parse("049ddd0c-6e63-4b14-b803-4ffc913c2958");
                Assert.IsTrue(dal.DeleteContact(guid));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void VI_TestDeleteMultipleContacts()
        {
            try
            {
                Assert.IsTrue(dal.DeleteContact(new string[] { "049ddd0c-6e63-4b14-b803-4ffc913c2958" }));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
