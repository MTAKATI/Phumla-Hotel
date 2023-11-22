using Login.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login.Database
{
    public class DB
    {
        #region Variable Declaration
        private string strConn = Settings.Default.PhumlaKamnandiDatabaseConnectionString;
        protected SqlConnection cnMain; //referencing database connections
        protected DataSet dsMain;
        protected SqlDataAdapter daMain;
        #endregion

        #region CRUD Enumeration
        public enum DBoperations // setting the state 
        {
            Add = 0,
            Edit = 1,
            Delete = 2
        }
        #endregion

        #region Constructor
        public DB()//Connecting to database
        {
            try
            {
                cnMain = new SqlConnection(strConn);
                dsMain = new DataSet();
            }
            catch (SystemException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Error");//catching if an error occurs
                return;
            }
        }
        #endregion

        #region Update DataSet
        public void FillDataSet(string aSQLstring, string aTable)//filling data set
        {
            try
            {
                daMain = new SqlDataAdapter(aSQLstring, cnMain);
                cnMain.Open();//opening connection
                dsMain.Clear();
                daMain.Fill(dsMain, aTable);//filling table
                cnMain.Close();//clsoing connection
            }
            catch (Exception errObj)
            {
                MessageBox.Show(errObj.Message + "  " + errObj.StackTrace);//catching error
            }
        }
        #endregion

        #region Update DataSource
        protected bool UpdateDataSource(string sqlLocal, string table)
        {
            bool success;
            try
            {
                //open the connection
                cnMain.Open();
                //***update the database table via the data adapter
                daMain.Update(dsMain, table);
                //---close the connection
                cnMain.Close();
                //refresh the dataset
                FillDataSet(sqlLocal, table);
                success = true;
            }
            catch (Exception errObj)
            {
                MessageBox.Show(errObj.Message + "  " + errObj.StackTrace);
                success = false;
            }
            finally
            {
            }
            return success;
        }
        #endregion
    }
}
