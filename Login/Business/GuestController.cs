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
    public class GuestController
    {
        #region Data Members 
        private GuestDB guestDB; //Adding references
        private Collection<Guest> guests;
        #endregion

        #region Properties
        public Collection<Guest> AllGuests //retrieving all guests from database
        {
            get { return guests; }
        }
        #endregion

        #region Constructor
        public GuestController()
        {
            guestDB = new GuestDB();//instantiating 
            guests = guestDB.AllGuests;
        }
        public void DataMaintenance(Guest angues, DB.DBoperations operation)
        {
            int index = 0;
            guestDB.DataSetChange(angues, operation);//performing database operations
            switch (operation)
            {
                case DB.DBoperations.Add:
                    guests.Add(angues);//adding guest to collection
                    break;
                case DB.DBoperations.Edit:
                    index = FindIndex(angues);//updating existing entry
                    guests[index] = angues;
                    break;
                case DB.DBoperations.Delete:
                    index = FindIndex(angues);//deleting entry
                    guests.RemoveAt(index);
                    break;
            }
        }

        public bool FinalizeChanges(Guest angues)
        {
            return guestDB.UpdateDataSource(angues);//Finalising all changes made
        }
        #endregion

        #region Search Methods
        public Guest Find(string ID)//finding guest
        {
            int index = 0;
            bool found = (guests[index].ID == ID);
            int count = guests.Count;
            while (!(found) && (index < guests.Count - 1))
            {
                index = index + 1;
                found = (guests[index].ID == ID);
            }
            return guests[index];
        }

        public int FindIndex(Guest angues)//finding index of guest
        {
            int counter = 0;
            bool found = false;
            found = (angues.ID == guests[counter].ID);
            while (!(found) && (counter < guests.Count - 1))
            {
                counter = counter + 1;
                found = (angues.ID == guests[counter].ID);
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
