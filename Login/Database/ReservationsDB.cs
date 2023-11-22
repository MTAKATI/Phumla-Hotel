using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.Business;

namespace Login.Database
{
    public class ReservationsDB : DB
    {
        #region Data Members
        private string table2 = "Reservations";//reference variables
        private string sqlLocal2 = "SELECT * FROM Reservations";
        private Collection<Reservations> reservations;
        #endregion

        #region Property Method: Collection
        public Collection<Reservations> AllReservations// getting all reservations from collection
        {
            get { return reservations; }
        }
        #endregion

        #region Constructor
        public ReservationsDB() : base()
        {
            reservations = new Collection<Reservations>();//instantiating collection
            FillDataSet(sqlLocal2, table2);
            Add2Collection(table2);
        }
        #endregion

        #region Utility Methods
        public DataSet GetDataSet()//getting dataset
        {
            return dsMain;
        }

        private void Add2Collection(string table)// adding entry into collection
        {
            DataRow myRow = null;
            Reservations anRes;
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    anRes = new Reservations();
                    anRes.ReferenceNumber = Convert.ToString(myRow["Reference Number"]).TrimEnd();
                    anRes.ID = Convert.ToString(myRow["ID"]).TrimEnd();
                    anRes.GuestID = Convert.ToString(myRow["GuestID"]).TrimEnd();
                    anRes.Name = Convert.ToString(myRow["Name"]).TrimEnd();
                    anRes.Phone = Convert.ToString(myRow["Phone"]).TrimEnd();
                    anRes.Email = Convert.ToString(myRow["Email"]).TrimEnd();
                    anRes.ArrivalDate = Convert.ToDateTime(myRow["Check-In Date"]);
                    anRes.CheckoutDate = Convert.ToDateTime(myRow["Check-Out Date"]);;
                    anRes.DepositConfirmation = Convert.ToString(myRow["Deposit Paid"]);
                    anRes.NumberOfRooms = Convert.ToInt32(myRow["Number of Rooms"]);
                    reservations.Add(anRes);
                }
            }
        }

        private void FillRow(DataRow aRow, Reservations anRes, DBoperations operation)//adding entry into database
        {
            if (operation == DBoperations.Add)
            {
                aRow["Reference Number"] = anRes.ReferenceNumber;
                aRow["ID"] = anRes.ID;
                aRow["GuestID"] = anRes.GuestID;
                aRow["Name"] = anRes.Name;
                aRow["Phone"] = anRes.Phone;
                aRow["Email"] = anRes.Email;
                aRow["Check-In Date"] = anRes.ArrivalDate;
                aRow["Check-Out Date"] = anRes.CheckoutDate;
                aRow["Deposit Paid"] = anRes.DepositConfirmation;
                aRow["Number of Rooms"] = anRes.NumberOfRooms;
            }

        }

        private int FindRow(Reservations anRes, string table) //finding row in table of reservation
        {
            int rowIndex = 0;
            DataRow myRow;
            int returnValue = -1;
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    if (anRes.ReferenceNumber == Convert.ToString(dsMain.Tables[table].Rows[rowIndex]["Reference Number"]))
                    {
                        returnValue = rowIndex; ;
                    }
                }
                rowIndex += 1;
            }
            return returnValue;
        }
        #endregion

        #region Database Operations CRUD
        public void DataSetChange(Reservations anRes, DB.DBoperations operation)
        {
            DataRow aRow = null;
            string dataTable = table2;
            switch (operation)
            {
                case DBoperations.Add:
                    aRow = dsMain.Tables[dataTable].NewRow();//adding reservation to table
                    FillRow(aRow, anRes, operation);
                    dsMain.Tables[dataTable].Rows.Add(aRow);
                    break;
                case DBoperations.Edit:
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(anRes, dataTable)];
                    FillRow(aRow, anRes, operation);//editing reservation from table
                    break;
                case DBoperations.Delete:
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(anRes, dataTable)];//deleting reservation from table
                    aRow.Delete();
                    break;
            }
        }
        #endregion

        #region Build Parameters, Create Commands & Update database
        private void Build_INSERT_Parameters(Reservations anRes)
        {
            SqlParameter param = default(SqlParameter);

            param = new SqlParameter("@[Reference Number]", SqlDbType.NVarChar, 15, "[Reference Number]");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@ID", SqlDbType.NVarChar, 15, "ID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@GuestID", SqlDbType.NVarChar, 10, "GuestID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@Name", SqlDbType.NVarChar, 100, "Name");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@Phone", SqlDbType.NVarChar, 15, "Phone");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@Email", SqlDbType.NVarChar, 50, "Email");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@[Check-In Date]", SqlDbType.DateTime, 50, "[Check-In Date]");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@[Check-Out Date]", SqlDbType.DateTime, 50, "[Check-Out Date]");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@[Deposit Paid]", SqlDbType.NVarChar, 25, "[Deposit Paid]");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@[Number of Rooms]", SqlDbType.Int, 5, "[Number of Rooms]");
            daMain.InsertCommand.Parameters.Add(param);
        }

        private void Create_INSERT_Command(Reservations anRes)
        {
            daMain.InsertCommand = new SqlCommand("INSERT into Reservations ([Reference Number], ID, GuestID, Name, Phone, Email, [Check-In Date], [Check-Out Date], [Deposit Paid], [Number of Rooms]) VALUES (@[Reference Number], @ID, @GuestID, @Name, @Phone, @Email, @[Check-In Date], @[Check-Out Date], @[Deposit Paid], @[Number of Rooms]", cnMain);
            Build_INSERT_Parameters(anRes);
        }

        private void Build_UPDATE_Parameters(Reservations anRes)
        {
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@Name", SqlDbType.NVarChar, 100, "Name");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@Phone", SqlDbType.NVarChar, 15, "Phone");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@Email", SqlDbType.NVarChar, 50, "Email");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@[Check-In Date]", SqlDbType.DateTime, 50, "[Check-In Date]");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@[Check-Out Date]", SqlDbType.DateTime, 50, "[Check-Out Date]");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@[Deposit Paid]", SqlDbType.NVarChar, 25, "[Deposit Paid]");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@[Number of Rooms]", SqlDbType.Int, 5, "[Number of Rooms]");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@Original_ReferenceNumber", SqlDbType.NVarChar, 15, "[Reference Number]");
            param.SourceVersion = DataRowVersion.Original;
            daMain.UpdateCommand.Parameters.Add(param);
        }

        private void Create_UPDATE_Command(Reservations anRes)
        {
            daMain.UpdateCommand = new SqlCommand("UPDATE Reservations SET Name = @Name, Phone = @Phone, Email = @Email, [Check-In Date] = @[Check-In Date], [Check-Out Date] = @[Check-Out Date], [Deposit Made] = @[Deposit Made], [Number of Rooms] = @[Number of Rooms]" + "WHERE [Reference Number] = @Original_ReferenceNumber", cnMain);
            Build_UPDATE_Parameters(anRes);
        }

        private void Build_DELETE_Parameters()
        {
            SqlParameter param;
            param = new SqlParameter("@[Reference Number]", SqlDbType.NVarChar, 15, "[Reference Number]");
            param.SourceVersion = DataRowVersion.Original;
            daMain.DeleteCommand.Parameters.Add(param);
        }

        private void Create_DELETE_Command(Reservations anRes)
        {
            string errorString = null;
            daMain.DeleteCommand = new SqlCommand("DELETE FROM Reservations WHERE [Reference Number] = @[Reference Number]", cnMain);
            try
            {
                Build_DELETE_Parameters();
            }
            catch (Exception errObj)
            {
                errorString = errObj.Message + "  " + errObj.StackTrace;
            }
        }

        public bool UpdateDataSource(Reservations anRes)
        {
            bool success = true;
            Create_INSERT_Command(anRes);
            Create_UPDATE_Command(anRes);
            Create_DELETE_Command(anRes);
            return success;
        }
        #endregion
    }
}
