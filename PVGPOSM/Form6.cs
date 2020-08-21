using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;
using Word = Microsoft.Office.Interop.Word;
using System.Threading;

namespace PVGPOSM
{
    public partial class Form6 : Form
    {
        /// объяевление публичных (открытых) переменных используемых в функциях 
        public string row1;
        public string row2;
        public string photo;
        public string strDes;
        public string connectString = @"Data Source=S6-084-FP; Initial Catalog=BSJobs;  User id =admin; Password=admin"; //строка подключения к DB

        public int indexrow; //переменная хранящая значение индекса строки в POSM
        public int indexrow1; //переменная хранящая значение индекса строки в POSM1
        Word._Application oWord = new Word.Application();

        Thread[] potokUser = new Thread[2]; //массив потоков

        /// основное тело программы
        public Form6(string userProfile)
        {
            InitializeComponent();
            this.Text = userProfile;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.PVGPOSMConstruction". При необходимости она может быть перемещена или удалена.
            this.pVGPOSMConstructionTableAdapter.Fill(this.bSJobsDataSet.PVGPOSMConstruction);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.PVGPOSMCustomer". При необходимости она может быть перемещена или удалена.
            this.pVGPOSMCustomerTableAdapter.Fill(this.bSJobsDataSet.PVGPOSMCustomer);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.PVGPOSM2". При необходимости она может быть перемещена или удалена.
            this.pVGPOSM2TableAdapter.Fill(this.bSJobsDataSet.PVGPOSM2);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.PVGPOSM1". При необходимости она может быть перемещена или удалена.
            this.pVGPOSM1TableAdapter.Fill(this.bSJobsDataSet.PVGPOSM1);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bSJobsDataSet.PVGPOSM". При необходимости она может быть перемещена или удалена.
            this.pVGPOSMTableAdapter.Fill(this.bSJobsDataSet.PVGPOSM);

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //полное выделение строки dgwPVGPOSM
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //полное выделение строки dgwPVGPOSM1
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //полное выделение строки dgwPVGPOSM2

            //постановка изничальных значений в поля фильтров с промежутками
            textBox3.Text = "0";
            textBox4.Text = "100000";
            textBox5.Text = "0";
            textBox6.Text = "5000";
            textBox7.Text = "0";
            textBox8.Text = "5000";
            textBox9.Text = "0";
            textBox10.Text = "5000";
            textBox11.Text = "0";
            textBox12.Text = "1000";
            richTextBox1.Text = null;
            pictureBox1.Image = null;
        }

        private void button1_Click(object sender, EventArgs e) //кнопка обновления(перезгрузки) данных
        {
            Form6_Load(sender, e);
        }

        private void button2_Click(object sender, EventArgs e) //кнопка создание документа word.docx из таблиц по шаблону.
        {
            var wordApp = new Word.Application();

            string fileName = dataGridView1.Rows[indexrow].Cells[1].Value.ToString(); ;
            try
            {
                Word._Document oDoc = GetDoc(@"Z:\PROJEKT 2020\PVGPOSM2020\PVGPOSMPattern.dotm"); //Указываем путь к шаблону
                oDoc.SaveAs(FileName: Environment.CurrentDirectory + @"\ListObject" + fileName + ".doc");   //Путь к заполненному шаблону
                oDoc.Close();
            }
            catch
            {
                MessageBox.Show("Отсутствует доступ к шаблону на exampleJobСonteiner. Обратитесь к системным администратором для подключения сетевого диск exampleJobconteiner. \n Диск обязательно должен быть назван буквой Z!!!");
            }
        }

        private void button3_Click(object sender, EventArgs e) //кнопка применения фильтров
        {
            potokUser[0] = new Thread(Worker_DoWork);
            potokUser[0].Start();
            /*
            try
            {
                pVGPOSMBindingSource.Filter = "[Code] Like'" + textBox1.Text + "%' and [Number] Like'" + textBox2.Text + "%' and [Customer] Like'" + comboBox1.Text + "%' and [Type] Like'" + comboBox2.Text + "%' and [TypePrint] Like'" + comboBox3.Text + "%' and [QuantityShelf] Like'" + comboBox4.Text + "%' and [ProfileYN] Like'" + comboBox5.Text + "%' and [AccessProd] Like'" + comboBox6.Text + "%' and [Quantity] >='" + textBox3.Text + "' and [Quantity] <='" + textBox4.Text + "' and [TotalHeight] >='" + textBox5.Text + "' and [TotalHeight] <='" + textBox6.Text + "' and [Width] >='" + textBox7.Text + "' and [Width] <='" + textBox8.Text + "' and [Depth] >='" + textBox9.Text + "' and [Depth] <='" + textBox10.Text + "' and [LoadShelf] >='" + textBox11.Text + "' and [LoadShelf] <='" + textBox12.Text + "'";
            }
            catch
            {
                MessageBox.Show("Поля фильтров с промежутками не заполнены!");
            }*/
        }

