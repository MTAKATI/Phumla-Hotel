using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Business
{
    public class Reservations : Guest
    {
        #region Data Members
        private string referenceID;
        private string depositConfirmation;
        private DateTime arrivalDate, checkoutDate;
        private int numberOfRooms;
        #endregion

        #region Property Methods
        public string DepositConfirmation
        {
            get { return depositConfirmation; }
            set { depositConfirmation = value; }
        }

        public string ReferenceNumber
        {
            get { return referenceID; }
            set { referenceID = value; }
        }

        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set { arrivalDate = value; }
        }

        public DateTime CheckoutDate
        {
            get { return checkoutDate; }
            set { checkoutDate = value; }
        }

        public int NumberOfRooms
        {
            get { return numberOfRooms; }
            set { numberOfRooms = value; }
        }
        #endregion

        #region Constructor
        public Reservations() : base()
        {
            ID = "";
            GuestID = "";
            Name = "";
            Phone = "";
            Email = "";
            referenceID = "";
            depositConfirmation = "";
            numberOfRooms = 0;
        }
        #endregion
    }
}
