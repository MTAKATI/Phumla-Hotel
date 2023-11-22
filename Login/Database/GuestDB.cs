using Login.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Database
{
    public class GuestDB : DB
    {
        #region Data Members
        private string table1 = "Guests";//reference variables
        private string sqlLocal1 = "SELECT * FROM Guests";
        private Collection<Guest> guests;
        #endregion

        #region Property Method: Collection
        public Collection<Guest> AllGuests//getting all guests in the collection
        {
            get { return guests; }
        }
        #endregion

        #region Constructor
        public GuestDB() : base()
        {
            guests = new Collection<Guest>();//instantiating
            FillDataSet(sqlLocal1, table1);
            Add2Collection(table1);
        }
        #endregion

        #region Utility Methods
        public DataSet GetDataSet()//getting the dataset
        {
            return dsMain;
        }

        private void Add2Collection(string table)//adding guest to collection
        {
            DataRow myRow = null;
            Guest angues;
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    angues = new Guest();
                    angues.ID = Convert.ToString(myRow["ID"]).TrimEnd();
                    angues.GuestID = Convert.ToString(myRow["GuestID"]).TrimEnd();
                    angues.Name = Convert.ToString(myRow["Name"]).TrimEnd();
                    angues.Phone = Convert.ToString(myRow["Phone"]).TrimEnd();
                    angues.Email = Convert.ToString(myRow["Email"]).TrimEnd();
                    angues.Payments = Convert.ToString(myRow["Payments Made"]);
                    guests.Add(angues);
                }
            }
        }

        private void FillRow(DataRow aRow, Guest angues, DBoperations operation)//filling the row in table when adding guest to database
        {
            if (operation == DBoperations.Add)
            {
                aRow["ID"] = angues.ID;
                aRow["GuestID"] = angues.GuestID;
                aRow["Name"] = angues.Name;
                aRow["Phone"] = angues.Phone;
                aRow["Email"] = angues.Email;
                aRow["Payments Made"] = angues.Payments;
            }
        }

        private int FindRow(Guest angues, string table)//finding the row number of guest in table
        {
            int rowIndex = 0;
            DataRow myRow;
            int returnValue = -1;
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    if (angues.ID == Convert.ToString(dsMain.Tables[table].Rows[rowIndex]["ID"]))
                    {
                        returnValue = rowIndex;
                    }
                }
                rowIndex += 1;
            }
            return returnValue;
        }
        #endregion

        #region Database Operations CRUD
        public void DataSetChange(Guest angues, DB.DBoperations operation)
        {
            DataRow aRow = null;
            string dataTable = table1;
            switch (operation)
            {
                case DBoperations.Add:
                    aRow = dsMain.Tables[dataTable].NewRow();//adding new entry in table
                    FillRow(aRow, angues, operation);
                    dsMain.Tables[dataTable].Rows.Add(aRow);
                    break;
                case DBoperations.Edit:
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(angues, dataTable)];//editing entry in table
                    FillRow(aRow, angues, operation);
                    break;
                case DBoperations.Delete:
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(angues, dataTable)];//deleting entry in table
                    aRow.Delete();
                    break;
            }
        }
        #endregion

        #region Build Parameters, Create Commands & Update database
        private void Build_INSERT_Parameters(Guest angues)//inserting data into table with specific command
        {
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@ID", SqlDbType.NVarChar, 15, "ID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@GuestID", SqlDbType.NVarChar, 10, "GuestID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@Name", SqlDbType.NVarChar, 50, "Name");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@Phone", SqlDbType.NVarChar, 15, "Phone");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@Email", SqlDbType.NVarChar, 50, "Email");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@[Payments Made]", SqlDbType.Money, 1000000, "[Payments Made]");
            daMain.InsertCommand.Parameters.Add(param);
        }

        private void Create_INSERT_Command(Guest angues)//sql query being made to insert data into table
        {
            daMain.InsertCommand = new SqlCommand("INSERT into Guests (ID, GuestID, Name, Phone, Email, [Payments Made]) VALUES (@ID, @GuestID, @Name, @Phone, @Email, @[Payments Made]", cnMain);
            Build_INSERT_Parameters(angues);
        }

        private void Build_UPDATE_Parameters(Guest angues)// updating entry command
        {
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@Name", SqlDbType.NVarChar, 50, "Name");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@Phone", SqlDbType.NVarChar, 15, "Phone");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@Email", SqlDbType.NVarChar, 50, "Email");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@[Payments Made]", SqlDbType.Money, 1000000, "[Payments Made]");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@Original_ID", SqlDbType.NVarChar, 15, "ID");
            param.SourceVersion = DataRowVersion.Original;
            daMain.UpdateCommand.Parameters.Add(param);


        }

        private void Create_UPDATE_Command(Guest angues)//sql query to update entry
        {
            daMain.UpdateCommand = new SqlCommand("UPDATE Guests SET Name = @Name, Phone = @Phone, Email = @Email, [Payments Made] = @[Payments Made]" + "WHERE ID = @Original_ID", cnMain);
            Build_UPDATE_Parameters(angues);
        }

        private void Build_DELETE_Parameters()//deleting entry command
        {
            SqlParameter param;
            param = new SqlParameter("@ID", SqlDbType.NVarChar, 15, "ID");
            param.SourceVersion = DataRowVersion.Original;
            daMain.DeleteCommand.Parameters.Add(param);
        }

        private void Create_DELETE_Command(Guest angues)//sql query to delete entry from table
        {
            string errorString = null;
            daMain.DeleteCommand = new SqlCommand("DELETE FROM Guests WHERE ID = @ID", cnMain);
            try
            {
                Build_DELETE_Parameters();
            }
            catch (Exception errObj)
            {
                errorString = errObj.Message + "  " + errObj.StackTrace;
            }
        }

        public bool UpdateDataSource(Guest angues)
        {
            bool success = true;
            Create_INSERT_Command(angues);
            Create_UPDATE_Command(angues);
            Create_DELETE_Command(angues);
            return success;
        }
        #endregion
    }
}
