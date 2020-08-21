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
    public partial class FormMain : Form
    {
        string connectString = @"Data Source=S6-084-FP; Initial Catalog=BSJobs; User id =admin; Password=admin"; //строка подключения к DB

        public FormMain()
        {
            InitializeComponent();

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet1.PVGPOSMUsers". При необходимости она может быть перемещена или удалена.
            this.pVGPOSMUsersTableAdapter.Fill(this.bSJobsDataSet1.PVGPOSMUsers);
            comboBox1.Text = null;

        }

        private void button1_Click(object sender, EventArgs e) //кнопка ввода
        {
            //переменные для работы с определением профиля и пароля
            string userProfile = comboBox1.Text;
            string PasswordP1 = textBox1.Text;
            string PasswordP;
            string Profile;


                if (userProfile == "admin" & PasswordP1 == "admin")
                {
                    var frm1 = new Form1(userProfile);
                    frm1.Show();
                    textBox1.Text = null;
                    
                }
                else
                {
                    try
                    {

                        SqlConnection sqlConn = new SqlConnection(connectString);
                        sqlConn.Open();
                        string sql = "select PasswordP from PVGPOSMUsers where userprofile = N'" + comboBox1.Text + "'";
                        SqlCommand cmd = new SqlCommand(sql, sqlConn);
                        //cmd.ExecuteNonQuery();
                        PasswordP = cmd.ExecuteScalar().ToString();
                        sqlConn.Close();


                        SqlConnection sqlConn1 = new SqlConnection(connectString);
                        sqlConn1.Open();
                        string sql1 = "select CodeProfile from PVGPOSMUsers where userprofile = N'" + comboBox1.Text + "'";
                        SqlCommand cmd1 = new SqlCommand(sql1, sqlConn1);
                        //cmd.ExecuteNonQuery();
                        Profile = cmd1.ExecuteScalar().ToString();
                        sqlConn1.Close();


                        if (PasswordP == PasswordP1)
                        {
                            switch (Profile)
                            {
                                case "Администратор":
                                    var frm1 = new Form1(userProfile);
                                    frm1.Show();
                                    break;
                                case "Менеджер":
                                    var frm6 = new Form6(userProfile);
                                    frm6.ShowDialog();
                                    break;
                            }
                            textBox1.Text = null;
                        }
                        else
                        {
                            MessageBox.Show("неверный пароль");
                        }
                    }
                    catch
                    {
                        if(userProfile == "admin")
                        {
                            MessageBox.Show("неверный пароль Администратора");
                            textBox1.Text = null;
                        }
                        else
                        {
                            MessageBox.Show("неверный логин");
                        }
                    }
                }

        }
    }
}
