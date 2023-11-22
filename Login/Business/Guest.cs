using PhumlaKamnandiHotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;

namespace Login.Business
{
    public class Guest : Person
    {
        #region Data Members
        private string guestID;
        private string payments;
        #endregion

        #region Property Methods
        public string GuestID
        {
            get { return guestID; }
            set { guestID = value; }
        }

        public string Payments
        {
            get { return payments; }
            set { payments = value; }
        }
        #endregion

        #region Contructor
        public Guest() : base()
        {
            ID = "";
            guestID = "";
            Name = "";
            Phone = "";
            Email = "";
            payments = "";
        }
        #endregion
    }
}