        private void button4_Click(object sender, EventArgs e) //кнопка сброса фильтров
        {
            potokUser[1] = new Thread(Worker_DoWork1);
            potokUser[1].Start();


            /*
            pVGPOSMBindingSource.Filter = "[Code] Like'%'";
            //button2_Click(sender, e); //вызов кнопки обновления формы
            Form6_Load(sender, e); //вызов обновления формы
            textBox1.Text = null;
            textBox2.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;
            comboBox3.Text = null;
            comboBox4.Text = null;
            comboBox5.Text = null;
            comboBox6.Text = null;
            */
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e) //событие определения активной строки таблицы POSM
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                row1 = row.Cells[codeDataGridViewTextBoxColumn.Name].Value.ToString();
                row2 = row.Cells[idDataGridViewTextBoxColumn.Name].Value.ToString();
                indexrow = e.RowIndex;
                try
                {
                    photo = row.Cells[photoDataGridViewTextBoxColumn.Name].Value.ToString();
                    pictureBox1.Image = new Bitmap(photo);
                    //richTextBox1.Text = strDes; 
                }
                catch
                {
                    pictureBox1.Image = null; //очистка pictureBox1 с картинкой объекта
                }
                try
                {
                    strDes = row.Cells[desDataGridViewTextBoxColumn.Name].Value.ToString();
                    richTextBox1.Text = strDes;
                }
                catch
                {
                    richTextBox1.Text = null; //очистка textbox13 с описанием объекта
                }
            }
            else
            {
                MessageBox.Show("Не вся информация по объекту сохранена!");
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //событие двойного клика по строке и последующего вызова масштабной картинки
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    photo = row.Cells[photoDataGridViewTextBoxColumn.Name].Value.ToString();
                    var frm2 = new Form2(photo);
                    frm2.ShowDialog();
                }
                catch
                {
                    MessageBox.Show("Картинка отсутсвует");
                }
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e) //событие клика внутри ячейки (активация гиперссылок)
        {
            //блок запускающий процесс перехода по ссылке (нужна дополнительная библиотека System.Diagnostics)
            try
            {
                Process.Start(dataGridView3.SelectedCells[2].Value.ToString()); //процесс активации гиперссылки
            }
            catch
            {
                MessageBox.Show("Указанный путь к файлу неактивен или отсутствует подключение у сетевому диску!");
            }
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e) //событие определения активной строки таблицы POSM1
        {
            if (e.RowIndex >= 0)
            {
                indexrow1 = e.RowIndex;
            }
            else
            {
                MessageBox.Show("Атрибуты объекта отсутствуют!");
            }
        }

        private Word._Document GetDoc(string path) //функция создания документа по шаблону
        {
            Word._Document oDoc = oWord.Documents.Add(path); //path - путь к шаблону
            SetTemplate(oDoc);
            return oDoc;
        }

        private void SetTemplate(Word._Document oDoc) //функция зпаолнения полей шаблона из таблиц
        {
            //Заполнение данных с первой таблицы POSM
            oDoc.FormFields["Code"].Range.Text = dataGridView1.Rows[indexrow].Cells[1].Value.ToString();
            oDoc.FormFields["Number"].Range.Text = dataGridView1.Rows[indexrow].Cells[2].Value.ToString();
            oDoc.FormFields["PrName"].Range.Text = dataGridView1.Rows[indexrow].Cells[3].Value.ToString();
            oDoc.FormFields["DataLaunch"].Range.Text = dataGridView1.Rows[indexrow].Cells[4].Value.ToString();
            oDoc.FormFields["Quantity"].Range.Text = dataGridView1.Rows[indexrow].Cells[5].Value.ToString();
            oDoc.FormFields["Cost"].Range.Text = dataGridView1.Rows[indexrow].Cells[6].Value.ToString();
            oDoc.FormFields["CostPrice"].Range.Text = dataGridView1.Rows[indexrow].Cells[7].Value.ToString();
            oDoc.FormFields["Type"].Range.Text = dataGridView1.Rows[indexrow].Cells[8].Value.ToString();
            oDoc.FormFields["TotalHeight"].Range.Text = dataGridView1.Rows[indexrow].Cells[9].Value.ToString();
            oDoc.FormFields["Height"].Range.Text = dataGridView1.Rows[indexrow].Cells[10].Value.ToString();
            oDoc.FormFields["Width"].Range.Text = dataGridView1.Rows[indexrow].Cells[11].Value.ToString();
            oDoc.FormFields["Depth"].Range.Text = dataGridView1.Rows[indexrow].Cells[12].Value.ToString();
            oDoc.FormFields["LoadShelf"].Range.Text = dataGridView1.Rows[indexrow].Cells[13].Value.ToString();
            oDoc.FormFields["QuantityShelf"].Range.Text = dataGridView1.Rows[indexrow].Cells[14].Value.ToString();
            oDoc.FormFields["Customer"].Range.Text = dataGridView1.Rows[indexrow].Cells[15].Value.ToString();
            oDoc.FormFields["TypePrint"].Range.Text = dataGridView1.Rows[indexrow].Cells[16].Value.ToString();
            oDoc.FormFields["ProfileYN"].Range.Text = dataGridView1.Rows[indexrow].Cells[17].Value.ToString();
            oDoc.FormFields["Des"].Range.Text = dataGridView1.Rows[indexrow].Cells[19].Value.ToString();
            oDoc.FormFields["AccessProd"].Range.Text = dataGridView1.Rows[indexrow].Cells[20].Value.ToString();
            //Заполнение данных со второй таблицы(связанной по ключу) POSM1
            oDoc.FormFields["Product"].Range.Text = dataGridView2.Rows[indexrow1].Cells[2].Value.ToString();
            oDoc.FormFields["TotalLoad"].Range.Text = dataGridView2.Rows[indexrow1].Cells[3].Value.ToString();
            oDoc.FormFields["Pallet"].Range.Text = dataGridView2.Rows[indexrow1].Cells[4].Value.ToString();
            oDoc.FormFields["Lamination"].Range.Text = dataGridView2.Rows[indexrow1].Cells[5].Value.ToString();
            oDoc.FormFields["Stamp"].Range.Text = dataGridView2.Rows[indexrow1].Cells[6].Value.ToString();
            oDoc.FormFields["VarnishForm"].Range.Text = dataGridView2.Rows[indexrow1].Cells[7].Value.ToString();
            oDoc.FormFields["QuantityPrint"].Range.Text = dataGridView2.Rows[indexrow1].Cells[8].Value.ToString();
            oDoc.FormFields["QuantityNoPrint"].Range.Text = dataGridView2.Rows[indexrow1].Cells[9].Value.ToString();
        }

        private void Worker_DoWork() //функция фильтрации отрабатывающая в отдельном потоке.
        {
            Form6.ActiveForm.Invoke(new Action(() => {
                try
                {
                    pVGPOSMBindingSource.Filter = "[Code] Like'" + textBox1.Text + "%' and [Number] Like'" + textBox2.Text + "%' and [Customer] Like'" + comboBox1.Text + "%' and [Type] Like'" + comboBox2.Text + "%' and [TypePrint] Like'" + comboBox3.Text + "%' and [QuantityShelf] Like'" + comboBox4.Text + "%' and [ProfileYN] Like'" + comboBox5.Text + "%' and [AccessProd] Like'" + comboBox6.Text + "%' and [Quantity] >='" + textBox3.Text + "' and [Quantity] <='" + textBox4.Text + "' and [TotalHeight] >='" + textBox5.Text + "' and [TotalHeight] <='" + textBox6.Text + "' and [Width] >='" + textBox7.Text + "' and [Width] <='" + textBox8.Text + "' and [Depth] >='" + textBox9.Text + "' and [Depth] <='" + textBox10.Text + "' and [LoadShelf] >='" + textBox11.Text + "' and [LoadShelf] <='" + textBox12.Text + "'";
                    potokUser[0].Abort();
                    //MessageBox.Show(potok[0].ThreadState.ToString()); //передача состояния потока в messagebox
                }
                catch
                {
                    potokUser[0].Abort();
                    //MessageBox.Show(potokUser[0].ThreadState.ToString()); //передача состояния потока в messagebox
                    MessageBox.Show("Поля фильтров с промежутками не заполнены!");
                }
            }));
        }

        private void Worker_DoWork1() //функция для сброса фильтра отрабатывающая в отдельном потоке.
        {
            //MessageBox.Show(potok[1].ThreadState.ToString()); //передача состояния потока в messagebox
            Form6.ActiveForm.Invoke(new Action(() => {
                pVGPOSMBindingSource.Filter = "[Code] Like'%'";
                textBox1.Text = null;
                textBox2.Text = null;
                comboBox1.Text = null;
                comboBox2.Text = null;
                comboBox3.Text = null;
                comboBox4.Text = null;
                comboBox5.Text = null;
                comboBox6.Text = null;
                textBox3.Text = "0";
                textBox4.Text = "700000";
                textBox5.Text = "0";
                textBox6.Text = "7000";
                textBox7.Text = "0";
                textBox8.Text = "5000";
                textBox9.Text = "0";
                textBox10.Text = "5000";
                textBox11.Text = "0";
                textBox12.Text = "1000";
                richTextBox1.Text = null;
                pictureBox1.Image = null;
                //MessageBox.Show(potok[1].ThreadState.ToString()); //передача состояния потока в messagebox
                potokUser[1].Abort();
                //MessageBox.Show(potok[1].ThreadState.ToString()); //передача состояния потока в messagebox
            }));
        }
    }
}
