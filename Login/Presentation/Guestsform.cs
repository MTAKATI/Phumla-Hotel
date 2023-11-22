using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Login.Business;
using Login.Database;
using System.Collections.ObjectModel;

namespace PhumlaKamnandiHotels
{
    public partial class Guestsform : Form
    {
        #region Data Members
        private Guest guest;
        private GuestController guestcontroller;//referencing variables
        private Collection<Guest> guests;
        public bool guestformclosed = false;
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
        public Guestsform(GuestController guesController)
        {
            InitializeComponent();
            guestcontroller = guesController;
            this.Load += Guestsform_Load;
            this.Activated += Guestsform_Activated;
            this.FormClosed += Guestsform_FormClosed;
            state = FormStates.View;
        }
        #endregion

        #region Utility Methods
        private void ClearAll()//clearing all fields
        {
            txtID.Text = "";
            txtGuestID.Text = "";
            txtName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtPayments.Text = "";
            rdoAddGuest.Checked = false;
            rdoEditGuest.Checked = false;
            rdoDeleteGuest.Checked = false;
        }

        private void PopulateObject()// populate guest data
        {
            try
            {
                guest = new Guest();
                guest.ID = txtID.Text;
                guest.GuestID = txtGuestID.Text;
                guest.Name = txtName.Text;
                guest.Phone = txtPhone.Text;
                guest.Email = txtEmail.Text;
                guest.Payments = txtPayments.Text;
            }
            catch
            {
                MessageBox.Show("Please Fill in All Fields", "Error");
            }
            
        }

        private void EnableEntries(bool value)//enabling all textboxes
        {
            if((state == FormStates.Edit) && value)//disabling ID and GuestID textbox if editing guest info
            {
                txtID.Enabled = !value;
                txtGuestID.Enabled = !value;
            }
            else
            {
                txtID.Enabled = value;
                txtGuestID.Enabled = value;
            }
            txtName.Enabled = value;
            txtPhone.Enabled = value;
            txtEmail.Enabled = value;
            txtPayments.Enabled = value;
            btnCancel.Enabled = value;
            btnClear.Enabled = value;
            btnConfirm.Enabled = value;
        }

        private void PopulateTextBoxes(Guest guest)// populating textboxes with guest info
        {
            txtID.Text = guest.ID;
            txtGuestID.Text = guest.GuestID;
            txtName.Text = guest.Name;
            txtPhone.Text = guest.Phone;
            txtEmail.Text = guest.Email;
            txtPayments.Text = Convert.ToString(guest.Payments);
        }
        #endregion

        #region Form Load, Activated, Closed
        private void Guestsform_Load(object sender, EventArgs e)
        {
            guestformListView.View = View.Details;
        }

        private void Guestsform_Activated(object sender, EventArgs e)
        {
            guestformListView.View = View.Details;
            setUpGuestListView();
        }

        private void Guestsform_FormClosed(object sender, FormClosedEventArgs e)
        {
            guestformclosed = true;
        }
        #endregion

        #region ListView Setup
        public void setUpGuestListView()
        {
            ListViewItem guestDetails;
            guestformListView.Clear();
            guestformListView.Columns.Insert(0, "ID", 120, HorizontalAlignment.Left);
            guestformListView.Columns.Insert(1, "GuestID", 120, HorizontalAlignment.Left);
            guestformListView.Columns.Insert(2, "Name", 120, HorizontalAlignment.Left);
            guestformListView.Columns.Insert(3, "Phone", 120, HorizontalAlignment.Left);
            guestformListView.Columns.Insert(4, "Email", 120, HorizontalAlignment.Left);
            guestformListView.Columns.Insert(5, "Payments Made", 120, HorizontalAlignment.Left);
            guests = null;
            guests = guestcontroller.AllGuests;
            foreach (Guest guest in guests) //getting all guests from collection database and displaying in ListView
            {
                guestDetails = new ListViewItem();
                guestDetails.Text = guest.ID.ToString();
                guestDetails.SubItems.Add(guest.GuestID.ToString());
                guestDetails.SubItems.Add(guest.Name.ToString());
                guestDetails.SubItems.Add(guest.Phone.ToString());
                guestDetails.SubItems.Add(guest.Email.ToString());
                guestDetails.SubItems.Add(guest.Payments.ToString());
                guestformListView.Items.Add(guestDetails);

            }

            guestformListView.Refresh();
            guestformListView.GridLines = true;
        }
        #endregion

        #region Buttons and Actions
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            PopulateObject();
            if (rdoAddGuest.Checked == true)
            {
                guestcontroller.DataMaintenance(guest, DB.DBoperations.Add);
            }
            else if(rdoEditGuest.Checked == true)
            {
                guestcontroller.DataMaintenance(guest, DB.DBoperations.Edit);
            }
            else if(rdoDeleteGuest.Checked == true)
            {
                guestcontroller.DataMaintenance(guest, DB.DBoperations.Delete);
            }
            else if((rdoAddGuest.Checked == false) && (rdoEditGuest.Checked == false) && (rdoDeleteGuest.Checked == false))
            {
                MessageBox.Show("Please Select Guest Function", "Error");//throwing message if guest function not selected
            }
            guestcontroller.FinalizeChanges(guest);
            ClearAll();
            state = FormStates.View;
            EnableEntries(true);
            setUpGuestListView();
        }

        private void guestformListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rdoEditGuest.Checked == true)//changing form state depending on guest function
            {
                state = FormStates.Edit;
            }
            else if(rdoAddGuest.Checked == true)
            {
                state = FormStates.Add;
            }
            else if(rdoDeleteGuest.Checked == true)
            {
                state = FormStates.Delete;
            }
            else
            {
                state = FormStates.View;
            }
            EnableEntries(true);
            if(guestformListView.SelectedItems.Count > 0)
            {
                guest = guestcontroller.Find(guestformListView.SelectedItems[0].Text);
                PopulateTextBoxes(guest);//populating textboxes with data selected in ListView
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            guestformclosed = true;
        }

        private void rdoEditGuest_CheckedChanged(object sender, EventArgs e)
        {
            state = FormStates.Edit;
            EnableEntries(true);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
        #endregion
    }
}
