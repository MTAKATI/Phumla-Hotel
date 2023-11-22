using Login.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhumlaKamnandiHotels
{
    public partial class Main_MenuMDIParent : Form
    {
        #region Data Members
        private int childFormNumber = 0;
        private Guestsform guestform;
        private Reservationsform reservationsform;
        private GuestController guestController;
        private ReservationsController reservationsController;
        #endregion

        #region Constructor
        public Main_MenuMDIParent()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            guestController = new GuestController();
            reservationsController = new ReservationsController();
        }
        #endregion

        #region Child Forms
        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }
        #endregion

        #region Create Childforms
        public void CreateNewReservationsForm()
        {
            reservationsform = new Reservationsform(reservationsController, guestController); 
            reservationsform.MdiParent = this;
            reservationsform.StartPosition = FormStartPosition.CenterParent;
        }

        public void CreateNewGuestForm()
        {
            guestform = new Guestsform(guestController);
            guestform.MdiParent = this;
            guestform.StartPosition = FormStartPosition.CenterParent;
        }

        #endregion

        #region Toolstrip Menu
        private void guestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (guestform == null)
            {
                CreateNewGuestForm();
            }
            if (guestform.guestformclosed)
            {
                CreateNewGuestForm();
            }
            guestform.Show();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(Form childform in MdiChildren)
            {
                childform.Close();
            }
        }

        private void reservationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reservationsform == null)
            {
                CreateNewReservationsForm();
            }
            if (reservationsform.reservationFormClosed)
            {
                CreateNewReservationsForm();
            }
            reservationsform.Show();
        }
        #endregion

        #region Buttons
        private void btnReservations_Click(object sender, EventArgs e)
        {
            if (reservationsform == null)
            {
                CreateNewReservationsForm();
            }
            if (reservationsform.reservationFormClosed)
            {
                CreateNewReservationsForm();
            }
            reservationsform.Show();
        }

        private void btnGuests_Click(object sender, EventArgs e)
        {
            if (guestform == null)
            {
                CreateNewGuestForm();
            }
            if (guestform.guestformclosed)
            {
                CreateNewGuestForm();
            }
            guestform.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dashboardform dashboard = new Dashboardform();
            dashboard.Show();
        }
        #endregion


    }
}
