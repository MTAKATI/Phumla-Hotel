using PhumlaKamnandiHotels;
using System;
using Login.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Business
{
    public class ReservationsController
    {
        #region Data Members
        private ReservationsDB reservationsDB;//references
        private Collection<Reservations> reservations;
        #endregion

        #region Properties
        public Collection<Reservations> AllReservations//get all reservations in collection
        {
            get { return reservations; }
        }
        #endregion

        #region Constructor

        public ReservationsController()
        {
            reservationsDB = new ReservationsDB();//instantiating
            reservations = reservationsDB.AllReservations;
        }
        public void DataMaintenance(Reservations anRes, DB.DBoperations operation)
        {
            int index = 0;
            reservationsDB.DataSetChange(anRes, operation);
            switch (operation)
            {
                case DB.DBoperations.Add://adding reservation to database
                    reservations.Add(anRes);
                    break;
                case DB.DBoperations.Edit:
                    index = FindIndex(anRes);//editing existing reservation
                    reservations[index] = anRes;
                    break;
                case DB.DBoperations.Delete://deleting existing reservation
                    index = FindIndex(anRes);
                    reservations.RemoveAt(index);
                    break;
            }
        }

        public bool FinalizeChanges(Reservations anRes)//finalising all chnages made to database
        {
            return reservationsDB.UpdateDataSource(anRes);
        }
        #endregion

        #region Search Methods
        public Reservations Find(string ReferenceNumber)//find reservation
        {
            int index = 0;
            bool found = (reservations[index].ReferenceNumber == ReferenceNumber);
            int count = reservations.Count;
            while (!(found) && (index < reservations.Count - 1))
            {
                index = index + 1;
                found = (reservations[index].ReferenceNumber == ReferenceNumber);
            }
            return reservations[index];
        }

        public int FindIndex(Reservations anRes)//finding index  of reservation in collection
        {
            int counter = 0;
            bool found = false;
            found = (anRes.ReferenceNumber == reservations[counter].ReferenceNumber);
            while (!(found) && (counter < reservations.Count - 1))
            {
                counter = counter + 1;
                found = (anRes.ReferenceNumber == reservations[counter].ReferenceNumber);
            }
            if (found)
            {
                return counter;
            }
            else
            {
                return -1;
            }
        }
        #endregion
    }
}
