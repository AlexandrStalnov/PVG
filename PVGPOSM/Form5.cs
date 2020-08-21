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
    public partial class Form5 : Form
    {
        /// объяевление публичных (открытых) переменных используемых в функциях 
        public string connectString = @"Data Source=S6-084-FP; Initial Catalog=BSJobs; User id =admin; Password=admin"; //строка подключения к DB

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet1.PVGPOSMUsers". При необходимости она может быть перемещена или удалена.
            this.pVGPOSMUsersTableAdapter.Fill(this.bSJobsDataSet1.PVGPOSMUsers);
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //полное выделение строки dgwPVGPOSMUsers

        }

        private void button1_Click(object sender, EventArgs e) //кнопка добавления нового пользователя
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(connectString);
                sqlConn.Open();
                string sql = "insert into PVGPOSMUsers (UserProfile, PasswordP, CodeProfile) VALUES (N'" + textBox1.Text + "', N'" + textBox2.Text + "', N'" + comboBox1.Text + "')";
                SqlCommand cmd = new SqlCommand(sql, sqlConn);
                cmd.ExecuteNonQuery();
                sqlConn.Close();
                MessageBox.Show("Новый пользователь добавлен.");
                textBox1.Text = null;
                textBox2.Text = null;
                comboBox1.Text = null;
            }
            catch
            {
                MessageBox.Show("Ошибка добавления нового пользователя.");
            }
            //this.pVGPOSMUsersTableAdapter.Update(this.bSJobsDataSet1);
        }

        private void button2_Click(object sender, EventArgs e) //кнопка удаления пользователя
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(row);
                this.pVGPOSMUsersTableAdapter.Update(this.bSJobsDataSet1); //сохранение изменений
            }
        }

        private void button3_Click(object sender, EventArgs e) //кнопка обновления данных dgw PVGPOSMUsers
        {
            Form5_Load(sender, e);
        }
    }
}
