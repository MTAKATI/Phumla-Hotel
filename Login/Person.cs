using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhumlaKamnandiHotels
{
    public class Person
    {
        #region Data Members
        private string Id, name, lastName, phone, email;
        #endregion

        #region Properties
        public string ID
        {
            get { return Id; }
            set { Id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        #endregion

        #region Constructor
        public Person()
        {
            Id = "";
            name = "";
            phone = "";
            email = "";
        }

        public Person(string pID, string pName, string pLastName, string pPhone, string pEmail)
        {
            Id = pID;
            name = pName;
            phone = pPhone;
            email = pEmail;
            lastName = pLastName;
        }
        #endregion

        #region ToString Method
        public override string ToString()
        {
            return name + '\n' + Phone;
        }
        #endregion
    }
}
