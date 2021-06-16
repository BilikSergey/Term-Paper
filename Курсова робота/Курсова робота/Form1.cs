using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb; 
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace Курсова_робота
{
    public partial class Form1 : Form
    {
        LoadDataClass LDS = new LoadDataClass();
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        public bool newRowAdding = false;

        public Form1()
        {
            InitializeComponent();            
        }
        private void LoadData()
        {
           
            try
            {
                sqlConnection = LDS.getSqlConnection;                
                sqlDataAdapter = LDS.getDataAdapter;
                sqlBuilder = LDS.getCommandBuilder;
                dataSet = LDS.getDataSet;
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                

                sqlDataAdapter.Fill(dataSet, "Table1");

                dataGridView1.DataSource = dataSet.Tables["Table1"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[6, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReloadData()
        {
            sqlConnection = LDS.getSqlConnection;
            sqlDataAdapter = LDS.getDataAdapter;
            sqlBuilder = LDS.getCommandBuilder;
            dataSet = LDS.getDataSet;
            try
            {
                dataSet.Tables["Table1"].Clear();

                sqlDataAdapter.Fill(dataSet, "Table1");

                dataGridView1.DataSource = dataSet.Tables["Table1"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[6, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = LDS.getSqlConnection;

            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("SELECT NGun as 'Назва', PGun as 'Розмір в сантиметрах', TGun as 'Пора року', YGun as 'Вага в грамах' FROM Table1", sqlConnection);

            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            sqlConnection = LDS.getSqlConnection;
            sqlDataAdapter = LDS.getDataAdapter;
            sqlBuilder = LDS.getCommandBuilder;
            dataSet = LDS.getDataSet;
            sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
            try
            {

                if (e.ColumnIndex == 6)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

                    if (task == "DELETE")
                    {
                        if (MessageBox.Show("Видалити цей рядок?", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {                           
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet.Tables["Table1"].Rows[rowIndex].Delete();

                            sqlDataAdapter.Update(dataSet, "Table1");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Table1"].NewRow();

                        row["Назва"] = dataGridView1.Rows[rowIndex].Cells["Назва"].Value;
                        row["Розмір в сантиметрах"] = dataGridView1.Rows[rowIndex].Cells["Розмір в сантиметрах"].Value;
                        row["Пора року"] = dataGridView1.Rows[rowIndex].Cells["Пора року"].Value;
                        row["Вага в грамах"] = dataGridView1.Rows[rowIndex].Cells["Вага в грамах"].Value;
                        row["Матеріал"] = dataGridView1.Rows[rowIndex].Cells["Матеріал"].Value;                       

                        dataSet.Tables["Table1"].Rows.Add(row);

                        dataSet.Tables["Table1"].Rows.RemoveAt(dataSet.Tables["Table1"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "DELETE";

                        sqlDataAdapter.Update(dataSet, "Table1");


                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["Table1"].Rows[r]["Назва"] = dataGridView1.Rows[r].Cells["Назва"].Value;
                        dataSet.Tables["Table1"].Rows[r]["Розмір в сантиметрах"] = dataGridView1.Rows[r].Cells["Розмір в сантиметрах"].Value;
                        dataSet.Tables["Table1"].Rows[r]["Пора року"] = dataGridView1.Rows[r].Cells["Пора року"].Value;
                        dataSet.Tables["Table1"].Rows[r]["Вага в грамах"] = dataGridView1.Rows[r].Cells["Вага в грамах"].Value;
                        dataSet.Tables["Table1"].Rows[r]["Матеріал"] = dataGridView1.Rows[r].Cells["Матеріал"].Value;

                        sqlDataAdapter.Update(dataSet, "Table1");

                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "DELETE";
                    }

                    ReloadData();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            sqlConnection = LDS.getSqlConnection;
            sqlDataAdapter = LDS.getDataAdapter;
            sqlBuilder = LDS.getCommandBuilder;
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[6, lastRow] = linkCell;

                    row.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            sqlConnection = LDS.getSqlConnection;
            sqlDataAdapter = LDS.getDataAdapter;
            sqlBuilder = LDS.getCommandBuilder;
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[6, rowIndex] = linkCell;

                    editingRow.Cells["Command"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);

            if (dataGridView1.CurrentCell.ColumnIndex == 2 || dataGridView1.CurrentCell.ColumnIndex == 4)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
        }

        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            sqlConnection = LDS.getSqlConnection;
            sqlDataAdapter = LDS.getDataAdapter;
            sqlBuilder = LDS.getCommandBuilder;

            sqlConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\Admin\documents\visual studio 2010\Projects\Курсова робота\Курсова робота\Database1.mdf;Integrated Security=True;User Instance=True");
            sqlConnection.Open();
            SqlCommand sqlCommand1 = new SqlCommand("SELECT * FROM Table1 WHERE [Назва] LIKE '%" + textBox1.Text + "'", sqlConnection);
            DataTable dt = new DataTable();
            dt.Load(sqlCommand1.ExecuteReader());
            dataGridView1.DataSource = dt.DefaultView;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            sqlConnection.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\Admin\documents\visual studio 2010\Projects\Курсова робота\Курсова робота\Database1.mdf;Integrated Security=True;User Instance=True");
            sqlConnection.Open();
            SqlCommand sqlCommand2 = new SqlCommand("SELECT * FROM Table1 WHERE [Розмір в сантиметрах] LIKE '%" + textBox2.Text + "'", sqlConnection);
            DataTable dt = new DataTable();
            dt.Load(sqlCommand2.ExecuteReader());
            dataGridView1.DataSource = dt.DefaultView;
            textBox1.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            sqlConnection.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\Admin\documents\visual studio 2010\Projects\Курсова робота\Курсова робота\Database1.mdf;Integrated Security=True;User Instance=True");
            sqlConnection.Open();
            SqlCommand sqlCommand3 = new SqlCommand("SELECT * FROM Table1 WHERE [Пора року] LIKE '%" + textBox3.Text + "'", sqlConnection);
            DataTable dt = new DataTable();
            dt.Load(sqlCommand3.ExecuteReader());
            dataGridView1.DataSource = dt.DefaultView;
            textBox1.Text = null;
            textBox2.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            sqlConnection.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\Admin\documents\visual studio 2010\Projects\Курсова робота\Курсова робота\Database1.mdf;Integrated Security=True;User Instance=True");
            sqlConnection.Open();
            SqlCommand sqlCommand4 = new SqlCommand("SELECT * FROM Table1 WHERE [Вага в грамах] LIKE '%" + textBox4.Text + "'", sqlConnection);
            DataTable dt = new DataTable();
            dt.Load(sqlCommand4.ExecuteReader());
            dataGridView1.DataSource = dt.DefaultView;
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox5.Text = null;
            sqlConnection.Close();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\Admin\documents\visual studio 2010\Projects\Курсова робота\Курсова робота\Database1.mdf;Integrated Security=True;User Instance=True");
            sqlConnection.Open();
            SqlCommand sqlCommand5 = new SqlCommand("SELECT * FROM Table1 WHERE [Матеріал] LIKE '%" + textBox5.Text + "'", sqlConnection);
            DataTable dt = new DataTable();
            dt.Load(sqlCommand5.ExecuteReader());
            dataGridView1.DataSource = dt.DefaultView;
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            sqlConnection.Close();
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
