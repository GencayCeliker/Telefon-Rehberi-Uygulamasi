using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelRehber_OtomasyonB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DbTelRehberBEntities2 db = new DbTelRehberBEntities2();
        private void Form1_Load(object sender, EventArgs e)
        {
            var rehberdekiler = db.TBLKISILER.ToList();

            dataGridView1.DataSource = rehberdekiler;
            dataGridView1.Font = new Font("verdana", 12, FontStyle.Bold);
            dataGridView1.ForeColor = Color.Black;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Tabloya kayıt yapalım

            TBLKISILER yeni = new TBLKISILER();
            yeni.AD = textBox2.Text;
            yeni.SOYAD = textBox3.Text;
            yeni.TEL = textBox4.Text;
            yeni.DOGUMGUNU = Convert.ToDateTime(dateTimePicker1.Value.ToString());
            yeni.ADRES = textBox6.Text;
            if (radioButton1.Checked)  //E işaretli ise
            {
                yeni.CINSIYET = "E";
            }
            else
            {
                yeni.CINSIYET = "K";
            }
            db.TBLKISILER.Add(yeni);
            db.SaveChanges();
            MessageBox.Show("Yeni kayıt eklendi!");

            var rehberdekiler = db.TBLKISILER.ToList();
            dataGridView1.DataSource = rehberdekiler;

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            //harfe göre arama yapalım 
            string aranan = textBox7.Text;
            var arama = db.TBLKISILER.Where(x => x.AD.Contains(aranan) || x.SOYAD.Contains(aranan)).ToList();
            dataGridView1.DataSource = arama;

        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            //ID NOSU VERİLEN KAYDI SİLSİN

            int silnecekkayıtıd = int.Parse(textBox1.Text);
            var silinecek = db.TBLKISILER.Find(silnecekkayıtıd);
            db.TBLKISILER.Remove(silinecek);
            MessageBox.Show("Kayıt silindi!");
            db.SaveChanges();

            var rehberdekiler = db.TBLKISILER.ToList();
            dataGridView1.DataSource = rehberdekiler;

        }
        private void temizle() //Burda method oluşturup ekrandakileri temizledik
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var rehberdekiler = db.TBLKISILER.ToList();
            dataGridView1.DataSource = rehberdekiler;
            temizle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            var kayıtbul = db.TBLKISILER.Find(int.Parse(textBox1.Text));
            kayıtbul.AD = textBox2.Text;
            kayıtbul.SOYAD = textBox3.Text;
            kayıtbul.TEL = textBox4.Text;
            kayıtbul.DOGUMGUNU = Convert.ToDateTime(dateTimePicker1.Value.ToString());
            kayıtbul.ADRES = textBox6.Text;
            kayıtbul.CINSIYET = radioButton1.Checked ? "E" : "K";
            db.SaveChanges();
            MessageBox.Show("Kayıt güncellendi!");
            var rehberdekiler = db.TBLKISILER.ToList();
            dataGridView1.DataSource = rehberdekiler;
            temizle();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            //checkBox1 işaretli ise erkekleri listele
            if (checkBox1.Checked)
            {
                var erkekler = db.TBLKISILER.Where(x => x.CINSIYET == "E").ToList();
                dataGridView1.DataSource = erkekler;
            }
            else
            {
        
                dataGridView1.DataSource =db.TBLKISILER.ToList();
            }   



        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {//datagridviewdeki satıra tıklayınca bilgilerin textboxlara gelmesi
            int satirno = e.RowIndex;
            textBox1.Text = dataGridView1.Rows[satirno].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[satirno].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[satirno].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[satirno].Cells[3].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.Rows[satirno].Cells[4].Value.ToString());
            textBox6.Text = dataGridView1.Rows[satirno].Cells[5].Value.ToString();
            string cinsiyet = dataGridView1.Rows[satirno].Cells[6].Value.ToString();
            if (cinsiyet == "E")
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }


        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //checkBox1 işaretli ise kızları listele
            if (checkBox1.Checked)
            {
                var kızlar = db.TBLKISILER.Where(x => x.CINSIYET == "K").ToList();
                dataGridView1.DataSource = kızlar;
            }
            else
            {

                dataGridView1.DataSource = db.TBLKISILER.ToList();
            }
        }
    }
}
