using System;
using System.Drawing;
using System.Linq;
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
            dataGridView1.Font = new Font("verdana", 12, FontStyle.Bold);
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // DOGUMGUNU kolonu smalldatetime: 1900 öncesi tarih kabul etmez
            dateTimePicker1.MinDate = new DateTime(1900, 1, 1);

            try
            {
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanına bağlanılamadı. App.config içindeki bağlantı cümlesini kontrol edin.\n\n" + ex.Message,
                    "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Arama kutusu ve cinsiyet filtrelerini birlikte uygulayarak listeyi yeniler
        private void Listele()
        {
            var sorgu = db.TBLKISILER.AsQueryable();

            string aranan = textBox7.Text.Trim();
            if (aranan != "")
            {
                sorgu = sorgu.Where(x => x.AD.Contains(aranan) || x.SOYAD.Contains(aranan));
            }

            // Sadece biri işaretliyse o cinsiyete göre süz; ikisi de işaretliyse ya da hiçbiri değilse hepsini göster
            if (checkBox1.Checked && !checkBox2.Checked)
            {
                sorgu = sorgu.Where(x => x.CINSIYET == "E");
            }
            else if (checkBox2.Checked && !checkBox1.Checked)
            {
                sorgu = sorgu.Where(x => x.CINSIYET == "K");
            }

            dataGridView1.DataSource = sorgu.OrderBy(x => x.KISIID).ToList();
        }

        // Zorunlu alanları kontrol eder
        private bool FormGecerliMi()
        {
            if (textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "")
            {
                MessageBox.Show("Ad, soyad ve telefon alanları boş bırakılamaz.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Lütfen cinsiyet seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // ID kutusundaki değere göre kaydı bulur, bulamazsa kullanıcıyı uyarır
        private TBLKISILER SeciliKayit()
        {
            int id;
            if (!int.TryParse(textBox1.Text, out id))
            {
                MessageBox.Show("Önce listeden bir kişi seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var kayit = db.TBLKISILER.Find(id);
            if (kayit == null)
            {
                MessageBox.Show("Bu ID ile kayıtlı kişi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return kayit;
        }

        // Formdaki bilgileri kişi nesnesine aktarır (Kaydet ve Güncelle ortak kullanır)
        private void FormuKayitaAktar(TBLKISILER kisi)
        {
            kisi.AD = textBox2.Text.Trim();
            kisi.SOYAD = textBox3.Text.Trim();
            kisi.TEL = textBox4.Text.Trim();
            kisi.DOGUMGUNU = dateTimePicker1.Value.Date;
            kisi.ADRES = textBox6.Text.Trim();
            kisi.CINSIYET = radioButton1.Checked ? "E" : "K";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Yeni kayıt ekle
            if (!FormGecerliMi()) return;

            TBLKISILER yeni = new TBLKISILER();
            FormuKayitaAktar(yeni);
            db.TBLKISILER.Add(yeni);
            db.SaveChanges();
            MessageBox.Show("Yeni kayıt eklendi!");

            temizle();
            Listele();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            // Ad veya soyada göre anlık arama
            Listele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // ID'si seçilen kaydı sil
            var silinecek = SeciliKayit();
            if (silinecek == null) return;

            var cevap = MessageBox.Show(silinecek.AD + " " + silinecek.SOYAD + " silinsin mi?", "Silme Onayı",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cevap != DialogResult.Yes) return;

            db.TBLKISILER.Remove(silinecek);
            db.SaveChanges();
            MessageBox.Show("Kayıt silindi!");

            temizle();
            Listele();
        }

        private void temizle() // Ekrandaki kutuları temizler
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox6.Text = "";
            dateTimePicker1.Value = DateTime.Today;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Listeyi yenile: arama ve filtreleri de sıfırla
            temizle();
            textBox7.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            Listele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Seçili kaydı güncelle
            var kayit = SeciliKayit();
            if (kayit == null) return;
            if (!FormGecerliMi()) return;

            FormuKayitaAktar(kayit);
            db.SaveChanges();
            MessageBox.Show("Kayıt güncellendi!");

            temizle();
            Listele();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // "Erkekleri Listele"
            Listele();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            // "Kızları Listele"
            Listele();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Satıra tıklanınca bilgileri kutulara getir (başlık satırına tıklamayı yok say)
            if (e.RowIndex < 0) return;

            var kisi = dataGridView1.Rows[e.RowIndex].DataBoundItem as TBLKISILER;
            if (kisi == null) return;

            textBox1.Text = kisi.KISIID.ToString();
            textBox2.Text = kisi.AD;
            textBox3.Text = kisi.SOYAD;
            textBox4.Text = kisi.TEL == null ? "" : kisi.TEL.Trim();
            dateTimePicker1.Value = kisi.DOGUMGUNU ?? DateTime.Today;
            textBox6.Text = kisi.ADRES;
            radioButton1.Checked = kisi.CINSIYET == "E";
            radioButton2.Checked = kisi.CINSIYET == "K";
        }
    }
}
