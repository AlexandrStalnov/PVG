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
    public partial class Form3 : Form
    {
        /// объяевление публичных (открытых) переменных используемых в функциях 
        public string link;
        public string photo;
        public string connectString = @"Data Source=S6-084-FP; Initial Catalog=BSJobs; User id =admin; Password=admin"; //строка подключения

        /// основное тело формы
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.PVGPOSMConstruction". При необходимости она может быть перемещена или удалена.
            this.pVGPOSMConstructionTableAdapter.Fill(this.bSJobsDataSet.PVGPOSMConstruction);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.PVGPOSMCustomer". При необходимости она может быть перемещена или удалена.
            this.pVGPOSMCustomerTableAdapter.Fill(this.bSJobsDataSet.PVGPOSMCustomer);

        }

        private void button1_Click(object sender, EventArgs e) //кнопка добавления ссылки
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                //SqlConnection sqlConn = new SqlConnection(connectString);
                link = folderBrowserDialog1.SelectedPath; //запись пути к папке в переменную
                checkBox1.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e) //кнопка добавления фото
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "(*.JPEG)|*.JPG";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    photo = fileDialog.FileName;
                    checkBox2.Checked = true;
                    pictureBox1.Image = new Bitmap(fileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Картинка не загрузилась!");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) //кнопка сохранения данных в DB
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(connectString);
                //DateTime dt = dateTimePicker1.Value;
                //Блок для вставки строки данных в таблицу объекта PVGPOSM
                sqlConn.Open();
                string sql = "insert into PVGPOSM (Code, Number, Customer, Type, TypePrint, QuantityShelf, ProfileYN, AccessProd, Quantity, TotalHeight, Width, Depth, LoadShelf, Photo, DataLaunch, PrName, Cost, CostPrice, Height, Des) VALUES ('" + textBox1.Text + "','" + textBox2.Text + "',N'" + comboBox1.Text + "',N'" + comboBox2.Text + "',N'" + comboBox3.Text + "','" + comboBox4.Text + "',N'" + comboBox5.Text + "',N'" + comboBox6.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + textBox7.Text + "','" + photo + "','" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',N'" + textBox8.Text + "','" + textBox9.Text + "','" + textBox10.Text + "','" + textBox11.Text + "',N'" + richTextBox1.Text + "')";
                SqlCommand cmd = new SqlCommand(sql, sqlConn);
                cmd.ExecuteNonQuery();
                sqlConn.Close();
                //Блок для вставки строки атрибутов объекта в таблицу атрибутов PVGPOSM1 
                sqlConn.Open();
                string sql1 = "insert into PVGPOSM1 (Id_Req, Product, TotalLoad, Pallet, Lamination, Stamp, VarnishForm, QuantityPrint, QuantityNoPrint) VALUES ((SELECT MAX(id) FROM PVGPOSM) ,N'" + textBox12.Text + "','" + textBox13.Text + "' ,N'" + textBox14.Text + "',N'" + textBox15.Text + "',N'" + textBox16.Text + "',N'" + textBox17.Text + "',N'" + textBox18.Text + "',N'" + textBox19.Text + "')";
                SqlCommand cmd1 = new SqlCommand(sql1, sqlConn);
                cmd1.ExecuteNonQuery();
                sqlConn.Close();
                //Блок для вставки ссылки в таблицу с ссылками на документ PVGPOSM2
                sqlConn.Open();
                string sql2 = "insert into PVGPOSM2 (Id_Req, Link) VALUES ((SELECT MAX(id) FROM PVGPOSM) ,'" + link + "')";
                SqlCommand cmd2 = new SqlCommand(sql2, sqlConn);
                cmd2.ExecuteNonQuery();
                sqlConn.Close();

                MessageBox.Show("Новый объект добавлен.");
            }
            catch
            {
                MessageBox.Show("Ошибка добавления новго объекта! Неверный формат записи данных.");
            }
            finally
            {
                //очистка(обнуление) полей данных формы
                textBox1.Text = null;
                textBox2.Text = null;
                textBox3.Text = null;
                textBox4.Text = null;
                textBox5.Text = null;
                textBox6.Text = null;
                textBox7.Text = null;
                textBox8.Text = null;
                textBox9.Text = null;
                textBox10.Text = null;
                textBox11.Text = null;
                textBox12.Text = null;
                textBox13.Text = null;
                textBox14.Text = null;
                textBox15.Text = null;
                textBox16.Text = null;
                textBox17.Text = null;
                textBox18.Text = null;
                textBox19.Text = null;
                comboBox1.Text = null;
                comboBox2.Text = null;
                comboBox3.Text = null;
                comboBox4.Text = null;
                comboBox5.Text = null;
                comboBox6.Text = null;
                dateTimePicker1.Text = null;
                richTextBox1.Text = null;
                pictureBox1.Image = null;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
        }
    }
}
