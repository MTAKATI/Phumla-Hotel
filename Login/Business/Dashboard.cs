using Login.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Business
{
    public struct RevenueByDate
    {
        public string Date { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public struct BookingCountByMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int BookingCount { get; set; }
    }

    public struct GuestBookingData
    {
        public string GuestName { get; set; }
        public int BookingCount { get; set; }
    }


    public class Dashboard : DashboardDB
        {
        // ... (other members)
        //fields and properties
        private DateTime startTime;
        private DateTime endTime;
        private int numberDays;         //min, max ordered
        private int numberRooms;        //min, max ordered
        private int depositPaid;        //min, max 
        private int profit;
        private string highestPayingGuest;

        public int NumCustomers { get; private set; }
        public List<RevenueByDate> GrossRevenueList { get; set; }
        public int NumOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }

        public int DepositPaid { get; set; }

        public int MinNumberofRoomsOrdered { get; set; }

        public int MaxNumberofRoomsOrdered { get; set; }

        public string HighestPayingGuest { get; set; }

        public Dashboard()
        {

        }

        public void GetNumberCustomers()
            {
            // Initialize startTime and endTime with valid date values
            startTime = new DateTime(1753, 1, 1); // Minimum valid date
            endTime = new DateTime(9999, 12, 31); // Maximum valid date

            using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new System.Data.SqlClient.SqlCommand())
                    {
                        command.Connection = connection;
                        // Get total Number of Guests
                        command.CommandText = "SELECT count(ID) FROM Guests";
                        NumCustomers = (int)command.ExecuteScalar();

                    // deposits paid
                    command.CommandText = "SELECT COUNT(*) AS NumberOfDepositsPaid FROM Reservations WHERE [Deposit Paid] = 'Confirmed'";
                    DepositPaid = (int)command.ExecuteScalar();

                    // minimum number of rooms ordered
                    command.CommandText = "SELECT MIN([Number of Rooms]) AS MinRoomsOrdered FROM Reservations";
                    MinNumberofRoomsOrdered = (int)command.ExecuteScalar();

                    // maximum number of rooms ever ordered
                    command.CommandText = "SELECT MAX([Number of Rooms]) AS MinRoomsOrdered FROM Reservations";
                    MaxNumberofRoomsOrdered = (int)command.ExecuteScalar();

                    //highest amount paid by a guest
                    command.CommandText = "SELECT TOP 1 Name, [Payments Made] FROM Guests ORDER BY [Payments Made] DESC";
                    HighestPayingGuest = (string)command.ExecuteScalar();

                    // Get total Number of Orders within the valid date range
                    command.CommandText = "SELECT count(ID) FROM [Reservations] WHERE [Check-In Date] BETWEEN @fromDate AND @toDate";
                        command.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = startTime;
                        command.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = endTime;
                        NumOrders = (int)command.ExecuteScalar();
                    }
                }
            }

        public List<GuestBookingData> GetGuestBookingData()
        {
            List<GuestBookingData> guestBookings = new List<GuestBookingData>();

            using (var connection = GetConnection()) // Replace with your database connection logic
            {
                connection.Open();
                using (var command = new System.Data.SqlClient.SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                SELECT Name, COUNT(*) AS BookingCount
                FROM [Reservations]
                GROUP BY Name";

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        guestBookings.Add(new GuestBookingData
                        {
                            GuestName = (string)reader["Name"],
                            BookingCount = (int)reader["BookingCount"]
                        });
                    }
                    reader.Close();
                }
            }

            return guestBookings;
        }


        public List<BookingCountByMonth> GetBookingCountOverTime()
        {
            var bookingCountByMonth = new List<BookingCountByMonth>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new System.Data.SqlClient.SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                    SELECT 
                        DATEPART(YEAR, [Check-In Date]) AS Year, 
                        DATEPART(MONTH, [Check-In Date]) AS Month,
                        COUNT(*) AS BookingCount
                    FROM [Reservations]
                    GROUP BY DATEPART(YEAR, [Check-In Date]), DATEPART(MONTH, [Check-In Date])
                    ORDER BY Year, Month";

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        bookingCountByMonth.Add(new BookingCountByMonth
                        {
                            Year = (int)reader["Year"],
                            Month = (int)reader["Month"],
                            BookingCount = (int)reader["BookingCount"]
                        });
                    }
                    reader.Close();
                }
            }

            return bookingCountByMonth;
        }

    public void GetTotalRevenue()
        {
            // Initialize startTime and endTime with valid date values
            startTime = new DateTime(1753, 1, 1); // Minimum valid date
            endTime = new DateTime(9999, 12, 31); // Maximum valid date

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new System.Data.SqlClient.SqlCommand())
                {
                    command.Connection = connection;
                    // Get total Number of Guests
                    command.CommandText = "SELECT SUM(ISNULL([Payments Made], 0)) FROM Guests";
                    TotalRevenue = (int)command.ExecuteScalar();

                    // Get total Number of Orders within the valid date range
                    command.CommandText = "SELECT count(ID) FROM [Reservations] WHERE [Check-In Date] BETWEEN @fromDate AND @toDate";
                    command.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = startTime;
                    command.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = endTime;
                    NumOrders = (int)command.ExecuteScalar();
                }
            }
        }

        public string CalculateBookingDuration(DateTime checkInDate, DateTime checkOutDate)
        {
            TimeSpan duration = checkOutDate - checkInDate;
            int days = duration.Days;

            // You can customize the formatting of the duration as needed
            if (days == 1)
            {
                return "1 day";
            }
            else
            {
                return days + " days";
            }
        }

        public List<Reservations> GetReservations()
        {
            List<Reservations> reservations = new List<Reservations>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new System.Data.SqlClient.SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT CheckInDate, CheckOutDate FROM Reservations";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime checkInDate = (DateTime)reader["CheckInDate"];
                            DateTime checkOutDate = (DateTime)reader["CheckOutDate"];

                            reservations.Add(new Reservations
                            {
                                ArrivalDate = checkInDate,
                                CheckoutDate = checkOutDate
                            });
                        }
                    }
                }
            }

            return reservations;
        }

        public Dictionary<string, int> GetBookingDurationsWithCounts()
        {
            Dictionary<string, int> bookingDurations = new Dictionary<string, int>();

            // Retrieve reservations from the database
            List<Reservations> reservations = GetReservations();

            foreach (var reservation in reservations)
            {
                string duration = CalculateBookingDuration(reservation.ArrivalDate, reservation.CheckoutDate);

                if (bookingDurations.ContainsKey(duration))
                {
                    bookingDurations[duration]++;
                }
                else
                {
                    bookingDurations.Add(duration, 1);
                }
            }

            return bookingDurations;
        }


        public void GetOrderAnalysis()
            {
                GrossRevenueList = new List<RevenueByDate>();
                TotalProfit = 0;
                TotalRevenue = 0;

                // Initialize startTime and endTime with valid date values
                startTime = new DateTime(1753, 1, 1); // Minimum valid date
                endTime = new DateTime(9999, 12, 31); // Maximum valid date


            using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new System.Data.SqlClient.SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"SELECT [Check-In Date], SUM([Deposit Paid])
                                        FROM [Reservations]
                                        WHERE [Check-In Date] BETWEEN @fromDate AND @toDate 
                                        GROUP BY [Check-In Date]";
                        command.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = startTime;
                        command.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = endTime;

                        var reader = command.ExecuteReader();
                        var resultTable = new List<KeyValuePair<DateTime, decimal>>();
                        while (reader.Read())
                        {
                            resultTable.Add(new KeyValuePair<DateTime, decimal>((DateTime)reader[0], (decimal)reader[1]));
                            TotalRevenue += (decimal)reader[1]; // Use reader[1] for deposit sum
                        }
                        TotalProfit = TotalRevenue * 0.2m; // 20%
                        reader.Close();

                        // Group by Days
                        if (numberDays <= 30)
                        {
                            foreach (var item in resultTable)
                            {
                                GrossRevenueList.Add(new RevenueByDate()
                                {
                                    Date = item.Key.ToString("dd MM"),
                                    TotalAmount = item.Value
                                });
                            }
                        }
                    }
                }
            }
        }
    }

