using Login.Business;
using Login.Database;
using Login.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhumlaKamnandiHotels
{
    public partial class Reservationsform : Form
    {
        #region Data Members
        private Reservations reservation;//reference variables
        private ReservationsController reservationController;
        private Guest guest;
        private GuestController guestController;
        private Collection<Guest> guests;
        public bool reservationFormClosed = false;
        private Collection<Reservations> reservations;
        /* private string strConn = Settings.Default.PhumlaKamnandiDatabaseConnectionString;
         protected SqlConnection cnMain;
         protected SqlCommand cmd;
         protected SqlDataReader reader1;*///attempt at limiting rooms available, ran out of time
        public enum FormStates
        {
            View,
            Add,
            Edit,
            Delete
        }
        private FormStates state;
        #endregion

        #region Constructor
        public Reservationsform(ReservationsController resController, GuestController guesController) 
        {
            InitializeComponent();
            reservationController = resController;
            guestController = guesController;
            this.Load += Reservationsform_Load;
            this.Activated += Reservationsform_Activated;
            this.FormClosed += Reservationsform_FormClosed;
            state = FormStates.View;
        }
        #endregion

        #region Utility Methods
        private void ClearAll()
        {
            txtReferenceID.Text = "";
            txtCustomerID.Text = "";
            txtGuestID.Text = "";
            txtName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtNoOfRooms.Text = "";
            rdoAddReservation.Checked = false;
            rdoEditReservation.Checked = false;
            rdoDeleteReservation.Checked = false;
            txtDepositConfirmation.Text = "";
        }

        private void PopulateObject()
        {
            try
            {
                reservation = new Reservations();
                reservation.ReferenceNumber = txtReferenceID.Text;
                reservation.ID = txtCustomerID.Text;
                reservation.GuestID = txtGuestID.Text;
                reservation.Name = txtName.Text;
                reservation.Phone = txtPhone.Text;
                reservation.Email = txtEmail.Text;
                reservation.ArrivalDate = dateIn.Value;
                reservation.CheckoutDate = dateOut.Value;
                reservation.DepositConfirmation = txtDepositConfirmation.Text;
                reservation.NumberOfRooms = int.Parse(txtNoOfRooms.Text);
            }
            catch
            {
                MessageBox.Show("Please Fill in All Fields", "Error");
            }
            
        }

        private void EnableEntries(bool value)
        {
            if ((state == FormStates.Edit) && value)
            {
                txtReferenceID.Enabled = !value;
                txtGuestID.Enabled = !value;
                txtCustomerID.Enabled = !value;
            }
            else
            {
                txtReferenceID.Enabled = value;
                txtGuestID.Enabled = value;
                txtCustomerID.Enabled = value;
            }
            txtName.Enabled = value;
            txtPhone.Enabled = value;
            txtEmail.Enabled = value;
            dateIn.Enabled = value;
            dateOut.Enabled = value;
            txtDepositConfirmation.Enabled = value;
            txtNoOfRooms.Enabled = value;
            btnConfirm.Enabled = value;
            btnClear.Enabled = value;
            btnCancel.Enabled = value;
        }

        private void PopulateTextBoxesGuest(Guest guest)
        {
            txtCustomerID.Text = guest.ID;
            txtGuestID.Text = guest.GuestID;
            txtName.Text = guest.Name;
            txtPhone.Text = guest.Phone;
            txtEmail.Text = guest.Email;
        }
        private void PopulateTextBoxesReservations(Reservations reservation)
        {
            txtReferenceID.Text = reservation.ReferenceNumber;
            txtCustomerID.Text = reservation.ID;
            txtGuestID.Text = reservation.GuestID;
            txtName.Text = reservation.Name;
            txtPhone.Text = reservation.Phone;
            txtEmail.Text = reservation.Email;
            dateIn.Value = reservation.ArrivalDate;
            dateOut.Value = reservation.CheckoutDate;
            txtDepositConfirmation.Text = reservation.DepositConfirmation;
            txtNoOfRooms.Text = reservation.NumberOfRooms.ToString();
        }

        /*private int TotalRoomsBooked()
        {
            int roomsbooked = 0;
            cnMain = new SqlConnection(strConn);
            cnMain.Open();
            string selectquary = "SELECT SUM([Number of Rooms]) FROM Reservations WHERE [Check-In Date] = " + dateIn.Value;
            cmd = new SqlCommand(selectquary, cnMain);
            reader1 = cmd.ExecuteReader();
            roomsbooked = Convert.ToInt32(reader1.GetValue(1));
            cnMain.Close();
            return roomsbooked;
        }*/
        #endregion

        #region Form Load, Activated, Closed
        private void Reservationsform_Load(object sender, EventArgs e)
        {
            guestListView.View = View.Details;
            ReservationsListView.View = View.Details;
        }

        private void Reservationsform_Activated(object sender, EventArgs e)
        {
            guestListView.View = View.Details;
            ReservationsListView.View = View.Details;
            setUpGuestListView();
            setUpReservationsListView();
        }

        private void Reservationsform_FormClosed(object sender, FormClosedEventArgs e)
        {
            reservationFormClosed = true;
        }
        #endregion

        #region ListView Setup
        public void setUpGuestListView()
        {
            ListViewItem guestDetails;
            guestListView.Clear();
            guestListView.Columns.Insert(0, "ID", 100, HorizontalAlignment.Left);
            guestListView.Columns.Insert(1, "GuestID", 100, HorizontalAlignment.Left);
            guestListView.Columns.Insert(2, "Name", 100, HorizontalAlignment.Left);
            guestListView.Columns.Insert(3, "Phone", 100, HorizontalAlignment.Left);
            guestListView.Columns.Insert(4, "Email", 100, HorizontalAlignment.Left);
            guestListView.Columns.Insert(5, "Payments Made", 100, HorizontalAlignment.Left);
            guests = null;
            guests = guestController.AllGuests;

            foreach (Guest guest in guests)
            {
                guestDetails = new ListViewItem();
                guestDetails.Text = guest.ID.ToString();
                guestDetails.SubItems.Add(guest.GuestID.ToString());
                guestDetails.SubItems.Add(guest.Name.ToString());
                guestDetails.SubItems.Add(guest.Phone.ToString());
                guestDetails.SubItems.Add(guest.Email.ToString());
                guestDetails.SubItems.Add(guest.Payments.ToString());
                guestListView.Items.Add(guestDetails);

            }
            guestListView.Refresh();
            guestListView.GridLines = true;
        }
        public void setUpReservationsListView()
        {
            ListViewItem reservationdetails;
            ReservationsListView.Clear();
            ReservationsListView.Columns.Insert(0, "Reference Number", 100, HorizontalAlignment.Left);
            ReservationsListView.Columns.Insert(1, "ID", 100, HorizontalAlignment.Left);
            ReservationsListView.Columns.Insert(2, "GuestID", 100, HorizontalAlignment.Left);
            ReservationsListView.Columns.Insert(3, "Name", 100, HorizontalAlignment.Left);
            ReservationsListView.Columns.Insert(4, "Phone", 100, HorizontalAlignment.Left);
            ReservationsListView.Columns.Insert(5, "Email", 100, HorizontalAlignment.Left);
            ReservationsListView.Columns.Insert(6, "Check-In Date", 100, HorizontalAlignment.Left);
            ReservationsListView.Columns.Insert(7, "Check-Out Date", 100, HorizontalAlignment.Left);
            ReservationsListView.Columns.Insert(8, "Deposit Paid", 100, HorizontalAlignment.Left);
            ReservationsListView.Columns.Insert(9, "Number of Rooms", 100, HorizontalAlignment.Left);
            reservations = null;
            reservations = reservationController.AllReservations;

            foreach (Reservations reservation in reservations)
            {
                reservationdetails = new ListViewItem();
                reservationdetails.Text = reservation.ReferenceNumber.ToString();
                reservationdetails.SubItems.Add(reservation.ID.ToString());
                reservationdetails.SubItems.Add(reservation.GuestID.ToString());
                reservationdetails.SubItems.Add(reservation.Name.ToString());
                reservationdetails.SubItems.Add(reservation.Phone.ToString());
                reservationdetails.SubItems.Add(reservation.Email.ToString());
                reservationdetails.SubItems.Add(reservation.ArrivalDate.ToString());
                reservationdetails.SubItems.Add(reservation.CheckoutDate.ToString());
                reservationdetails.SubItems.Add(reservation.DepositConfirmation.ToString());
                reservationdetails.SubItems.Add(reservation.NumberOfRooms.ToString());
                ReservationsListView.Items.Add(reservationdetails);
            }
            ReservationsListView.Refresh();
            ReservationsListView.GridLines = true;
        }

        #endregion

        #region Buttons and Actions
        private void guestListView_SelectedIndexChanged(object sender, EventArgs e)
         {
             if(rdoAddReservation.Checked == true)
             {
                 state = FormStates.Add;
             }
             else if(rdoEditReservation.Checked == true)
             {
                 state = FormStates.Edit;
             }
             else if (rdoDeleteReservation.Checked == true)
             {
                 state = FormStates.Delete;
             }
             else
             {
                 state = FormStates.View;
             }
             EnableEntries(false);
             if (guestListView.SelectedItems.Count > 0)
             {
                 guest = guestController.Find(guestListView.SelectedItems[0].Text);
                 PopulateTextBoxesGuest(guest);
             }
         }

        private void ReservationsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoAddReservation.Checked == true)
            {
                state = FormStates.Add;
            }
            else if (rdoEditReservation.Checked == true)
            {
                state = FormStates.Edit;
            }
            else if (rdoDeleteReservation.Checked == true)
            {
                state = FormStates.Delete;
            }
            else
            {
                state = FormStates.View;
            }
            EnableEntries(true);
            if (ReservationsListView.SelectedItems.Count > 0)
            {
                reservation = reservationController.Find(ReservationsListView.SelectedItems[0].Text);
                PopulateTextBoxesReservations(reservation);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            reservationFormClosed = true;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            PopulateObject();
            if (rdoAddReservation.Checked == true)
            {
                reservationController.DataMaintenance(reservation, DB.DBoperations.Add);
                MessageBox.Show("Confirmation Email Sent", "Success");
            }
            else if (rdoEditReservation.Checked == true)
            {
                reservationController.DataMaintenance(reservation, DB.DBoperations.Edit);
            }
            else if (rdoDeleteReservation.Checked == true)
            {
                reservationController.DataMaintenance(reservation, DB.DBoperations.Delete);
            }
            else if((rdoAddReservation.Checked == false) && (rdoEditReservation.Checked == false) && (rdoDeleteReservation.Checked == false))
            {
                MessageBox.Show("Please Select Reservation Function", "Error");
            }
            reservationController.FinalizeChanges(reservation);
            ClearAll();
            state = FormStates.View;
            EnableEntries(true);
            setUpGuestListView();
            setUpReservationsListView();

        }

        private void guestListView_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (rdoAddReservation.Checked == true)
            {
                state = FormStates.Add;
            }
            else if (rdoEditReservation.Checked == true)
            {
                state = FormStates.Edit;
            }
            else if (rdoDeleteReservation.Checked == true)
            {
                state = FormStates.Delete;
            }
            else
            {
                state = FormStates.View;
            }
            EnableEntries(true);
            if(guestListView.SelectedItems.Count > 0)
            {
                guest = guestController.Find(guestListView.SelectedItems[0].Text);
                PopulateTextBoxesGuest(guest);
            }
        }

        private void rdoEditReservation_CheckedChanged(object sender, EventArgs e)
        {
            state = FormStates.Edit;
            EnableEntries(true);
        }

        private void rdoAddReservation_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("December 1st-7th = Low Season; R550 per room per night" + "\n" +
                "December 8th-15th = Mid Season; R750 per room per night" + "\n" +
                "December 16th-31st = High Season; R995 per room per night" + "\n" +
                "Deposit must be paid 14 days before arrival; 10% of total", "Disclaimer");
            state = FormStates.Add;
            EnableEntries(true);
        }
        private void rdoDeleteReservation_CheckedChanged(object sender, EventArgs e)
        {
            state = FormStates.Delete;
            EnableEntries(true);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
        #endregion

        
    }
}
