using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace kino2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        List<Button> list_but = new List<Button>(); /// масив для хранения выбраных местов
        int x = 0;
        int Id_seans = 0;

        private void Form1_Load(object sender, EventArgs e) ///загружаем в комбо бокс фильмы
        {
            DataClasses1DataContext context = new DataClasses1DataContext();
            var mas = context.film.ToArray<film>();
            foreach (var film in mas)
            {
                comboBox1.Items.Add(film.name.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //событие которое возникает при выборе фильма
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            panel2.Controls.Clear();
            DataClasses1DataContext context = new DataClasses1DataContext();
            var mas = context.seans.ToList<seans>();
            var mas1 = context.film.ToList<film>();
            int i = 0;
            foreach (var v1 in mas)
            {
                foreach (var v in mas1)                                                /// загрузка времени сеансов и картинка
                {
                    if ((sender as ComboBox).Text == v.name && v1.id_film == v.id)
                    {
                        panel1.Controls[i].Text = v1.time_;
                        panel1.Controls[i].Tag = v1.id;
                        //MessageBox.Show(panel1.Controls[i].Tag.ToString());
                        i++;
                        //open image//
                        byte[] arr = new byte[50000];
                        arr = v.img.ToArray();
                        Image img = Image.FromStream(new MemoryStream(arr));
                        pictureBox1.Image = img;
                        func_clear();
                    }
                }
            }
            list_but.Clear();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)  // событие которое срабатывает при выборе сеанса
        {
            panel2.Controls.Clear();
            func_clear();
            DataClasses1DataContext context = new DataClasses1DataContext();
            var mas2 = context.seans.ToArray<seans>();
            var mas = context.tiket.ToArray<tiket>();
            Id_seans = Convert.ToInt32((sender as RadioButton).Tag);

            foreach (var seans in mas2)
            {
                if (seans.id == Convert.ToInt32((sender as RadioButton).Tag))
                    label3.Text = seans.cena.ToString();

                foreach (var tik in mas)
                {
                    if (tik.id_seans == Convert.ToInt32((sender as RadioButton).Tag)) // рисует места которые занятые
                    {
                        panel2.Controls[tik.gorizontal - 1].BackColor = Color.Red;
                    }
                }
            }
            list_but.Clear();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) // событие которое срабатывает при выборе сеанса
        {
            panel2.Controls.Clear();
            func_clear();
            DataClasses1DataContext context = new DataClasses1DataContext();
            var mas2 = context.seans.ToArray<seans>();
            var mas = context.tiket.ToArray<tiket>();
            Id_seans = Convert.ToInt32((sender as RadioButton).Tag);

            foreach (var seans in mas2)
            {
                if (seans.id == Convert.ToInt32((sender as RadioButton).Tag))
                    label3.Text = seans.cena.ToString();

                foreach (var tik in mas)
                {
                    if (tik.id_seans == Convert.ToInt32((sender as RadioButton).Tag))  // рисует места которые занятые
                    {
                        panel2.Controls[tik.gorizontal - 1].BackColor = Color.Red;
                    }
                }
            }
            list_but.Clear();
        }

        public void func_clear()                            /// ф---ция для проресовки мест
        {
            x = 0;
            textBox1.Text = "";
            label3.Text = "";
            DataClasses1DataContext context = new DataClasses1DataContext();
            var sec = context.sector.ToList<sector>();
            int l = 1;
            for (int k = 0; k < sec[0].vertical; k++)
            {
                for (int j = 0; j < sec[0].gorizontal; j++)
                {
                    List<Button> buttons = new List<Button>();
                    Button b = new Button();
                    b.Location = new Point((j + 1) * 47, (k + 1) * 50);
                    b.Size = new Size(40, 40);
                    b.Text = l.ToString();
                    b.MouseClick += MouseClick_c;
                    panel2.Controls.Add(b);
                    l++;
                }
            }
        }

        private void MouseClick_c(object sender, EventArgs e)  /// сщбытие которое срабатывает при нажатии на место
        {      
            Button b = sender as Button;      
            if (radioButton1.Checked)
            {
                if (b.BackColor == Color.Aqua)
                {
                    b.BackColor = Color.Red;
                    list_but.Add(b);
                    x++;
                    textBox1.Text = (x * Convert.ToInt32(label3.Text)).ToString();
                }
            }
            if (radioButton2.Checked)
            {
                if (b.BackColor == Color.Aqua)
                {
                    b.BackColor = Color.Red;
                    list_but.Add(b);
                    x++;
                    textBox1.Text = (x * Convert.ToInt32(label3.Text)).ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)  //// бронь местов
        {    
            DataClasses1DataContext context = new DataClasses1DataContext();
            var mas = context.tiket.ToArray<tiket>();
            List<Button> list = new List<Button>();
            

            foreach (var v in list_but)
            {
                tiket tiket = new tiket();
                if (Convert.ToInt32(v.Text) > 5 && Convert.ToInt32(v.Text) < 10)
                    tiket.vertical = 2;
                else if (Convert.ToInt32(v.Text) > 10)
                    tiket.vertical = 3;
                else
                    tiket.vertical = 1;

                tiket.gorizontal = Convert.ToInt32(v.Text);
                tiket.id_seans = Id_seans;
                context.tiket.InsertOnSubmit(tiket);
                context.SubmitChanges();
            }
            list_but.Clear();
                MessageBox.Show("successful!");
        }
    }
}

