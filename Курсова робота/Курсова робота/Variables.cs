using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Курсова_робота
{
    abstract class Variables
    {
        private SqlConnection sqlConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\Admin\documents\visual studio 2010\Projects\Курсова робота\Курсова робота\Database1.mdf;Integrated Security=True;User Instance=True");
        private SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT *, 'DELETE' AS [Command] FROM Table1", (@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\Admin\documents\visual studio 2010\Projects\Курсова робота\Курсова робота\Database1.mdf;Integrated Security=True;User Instance=True"));
        private SqlCommandBuilder sqlBuilder;
        private DataSet dataSet = new DataSet();
        private bool newRowAdding = false;      

        public DataSet getDataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }
        public bool getRowAdding
        {
            get { return newRowAdding; }
            set { newRowAdding = value; }
        }               
        public SqlConnection getSqlConnection
        {
            get { return sqlConnection; }
            set { sqlConnection = value; }
        }
        public SqlDataAdapter getDataAdapter
        {
             get { return sqlDataAdapter; }
            set { sqlDataAdapter = value; }
        }
        public SqlCommandBuilder getCommandBuilder
        {
            get { return sqlBuilder; }
            set { sqlBuilder = value; }
        }              
    }
}
