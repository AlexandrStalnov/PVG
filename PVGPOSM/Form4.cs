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

namespace PVGPOSM
{
    public partial class Form4 : Form
    {
        /// объяевление публичных (открытых) переменных используемых в функциях 
        public string connectString = @"Data Source=S6-084-FP; Initial Catalog=BSJobs; User id =admin; Password=admin"; //строка подключения к DB
        
        /// основное тело формы
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.PVGPOSMConstruction". При необходимости она может быть перемещена или удалена.
            this.pVGPOSMConstructionTableAdapter.Fill(this.bSJobsDataSet.PVGPOSMConstruction);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.PVGPOSMCustomer". При необходимости она может быть перемещена или удалена.
            this.pVGPOSMCustomerTableAdapter.Fill(this.bSJobsDataSet.PVGPOSMCustomer);

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //полное выделение строки dgwPVGPOSMCustomer
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //полное выделение строки dgwPVGPOSMConstruction

        }

        private void button1_Click(object sender, EventArgs e) //кнопка добавления нового клиента
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(connectString);
                sqlConn.Open();
                string sql = "insert into PVGPOSMCustomer (Customer) VALUES (N'" + textBox1.Text + "')";
                SqlCommand cmd = new SqlCommand(sql, sqlConn);
                cmd.ExecuteNonQuery();
                sqlConn.Close();
                MessageBox.Show("Новый клиент добавлен.");
                textBox1.Text = null;
            }
            catch
            {
                MessageBox.Show("Ошибка добавления нового клиента.");
            }
        }

        private void button2_Click(object sender, EventArgs e) //кнопка добавления нового типа конструкции
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(connectString);
                sqlConn.Open();
                string sql = "insert into PVGPOSMConstruction (Construction) VALUES (N'" + textBox2.Text + "')";
                SqlCommand cmd = new SqlCommand(sql, sqlConn);
                cmd.ExecuteNonQuery();
                sqlConn.Close();
                MessageBox.Show("Новый тип конструкции добавлен.");
                textBox2.Text = null;
            }
            catch
            {
                MessageBox.Show("Ошибка добавления нового типа конструкции.");
            }
        }

        private void button3_Click(object sender, EventArgs e) //кнопка удаления клиента
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(row);
                //this.pVGPOSMCustomerTableAdapter.Update(this.bSJobsDataSet); //сохранение изменений
            }
        }

        private void button4_Click(object sender, EventArgs e) //кнопка удаления типа конструкции
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.Remove(row);
                //this.pVGPOSMConstructionTableAdapter.Update(this.bSJobsDataSet); //сохранение изменений
            }
        }

        private void button5_Click(object sender, EventArgs e) //кнопка сохранения изменений в таблицах клиента/типа конструкций
        {
            this.pVGPOSMCustomerTableAdapter.Update(this.bSJobsDataSet); //сохранение изменений в таблице клиенитов pVGPOSMCustomer
            this.pVGPOSMConstructionTableAdapter.Update(this.bSJobsDataSet); //сохранение изменений в таблице типов конструкций pVGPOSMConstruction
        }

        private void button6_Click(object sender, EventArgs e) //кнопка обновления(перезгрузки) данных
        {
            Form4_Load(sender, e);
        }
    }
}
