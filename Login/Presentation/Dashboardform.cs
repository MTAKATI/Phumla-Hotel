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
using System.Windows.Forms.DataVisualization.Charting;

namespace PhumlaKamnandiHotels
{
    public partial class Dashboardform : Form
    {
        //fields 
        private Dashboard dashboard = new Dashboard();

        public Dashboardform()
        {
            InitializeComponent();
        }

        private void UpdateNumOrders()
        {
            // Call the method to fetch the number of orders
            dashboard.GetNumberCustomers();

            // Display the number of orders in a label (assuming you have a label named 'lblNumOrders')
            labelValueNumOfOrders.Text = dashboard.NumOrders.ToString();
            NumCustomers.Text = dashboard.NumCustomers.ToString();
            DepositPaid.Text = dashboard.DepositPaid.ToString();
            //la.Text = dashboard.MinNumberofRoomsOrdered.ToString();
            maxRooms.Text = dashboard.MaxNumberofRoomsOrdered.ToString();
            //minRooms.Text = dashboard.MaxNumberofRoomsOrdered.ToString();
            highest.Text = dashboard.HighestPayingGuest.ToString();
        }

        private void Dashboardform_Load(object sender, EventArgs e)
        {
            // Call the method to update the number of orders when the form loads
            UpdateNumOrders();

            // Call the method to update the booking count over time chart when the form loads
            UpdateBookingCountOverTime();

            // Call the method to update the booking duration when the form loads
            UpdateBookingDuration();

            UpdateGuestBookingsPieChart();
        }

        private void UpdateBookingCountOverTime()
        {
            // Call the method to fetch booking counts over time
            var bookingCounts = dashboard.GetBookingCountOverTime();

            // Clear any existing series in the chart
            chartBookingCountOverTime.Series.Clear();

            // Add a new series to the chart
            chartBookingCountOverTime.Series.Add("Booking Count");

            // Configure the chart series
            chartBookingCountOverTime.Series["Booking Count"].ChartType = SeriesChartType.Column;

            // Set axis titles
            chartBookingCountOverTime.ChartAreas[0].AxisX.Title = "Month";
            chartBookingCountOverTime.ChartAreas[0].AxisY.Title = "Booking Count";

            // Customize data point labels
            chartBookingCountOverTime.Series["Booking Count"].IsValueShownAsLabel = true;
            chartBookingCountOverTime.Series["Booking Count"].LabelFormat = "#,0"; // Display integer values
            chartBookingCountOverTime.Series["Booking Count"].CustomProperties = "BarLabelStyle=Center";

            // Bind the data to the chart
            chartBookingCountOverTime.Series["Booking Count"].XValueMember = "Month"; // Assuming you have a "Month" property in your struct
            chartBookingCountOverTime.Series["Booking Count"].YValueMembers = "BookingCount"; // Assuming you have a "BookingCount" property in your struct

            // Set the data source
            chartBookingCountOverTime.DataSource = bookingCounts;
        }

        private void UpdateBookingDuration()
        {
            // Assuming you have reservation data with check-in and check-out dates
            DateTime checkInDate = DateTime.Parse("2023/10/25 16:00:00");
            DateTime checkOutDate = DateTime.Parse("2023/11/01 14:00:00");

            // Calculate booking duration
            string bookingDuration = dashboard.CalculateBookingDuration(checkInDate, checkOutDate);

            // Update the Label control with the booking duration
            chartGuestBookings.Text = bookingDuration;
        }

        private void UpdateGuestBookingsPieChart()
        {
            // Get guest booking data from your method
            List<GuestBookingData> guestBookings = dashboard.GetGuestBookingData();

            // Clear any existing series in the chart
            chartGuestBookings.Series.Clear();

            // Add a new series for the pie chart
            chartGuestBookings.Series.Add("GuestBookings");

            // Set the chart type to Pie
            chartGuestBookings.Series["GuestBookings"].ChartType = SeriesChartType.Pie;

            // Add data points to the series
            foreach (var booking in guestBookings)
            {
                chartGuestBookings.Series["GuestBookings"].Points.AddXY(booking.GuestName, booking.BookingCount);
            }

            // Set chart title and legend
            chartGuestBookings.Titles.Add("Guest Booking Distribution");
            chartGuestBookings.Legends.Add("Legend");

            // Set the chart to display percentages
            chartGuestBookings.Series["GuestBookings"]["PieLabelStyle"] = "Outside";

            // Refresh the chart
            chartGuestBookings.Invalidate();
        }



        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void RoomsOccupied_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnThisMonth_Click(object sender, EventArgs e)
        {

        }

        private void buttonWeek_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void labelNumOrders_Click(object sender, EventArgs e)
        {

        }

        private void labelValueNumOfOrders_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void chartGuestBookings_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click_1(object sender, EventArgs e)
        {

        }

        private void label16_Click_1(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
