using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoruntuIslemeProje
{
    public  partial class Form1 : Form
    {
        
        public  Form1()
        {
            InitializeComponent();
        }

        int sayac = 1;
        double x1 = -1;
        double y1 = -1;
        double x2 = -1;
        double y2 = -1;
        double x3 = -1;
        double y3 = -1;
        double x4 = -1;
        double y4 = -1;

        
        //RESMİ KAYDET
        public void ResmiKaydet()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Jpeg Resmi|*.jpg|Bitmap Resmi|*.bmp|Gif Resmi|*.gif";
            saveFileDialog1.Title = "Resmi Kaydet";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "") //Dosya adı boş değilse kaydedecek.
            {
                // FileStream nesnesi ile kayıtı gerçekleştirecek. 
                FileStream DosyaAkisi = (FileStream)saveFileDialog1.OpenFile();
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        pictureBox2.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        pictureBox2.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        pictureBox2.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }
                DosyaAkisi.Close();
            }
        }

        private void resimİndirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResmiKaydet();
        }

        private void resimYükleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog std = new OpenFileDialog();
            std.ShowDialog();
            
            pictureBox1.ImageLocation = std.FileName;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        
        private void griyeÇevirmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi = new Bitmap(pictureBox1.Image);//Bitmap , piksel verileriyle tanımlanan görüntülerle çalışmak için kullanılan bir nesnedir.

                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;

                Bitmap CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);//Çıkış resmi boyutları giriş resmi ile aynı oluyor.

                int GriDeger = 0;
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        double R = OkunanRenk.R;
                        double G = OkunanRenk.G;
                        double B = OkunanRenk.B;

                        GriDeger = Convert.ToInt16(R * 0.299 + G * 0.587 + B * 0.114);
                        DonusenRenk = Color.FromArgb(GriDeger, GriDeger, GriDeger);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void parlaklıkAyarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                int R = 0, G = 0, B = 0;
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);


                        if (trackBar1.Value >= 127)
                        {
                            R = OkunanRenk.R + ((trackBar1.Value - 127));
                            G = OkunanRenk.G + ((trackBar1.Value - 127));
                            B = OkunanRenk.B + ((trackBar1.Value - 127));
                            if (R > 255) R = 255;
                            if (G > 255) G = 255;
                            if (B > 255) B = 255;
                        }
                        else
                        {
                            R = OkunanRenk.R + ((trackBar1.Value - 127));
                            G = OkunanRenk.G + ((trackBar1.Value - 127));
                            B = OkunanRenk.B + ((trackBar1.Value - 127));
                            if (R < 0) R = 0;
                            if (G < 0) G = 0;
                            if (B < 0) B = 0;
                        }

                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void histogramÇıkartmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Histogram, bir resimde herhangi bir rengin belli bir seviyesine ait değeri taşıyan piksellerin 
            //sayısının elde edilmesi ve bunun grafikle ifade edilmesidir.
            pictureBox2.Image = null;
            
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                ArrayList DiziPiksel = new ArrayList();
                int OrtalamaRenk = 0;
                Color OkunanRenk;
                int R = 0, G = 0, B = 0;
                Bitmap GirisResmi; //Histogram için giriş resmi gri-ton olmalıdır.
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
                int ResimYuksekligi = GirisResmi.Height;
                int i = 0; //piksel sayısı tutulacak.
                for (int x = 0; x < GirisResmi.Width; x++)
                {
                    for (int y = 0; y < GirisResmi.Height; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        OrtalamaRenk = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B) / 3; //Griton resimde üç kanal rengi aynı değere sahiptir.
                        DiziPiksel.Add(OrtalamaRenk); //Resimdeki tüm noktaları diziye atıyor. 
                    }
                }
                int[] DiziPikselSayilari = new int[256];
                for (int r = 0; r <= 255; r++) //256 tane renk tonu için dönecek.
                {
                    int PikselSayisi = 0;
                    for (int s = 0; s < DiziPiksel.Count; s++) //resimdeki piksel sayısınca dönecek. 
                    {
                        if (r == Convert.ToInt16(DiziPiksel[s]))
                        PikselSayisi++;
                    }
                    DiziPikselSayilari[r] = PikselSayisi;
                }

                //Değerleri listbox'a ekliyor. 
                int RenkMaksPikselSayisi = 0; //Grafikte y eksenini ölçeklerken kullanılacak. 
                for (int k = 0; k <= 255; k++)
                {
                    listBox1.Items.Add("Renk:" + k + "=" + DiziPikselSayilari[k]);
                    //Maksimum piksel sayısını bulmaya çalışıyor.
                    if (DiziPikselSayilari[k] > RenkMaksPikselSayisi)
                    {
                        RenkMaksPikselSayisi = DiziPikselSayilari[k];
                    }
                }
                //Grafiği çiziyor. 
                Graphics CizimAlani;
                Pen Kalem1 = new Pen(System.Drawing.Color.Black, 1);
                Pen Kalem2 = new Pen(System.Drawing.Color.Red, 1);
                CizimAlani = pictureBox2.CreateGraphics();
                pictureBox2.Refresh();
                int GrafikYuksekligi = 512;
                double OlcekY = RenkMaksPikselSayisi / GrafikYuksekligi;
                double OlcekX = 1.8;
                int X_kaydirma = 30;
                for (int x = 0; x <= 255; x++)
                {
                    if (x % 50 == 0)
                        CizimAlani.DrawLine(Kalem2, (int)(X_kaydirma + x * OlcekX),
                       GrafikYuksekligi, (int)(X_kaydirma + x * OlcekX), 0);
                    CizimAlani.DrawLine(Kalem1, (int)(X_kaydirma + x * OlcekX), GrafikYuksekligi,
                   (int)(X_kaydirma + x * OlcekX), (GrafikYuksekligi - (int)(DiziPikselSayilari[x] / OlcekY)));
                    //Dikey kırmızı çizgiler.
                }
                textBox1.Text = "Maks.Piks=" + RenkMaksPikselSayisi.ToString(); 
            }
        }

        private void eşiklemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //resmin sadece siyah ve beyazdan oluşması.Belli değerin altındaki kısımlar siyah , üstü beyaz olur.
            //Görüntü içerisindeki gri parlaklık eğilimlerini ortadan kaldırmak ve görüntü içerisinde 
            //sadece iki renkten(siyah ve beyaz) bölgeler elde etmek için eşikleme işlemi yapılır.
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                int R = 0, G = 0, B = 0;
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur.           
                int EsiklemeDegeri = Convert.ToInt32(trackBar1.Value);
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        if (OkunanRenk.R >= EsiklemeDegeri)
                            R = 255;
                        else
                            R = 0;
                        if (OkunanRenk.G >= EsiklemeDegeri)
                            G = 255;
                        else
                            G = 0;
                        if (OkunanRenk.B >= EsiklemeDegeri)
                            B = 255;
                        else
                            B = 0;
                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void negatifGörüntülemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk, DonusenRenk;
                int R = 0, G = 0, B = 0;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        R = 255 - OkunanRenk.R;
                        G = 255 - OkunanRenk.G;
                        B = 255 - OkunanRenk.B;
                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void kontrastAyarlamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Görüntüdeki en açık ve en koyu bölgelerin farkı
            //Kontrast görüntüdeki en açık ve en koyu bölgeler arasındaki farkın bir derecesidir. Kontrastı 
            //düşük olan görüntülerin seçilebilirliği iyi olmamaktadır. 
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                int R = 0, G = 0, B = 0;
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur.
                double C_KontrastSeviyesi = Convert.ToInt32(trackBar1.Value);
                double F_KontrastFaktoru = (259 * (C_KontrastSeviyesi + 255)) / (255 * (259 - C_KontrastSeviyesi));
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        R = OkunanRenk.R;
                        G = OkunanRenk.G;
                        B = OkunanRenk.B;
                        R = (int)((F_KontrastFaktoru * (R - 128)) + 128);
                        G = (int)((F_KontrastFaktoru * (G - 128)) + 128);
                        B = (int)((F_KontrastFaktoru * (B - 128)) + 128);
                        //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;
                        if (R < 0) R = 0;
                        if (G < 0) G = 0;
                        if (B < 0) B = 0;
                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void kontrastGermeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //kontrasıt düşük olan görsellerin kontrast değeri yayılır.
            //Kontrast görüntüdeki en açık ve en koyu bölgeler arasındaki farkın bir derecesidir. Kontrastı
            //düşük olan görüntülerin seçilebilirliği iyi olmamaktadır.
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk, DonusenRenk;
                int R = 0, G = 0, B = 0;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı. İçerisine görüntü yüklendi.
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur. Tanımlaması globalde yapıldı.
                int X1 = Convert.ToInt32(textBox2.Text);
                int X2 = Convert.ToInt32(textBox3.Text);
                int Y1 = 0;
                int Y2 = 255;
                int i = 0, j = 0; //Çıkış resminin x ve y si olacak.
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        R = OkunanRenk.R;
                        G = OkunanRenk.G;
                        B = OkunanRenk.B;
                        int Gri = (R + G + B) / 3;

                        int X = Gri;
                        int Y = (((X - X1) * (Y2 - Y1)) / (X2 - X1)) + Y1;
                        if (Y > 255) Y = 255;
                        if (Y < 0) Y = 0;
                        DonusenRenk = Color.FromArgb(Y, Y, Y);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox2.Refresh();
                pictureBox2.Image = null;
                pictureBox2.Image = CikisResmi;
            }
        }

        private void histogramEşitlemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Görüntüdeki düşük karşıtlığı iyileştirmek amacı ile kullanılır. Histogramdaki frekans bilgisine 
            //bağlı olarak yapılan doğrusal olmayan eşlemedir.

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Bitmap renderedImage = new Bitmap(pictureBox1.Image);

                uint pixels = (uint)renderedImage.Height * (uint)renderedImage.Width;
                decimal Const = 255 / (decimal)pixels;

                int x, y, R, G, B;

                int[] HistogramRed2 = new int[256];
                int[] HistogramGreen2 = new int[256];
                int[] HistogramBlue2 = new int[256];


                for (var i = 0; i < renderedImage.Width; i++)
                {
                    for (var j = 0; j < renderedImage.Height; j++)
                    {
                        var piksel = renderedImage.GetPixel(i, j);

                        HistogramRed2[(int)piksel.R]++;
                        HistogramGreen2[(int)piksel.G]++;
                        HistogramBlue2[(int)piksel.B]++;

                    }
                }

                int[] cdfR = HistogramRed2;
                int[] cdfG = HistogramGreen2;
                int[] cdfB = HistogramBlue2;

                for (int r = 1; r <= 255; r++)
                {
                    cdfR[r] = cdfR[r] + cdfR[r - 1];
                    cdfG[r] = cdfG[r] + cdfG[r - 1];
                    cdfB[r] = cdfB[r] + cdfB[r - 1];
                }

                for (y = 0; y < renderedImage.Height; y++)
                {
                    for (x = 0; x < renderedImage.Width; x++)
                    {
                        Color pixelColor = renderedImage.GetPixel(x, y);

                        R = (int)((decimal)cdfR[pixelColor.R] * Const);
                        G = (int)((decimal)cdfG[pixelColor.G] * Const);
                        B = (int)((decimal)cdfB[pixelColor.B] * Const);

                        Color newColor = Color.FromArgb(R, G, B);
                        renderedImage.SetPixel(x, y, newColor);
                    }
                }
                pictureBox2.Image = renderedImage;
            }
        }

        private void gaussFiiltresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Verilen bir resim üzerinde yumuşatma işlemi uygulamak için kullanılır. Diğer bir tabirle 
            //resim üzerindeki gürültüyü kaldırır.
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 0; //Çekirdek matrisin boyutu
                try
                {
                    SablonBoyutu = Convert.ToInt32(textBox2.Text);
                }
                catch
                {
                    SablonBoyutu = 5;
                }    
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;
                int[] Matris = { 1, 4, 7, 4, 1, 4, 20, 33, 20, 4, 7, 33, 55, 33, 7, 4, 20, 33, 20, 4, 1, 4, 7, 4, 1 };
                int MatrisToplami = 1 + 4 + 7 + 4 + 1 + 4 + 20 + 33 + 20 + 4 + 7 + 33 + 55 + 33 + 7 + 4 + 20 + 33 + 20 + 4 + 1 + 4 + 7 + 4 + 1;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        toplamR = 0;
                        toplamG = 0;
                        toplamB = 0;
                        //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                        int k = 0; //matris içindeki elemanları sırayla okurken kullanılacak.
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                toplamR = toplamR + OkunanRenk.R * Matris[k];
                                toplamG = toplamG + OkunanRenk.G * Matris[k];
                                toplamB = toplamB + OkunanRenk.B * Matris[k];
                                k++;
                            }
                        }
                        ortalamaR = toplamR / MatrisToplami;
                        ortalamaG = toplamG / MatrisToplami;
                        ortalamaB = toplamB / MatrisToplami;
                        CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void meanFiltresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Komşu pikseller arasındaki büyük parlaklık değerleri farklarını aritmetik, geometrik veya 
            //harmonik ortalama kullanarak azaltıp görüntü üzerindeki istenmeyen gürültüleri elimine etmektir.
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);

                int ResimGenisligi = GirisResmi.Width; int ResimYuksekligi = GirisResmi.Height;

                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

                int SablonBoyutu; //Çekirdek matrisin boyutu
                try
                {
                    SablonBoyutu = Convert.ToInt32(textBox2.Text);//şablon boyutu 3 den büyük tek rakam olmalıdır (3,5,7 gibi).
                }
                catch
                {
                    SablonBoyutu = 5;
                }
                int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;

                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        toplamR = 0;
                        toplamG = 0;
                        toplamB = 0;

                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);

                                toplamR = toplamR + OkunanRenk.R; toplamG = toplamG + OkunanRenk.G; toplamB = toplamB + OkunanRenk.B;

                            }
                        }
                        ortalamaR = toplamR / (SablonBoyutu * SablonBoyutu);
                        ortalamaG = toplamG / (SablonBoyutu * SablonBoyutu);
                        ortalamaB = toplamB / (SablonBoyutu * SablonBoyutu);
                        CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void medyanFiltresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Medyan filtresi resimde veya seçili bölgedeki pixellerin parlaklıklarını harmanlar ve 
            //istenmeyen parazitleri azaltır.Ortanca değeri alır.
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);

                int ResimGenisligi = GirisResmi.Width; int ResimYuksekligi = GirisResmi.Height;

                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

                int SablonBoyutu = 0;//şablon boyutu 3 den büyük tek rakam olmalıdır (3,5,7 gibi).
                try
                {
                    SablonBoyutu = Convert.ToInt32(textBox2.Text);
                }
                catch
                {
                    SablonBoyutu = 5;
                }
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;

                int[] R = new int[ElemanSayisi]; int[] G = new int[ElemanSayisi]; int[] B = new int[ElemanSayisi]; int[] Gri = new int[ElemanSayisi];

                int x, y, i, j;

                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                        int k = 0;
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                R[k] = OkunanRenk.R; G[k] = OkunanRenk.G; B[k] = OkunanRenk.B;

                                Gri[k] = Convert.ToInt16(R[k] * 0.299 + G[k] * 0.587 + B[k] * 0.114); //Gri ton formülü

                                k++;
                            }
                        }

                        //Gri tona göre sıralama yapıyor. Aynı anda üç rengide değiştiriyor.
                        int GeciciSayi = 0;

                        for (i = 0; i < ElemanSayisi; i++)
                        {
                            for (j = i + 1; j < ElemanSayisi; j++)
                            {
                                if (Gri[j] < Gri[i])
                                {
                                    GeciciSayi = Gri[i]; Gri[i] = Gri[j];
                                    Gri[j] = GeciciSayi;

                                    GeciciSayi = R[i]; R[i] = R[j];
                                    R[j] = GeciciSayi;

                                    GeciciSayi = G[i]; G[i] = G[j];
                                    G[j] = GeciciSayi;

                                    GeciciSayi = B[i]; B[i] = B[j];
                                    B[j] = GeciciSayi;
                                }
                            }
                        }

                        //Sıralama sonrası ortadaki değeri çıkış resminin piksel değeri olarak atıyor.
                        CikisResmi.SetPixel(x, y, Color.FromArgb(R[(ElemanSayisi - 1) / 2], G[(ElemanSayisi - 1) / 2], B[(ElemanSayisi - 1) / 2]));
                    }
                }

                pictureBox2.Image = CikisResmi;
            }
        }

        private void sobelFiltresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Bu filtreler, bir resimdeki farklı renkler arasındaki sınırları bularak resimde yer alan 
            //nesnelerin dış hatlarını belirlememizi sağlar.
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Bitmap GirisResmi, CikisResmiXY, CikisResmiX, CikisResmiY;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmiX = new Bitmap(ResimGenisligi, ResimYuksekligi);
                CikisResmiY = new Bitmap(ResimGenisligi, ResimYuksekligi);
                CikisResmiXY = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 3;
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y;
                Color Renk;
                int P1, P2, P3, P4, P5, P6, P7, P8, P9;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        Renk = GirisResmi.GetPixel(x - 1, y - 1);
                        P1 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y - 1);
                        P2 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y - 1);
                        P3 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y);
                        P4 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y);
                        P5 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y);
                        P6 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y + 1);
                        P7 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y + 1);
                        P8 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y + 1);
                        P9 = (Renk.R + Renk.G + Renk.B) / 3;
                        //Hesaplamayı yapan Sobel Temsili matrisi ve formülü.
                        int Gx = Math.Abs(-P1 + P3 - 2 * P4 + 2 * P6 - P7 + P9); //Dikey çizgiler
                        int Gy = Math.Abs(P1 + 2 * P2 + P3 - P7 - 2 * P8 - P9); //Yatay Çizgiler
                                                                                
                        int Gxy = Gx + Gy;
                        
                        if (Gx > 255) Gx = 255;
                        if (Gy > 255) Gy = 255;
                        if (Gxy > 255) Gxy = 255;
                        
                        CikisResmiX.SetPixel(x, y, Color.FromArgb(Gx, Gx, Gx));
                        CikisResmiY.SetPixel(x, y, Color.FromArgb(Gy, Gy, Gy));
                        CikisResmiXY.SetPixel(x, y, Color.FromArgb(Gxy, Gxy, Gxy));
                    }
                }
                pictureBox2.Image = CikisResmiXY;
            }
        }

        private void prewittFiltresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Resimdeki farklı renkler arasındaki sınırları bularak resimde yer alan nesnelerin dış hatlarını 
            //belirlememizi sağlar. Resim üstünde ayrı ayrı yatay ve düşey kenarları belirginleştirir.
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 3;
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y;
                Color Renk;
                int P1, P2, P3, P4, P5, P6, P7, P8, P9;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        Renk = GirisResmi.GetPixel(x - 1, y - 1);
                        P1 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y - 1);
                        P2 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y - 1);
                        P3 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y);
                        P4 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y);
                        P5 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y);
                        P6 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x - 1, y + 1);
                        P7 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x, y + 1);
                        P8 = (Renk.R + Renk.G + Renk.B) / 3;
                        Renk = GirisResmi.GetPixel(x + 1, y + 1);
                        P9 = (Renk.R + Renk.G + Renk.B) / 3;
                        int Gx = Math.Abs(-P1 + P3 - P4 + P6 - P7 + P9); //Dikey çizgileri Bulur
                        int Gy = Math.Abs(P1 + P2 + P3 - P7 - P8 - P9); //Yatay Çizgileri Bulur.
                        int PrewittDegeri = 0;
                        PrewittDegeri = Gx;
                        PrewittDegeri = Gy;
                        PrewittDegeri = Gx + Gy; 
                                                 //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                        if (PrewittDegeri > 255) PrewittDegeri = 255;
                        
                        CikisResmi.SetPixel(x, y, Color.FromArgb(PrewittDegeri, PrewittDegeri, PrewittDegeri));
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void laplacianFiltresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Laplas filtresi basitçe bir resimdeki kenar hatlarını belirlemek için kullanılır.
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Bitmap img = new Bitmap(pictureBox1.Image);
                Bitmap image = new Bitmap(img);
                Color OkunanRenk;
                int R = 0, G = 0, B = 0;
                for (int x = 1; x < image.Width - 1; x++)
                {
                    for (int y = 1; y < image.Height - 1; y++)
                    {
                        OkunanRenk = img.GetPixel(x, y);

                        Color color2, color4, color5, color6, color8;

                        color2 = img.GetPixel(x, y - 1);
                        color4 = img.GetPixel(x - 1, y);
                        color5 = img.GetPixel(x, y);
                        color6 = img.GetPixel(x + 1, y);
                        color8 = img.GetPixel(x, y + 1);

                        R = color2.R + color4.R + color5.R * (-4) + color6.R + color8.R;
                        G = color2.G + color4.G + color5.G * (-4) + color6.G + color8.G;
                        B = color2.B + color4.B + color5.B * (-4) + color6.B + color8.B;


                        int avg = (R + G + B) / 3;
                        if (avg > 255)
                            avg = 255;
                        if (avg < 0)
                            avg = 0;

                        image.SetPixel(x, y, Color.FromArgb(avg, avg, avg));

                    }
                }
                pictureBox2.Image = image;
            }
        }

        private void açılıDöndürmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int Aci = 0;
                try
                {
                    Aci = Convert.ToInt16(textBox2.Text);
                }
                catch (Exception)
                {
                    Aci = 0;
                }
                
                double RadyanAci = Aci * 2 * Math.PI / 360;
                double x2 = 0, y2 = 0;
                //Resim merkezini buluyor. Resim merkezi etrafında döndürecek. 
                int x0 = ResimGenisligi / 2;
                int y0 = ResimYuksekligi / 2;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);
                        //Aliaslı Döndürme -Sağa Kaydırma
                        x2 = (x1 - x0) - Math.Tan(RadyanAci / 2) * (y1 - y0) + x0;
                        y2 = (y1 - y0) + y0;
                        x2 = Convert.ToInt16(x2);
                        y2 = Convert.ToInt16(y2);
                        //Aliaslı Döndürme -Aşağı kaydırma
                        x2 = (x2 - x0) + x0;
                        y2 = Math.Sin(RadyanAci) * (x2 - x0) + (y2 - y0) + y0;
                        x2 = Convert.ToInt16(x2);
                        y2 = Convert.ToInt16(y2);
                        //Aliaslı Döndürme -Sağa Kaydırma
                        x2 = (x2 - x0) - Math.Tan(RadyanAci / 2) * (y2 - y0) + x0;
                        y2 = (y2 - y0) + y0;
                        x2 = Convert.ToInt16(x2);
                        y2 = Convert.ToInt16(y2);
                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void tersÇevirmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                double Aci = 180;
                double RadyanAci = Aci * 2 * Math.PI / 360;
                double x2 = 0, y2 = 0;
                //Resim merkezini buluyor. Resim merkezi etrafında döndürecek. 
                int x0 = ResimGenisligi / 2;
                int y0 = ResimYuksekligi / 2;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);

                        //----C-Ortadan geçen 45 açılı çizgi etrafında aynalama----------
                        double Delta = (x1 - x0) * Math.Sin(RadyanAci) - (y1 - y0) * Math.Cos(RadyanAci);
                        x2 = Convert.ToInt16(x1 + 2 * Delta * (-Math.Sin(RadyanAci)));
                        y2 = Convert.ToInt16(y1 + 2 * Delta * (Math.Cos(RadyanAci)));
                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void aynalamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                double Aci = 90;
                double RadyanAci = Aci * 2 * Math.PI / 360;
                double x2 = 0, y2 = 0;
                //Resim merkezini buluyor. Resim merkezi etrafında döndürecek. 
                int x0 = ResimGenisligi / 2;
                int y0 = ResimYuksekligi / 2;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);
                       
                        //----C-Ortadan geçen 45 açılı çizgi etrafında aynalama----------
                        double Delta = (x1 - x0) * Math.Sin(RadyanAci) - (y1 - y0) * Math.Cos(RadyanAci);
                        x2 = Convert.ToInt16(x1 + 2 * Delta * (-Math.Sin(RadyanAci)));
                        y2 = Convert.ToInt16(y1 + 2 * Delta * (Math.Cos(RadyanAci)));
                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void yakınlaştırmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int BuyutmeKatsayisi = 0;
                try
                {
                    BuyutmeKatsayisi = Convert.ToInt32(textBox2.Text);
                }
                catch (Exception)
                {
                    BuyutmeKatsayisi = 1;
                }

                int x2 = 0, y2 = 0;
                for (int x1 = 0; x1 < ResimGenisligi; x1++)
                {

                    for (int y1 = 0; y1 < ResimYuksekligi; y1++)
                    {

                        OkunanRenk = GirisResmi.GetPixel(x1, y1);

                        for (int i = 0; i < BuyutmeKatsayisi; i++)
                        {
                            for (int j = 0; j < BuyutmeKatsayisi; j++)
                            {
                                x2 = x1 * BuyutmeKatsayisi + i;
                                y2 = y1 * BuyutmeKatsayisi + j;
                                if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                                    CikisResmi.SetPixel(x2, y2, OkunanRenk);
                            }
                        }

                    }

                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void uzaklaştırmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                int R = 0, G = 0, B = 0;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resminin boyutları
                int x2 = 0, y2 = 0; //Çıkış resminin x ve y si olacak.
                int KucultmeKatsayisi = 0;
                try
                {
                    KucultmeKatsayisi = Convert.ToInt32(textBox2.Text);
                }
                catch (Exception)
                {
                    KucultmeKatsayisi = 1;
                }
                
                for (int x1 = 0; x1 < ResimGenisligi; x1 = x1 + KucultmeKatsayisi)
                {
                    y2 = 0;
                    for (int y1 = 0; y1 < ResimYuksekligi; y1 = y1 + KucultmeKatsayisi)
                    {
                        //x ve y de ilerlerken her atlanan pikselleri okuyacak ve ortalama değerini alacak.
                        R = 0; G = 0; B = 0;
                        try //resim sınırının dışına çıkaldığında hata vermesin diye
                        {
                            for (int i = 0; i < KucultmeKatsayisi; i++)
                            {
                                for (int j = 0; j < KucultmeKatsayisi; j++)
                                {
                                    OkunanRenk = GirisResmi.GetPixel(x1 + i, y1 + j);
                                    R = R + OkunanRenk.R;
                                    G = G + OkunanRenk.G;
                                    B = B + OkunanRenk.B;
                                }
                            }
                        }
                        catch { }
                        //Renk kanallarının ortalamasını alıyor
                        R = R / (KucultmeKatsayisi * KucultmeKatsayisi);
                        G = G / (KucultmeKatsayisi * KucultmeKatsayisi);
                        B = B / (KucultmeKatsayisi * KucultmeKatsayisi);
                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x2, y2, DonusenRenk);
                        y2++;
                    }
                    x2++;
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void dilationYaymaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Yayma, ikili imgedeki nesneyi büyütmeye ya da kalınlaştırmaya yarayan morfolojik 
            //işlemdir.İncelenen alanın sınır bölgelerinin genişletilmesi işlemidir.

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk, OkunanRenk1;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu; //Çekirdek matrisin boyutu
                try
                {
                    SablonBoyutu = Convert.ToInt32(textBox2.Text);
                }
                catch
                {
                    SablonBoyutu = 5;
                }
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x = 0, y = 0, i, j;
                int[] Matris = { 1, 1, 1 };
                int P1, P2, P3;

                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        OkunanRenk1 = GirisResmi.GetPixel(x, y);
                        bool beyazbuldu = false;

                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                if (OkunanRenk.R == 255) //Komşularda Siyah Var ise 
                                    beyazbuldu = true;
                            }
                        }

                        if (beyazbuldu)
                        {
                            CikisResmi.SetPixel(x, y, Color.FromArgb(255, 255, 255));

                        }

                        else CikisResmi.SetPixel(x, y, OkunanRenk1);

                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        public Bitmap OrjinalResimdenGenislemisResmiCikar(Bitmap SiyahBeyazResim, Bitmap GenislemisResim)
        {
            Bitmap CikisResmi;
            int ResimGenisligi = SiyahBeyazResim.Width;
            int ResimYuksekligi = SiyahBeyazResim.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int x, y;
            int Fark;
            for (x = 0; x < ResimGenisligi; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
            {
                for (y = 0; y < ResimYuksekligi; y++)
                {
                    Color OrjinalRenk = SiyahBeyazResim.GetPixel(x, y);
                    Color GenislemisResimRenk = GenislemisResim.GetPixel(x, y);
                    int OrjinalGri = (OrjinalRenk.R + OrjinalRenk.G + OrjinalRenk.B) / 3;
                    int GenislemisGri = (GenislemisResimRenk.R + GenislemisResimRenk.G +
                   GenislemisResimRenk.B) / 3;
                    Fark = Math.Abs(OrjinalGri - GenislemisGri);
                    CikisResmi.SetPixel(x, y, Color.FromArgb(Fark, Fark, Fark));
                }
            }
            return CikisResmi;
        }


        private void openingAçmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void konvolüsyonYöntemiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Aşağıdaki çekirdek matris kullanarak da Netleştirme yapılabilir. Bu matriste temel mantık üzerinde işlem 
            //yapılan pikselin kenarlarındaki 4 tane piksele bakar(Köşelere bakmıyor, sıfırla çarpıyor, onları toplama
            //katmıyor) bu piksellerin değerini aşağıya indirir, üzerinde bulunduğu pikselin değerini yukarı çıkarır.
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 3;
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y, i, j, toplamR, toplamG, toplamB;
                int R, G, B;
                int[] Matris = { 0, -2, 0, -2, 11, -2, 0, -2, 0 };
                int MatrisToplami = 0 + -2 + 0 + -2 + 11 + -2 + 0 + -2 + 0;
                //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        toplamR = 0;
                        toplamG = 0;
                        toplamB = 0;
                        //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                        int k = 0;//matris içindeki elemanları sırayla okurken kullanılacak. ,
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                toplamR = toplamR + OkunanRenk.R * Matris[k];
                                toplamG = toplamG + OkunanRenk.G * Matris[k];
                                toplamB = toplamB + OkunanRenk.B * Matris[k];
                                k++;
                            }
                        }
                        R = toplamR / MatrisToplami;
                        G = toplamG / MatrisToplami;
                        B = toplamB / MatrisToplami;
                        //=========================================
                        //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                        if (R > 255) R = 255;
                        if (G > 255) G = 255;
                        if (B > 255) B = 255;
                        if (R < 0) R = 0;
                        if (G < 0) G = 0;
                        if (B < 0) B = 0;
                        //=========================================
                        CikisResmi.SetPixel(x, y, Color.FromArgb(R, G, B));
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void kenarGörüntüsünüKullanmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Bu algoritma orjinal görüntüden, görüntünün yumuşatılmış halini çıkararak belirgin kenarların
            //görüntüsünü ortaya çıkarır. Daha sonra orjinal görüntü ile belirginleşmiş kenarların görüntüsünü
            //birleştirerek, kenarları keskinleşmiş görüntüyü(netleşmiş görüntü) elde eder. 
            if (pictureBox1.Image == null)
            {   
                MessageBox.Show("Lütfen resim seçiniz", "Dikkat!!!!");
                return;
            }
            Bitmap OrjinalResim = new Bitmap(pictureBox1.Image);

            Bitmap BulanikResim = MeanFiltresi();
            //Bitmap BulanikResim = GaussFiltresi();
            Bitmap KenarGoruntusu = OrjianalResimdenBulanikResmiCikarma(OrjinalResim, BulanikResim);
            Bitmap NetlesmisResim = KenarGoruntusuIleOrjinalResmiBirlestir(OrjinalResim, KenarGoruntusu);
            pictureBox2.Image = NetlesmisResim;
        }
        public Bitmap OrjianalResimdenBulanikResmiCikarma(Bitmap OrjinalResim, Bitmap BulanikResim)
        {
            Color OkunanRenk1, OkunanRenk2, DonusenRenk;
            Bitmap CikisResmi;
            int ResimGenisligi = OrjinalResim.Width;
            int ResimYuksekligi = OrjinalResim.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int R, G, B;
            double Olcekleme = 2; //Keskin kenaları daha iyi görmek için değerini artırıyoruz.
            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk1 = OrjinalResim.GetPixel(x, y);
                    OkunanRenk2 = BulanikResim.GetPixel(x, y);
                    R = Convert.ToInt16(Olcekleme * Math.Abs(OkunanRenk1.R - OkunanRenk2.R));
                    G = Convert.ToInt16(Olcekleme * Math.Abs(OkunanRenk1.G - OkunanRenk2.G));
                    B = Convert.ToInt16(Olcekleme * Math.Abs(OkunanRenk1.B - OkunanRenk2.B));
                    //===============================================================
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak. (Dikkat: Normalizasyon yapılmamıştır. )
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
                    //================================================================
                    DonusenRenk = Color.FromArgb(R, G, B);
                    CikisResmi.SetPixel(x, y, DonusenRenk);
                }
            }
            return CikisResmi;
        }
        public Bitmap KenarGoruntusuIleOrjinalResmiBirlestir(Bitmap OrjinalResim, Bitmap KenarGoruntusu)
        {
            Color OkunanRenk1, OkunanRenk2, DonusenRenk;
            Bitmap CikisResmi;
            int ResimGenisligi = OrjinalResim.Width;
            int ResimYuksekligi = OrjinalResim.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int R, G, B;
            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk1 = OrjinalResim.GetPixel(x, y);
                    OkunanRenk2 = KenarGoruntusu.GetPixel(x, y);
                    R = OkunanRenk1.R + OkunanRenk2.R;

                    G = OkunanRenk1.G + OkunanRenk2.G;
                    B = OkunanRenk1.B + OkunanRenk2.B;
                    //===============================================================
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak. //DİKKAT: Burada sınırı aşan değerler NORMALİZASYON yaparak programlanmalıdır.
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
                    //================================================================
                    DonusenRenk = Color.FromArgb(R, G, B);
                    CikisResmi.SetPixel(x, y, DonusenRenk);
                }
            }
            return CikisResmi;
        }
        public Bitmap MeanFiltresi()
        {
            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);

            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;

            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

            int SablonBoyutu = 0;
            try
            {
                SablonBoyutu = Convert.ToInt16(textBox2.Text); //şablon boyutu 3 den büyük tek rakam olmalıdır (3,5,7 gibi).
            }
            catch (Exception)
            {
                SablonBoyutu = 3;
            }
            
            int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;

            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    toplamR = 0;
                    toplamG = 0;
                    toplamB = 0;

                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x + i, y + j);

                            toplamR = toplamR + OkunanRenk.R;
                            toplamG = toplamG + OkunanRenk.G;
                            toplamB = toplamB + OkunanRenk.B;

                        }
                    }

                    ortalamaR = toplamR / (SablonBoyutu * SablonBoyutu);
                    ortalamaG = toplamG / (SablonBoyutu * SablonBoyutu);
                    ortalamaB = toplamB / (SablonBoyutu * SablonBoyutu);

                    CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                }
            }
            return CikisResmi;
        }

        private void görüntüÖtelemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim seçiniz", "Dikkat!!!!");
                return;
            }
            if (textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Lütfen Değerleri Girin", "Dikkat!!!!");
                return;
            }
            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            double x2 = 0, y2 = 0;
            //Taşıma mesafelerini atıyor.
            int Tx = Convert.ToInt16(textBox2.Text);
            int Ty = Convert.ToInt16(textBox3.Text);
            for (int x1 = 0; x1 < (ResimGenisligi); x1++)
            {
                for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x1, y1);
                    x2 = x1 + Tx;
                    y2 = y1 + Ty;
                    if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                        CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void erosionAşındırmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Aşındırma, ikili imgedeki nesneyi küçültmeye ya da inceltmeye yarayan morfolojik 
            //işlemidir.İncelenen alanın sınır bölgelerinin aşındırması işlemidir. 

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
            else
            {
                Color OkunanRenk, OkunanRenk1;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu;
                try
                {
                    SablonBoyutu = Convert.ToInt32(textBox2.Text);
                }
                catch
                {
                    SablonBoyutu = 5;
                }
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x = 0, y = 0, i, j;

                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        OkunanRenk1 = GirisResmi.GetPixel(x, y);
                        bool siyahbuldu = false;

                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                if (OkunanRenk.R == 0) //Komşularda Siyah Var ise 
                                    siyahbuldu = true;
                            }
                        }
                        if (siyahbuldu)
                        {
                            CikisResmi.SetPixel(x, y, Color.FromArgb(0, 0, 0));

                        }
                        else CikisResmi.SetPixel(x, y, OkunanRenk1);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        
        private void perspektifDüzeltmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Perspektif nesnenin bulunduğu konuma bağlı olarak, gözlemcinin gözünde bıraktığı etkiyi (görüntüyü) 
            //iki boyutlu bir düzlemde canlandırmak için geliştirilmiş bir iz düşüm tekniğidir.Perspektif düzeltmede
            //amaç kişinin veya nesnenin konum değiştirmesi sonucu oluşacak etkiyi düzeltmektir. Bu işlem
            //sayesinde görüntü oluştuktan sonra dahi belirli kısıtlar içerisinde resme baktığımız açıyı değiştirebiliriz. 
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen resim yükleyiniz", "UYARI");
            }
            else
            {
                //Picture Boxta Koyulacak koordinat0
                double X1 = 0;
                double Y1 = 0;

                double X2 = 375;
                double Y2 = 0;

                double X3 = 0;
                double Y3 = 375;

                double X4 = 375;
                double Y4 = 375;

                double[,] GirisMatrisi = new double[8, 8];
                // { x1, y1, 1, 0, 0, 0, -x1 * X1, -y1 * X1 }
                GirisMatrisi[0, 0] = x1;
                GirisMatrisi[0, 1] = y1;
                GirisMatrisi[0, 2] = 1;
                GirisMatrisi[0, 3] = 0;
                GirisMatrisi[0, 4] = 0;
                GirisMatrisi[0, 5] = 0;
                GirisMatrisi[0, 6] = -x1 * X1;
                GirisMatrisi[0, 7] = -y1 * X1;
                //{ 0, 0, 0, x1, y1, 1, -x1 * Y1, -y1 * Y1 }
                GirisMatrisi[1, 0] = 0;
                GirisMatrisi[1, 1] = 0;
                GirisMatrisi[1, 2] = 0;
                GirisMatrisi[1, 3] = x1;
                GirisMatrisi[1, 4] = y1;
                GirisMatrisi[1, 5] = 1;
                GirisMatrisi[1, 6] = -x1 * Y1;
                GirisMatrisi[1, 7] = -y1 * Y1;
                //{ x2, y2, 1, 0, 0, 0, -x2 * X2, -y2 * X2 } 
                GirisMatrisi[2, 0] = x2;
                GirisMatrisi[2, 1] = y2;
                GirisMatrisi[2, 2] = 1;
                GirisMatrisi[2, 3] = 0;
                GirisMatrisi[2, 4] = 0;
                GirisMatrisi[2, 5] = 0;
                GirisMatrisi[2, 6] = -x2 * X2;
                GirisMatrisi[2, 7] = -y2 * X2;
                //{ 0, 0, 0, x2, y2, 1, -x2 * Y2, -y2 * Y2 }
                GirisMatrisi[3, 0] = 0;
                GirisMatrisi[3, 1] = 0;
                GirisMatrisi[3, 2] = 0;
                GirisMatrisi[3, 3] = x2;
                GirisMatrisi[3, 4] = y2;
                GirisMatrisi[3, 5] = 1;
                GirisMatrisi[3, 6] = -x2 * Y2;
                GirisMatrisi[3, 7] = -y2 * Y2;
                //{ x3, y3, 1, 0, 0, 0, -x3 * X3, -y3 * X3 }
                GirisMatrisi[4, 0] = x3;
                GirisMatrisi[4, 1] = y3;
                GirisMatrisi[4, 2] = 1;
                GirisMatrisi[4, 3] = 0;
                GirisMatrisi[4, 4] = 0;
                GirisMatrisi[4, 5] = 0;
                GirisMatrisi[4, 6] = -x3 * X3;
                GirisMatrisi[4, 7] = -y3 * X3;
                //{ 0, 0, 0, x3, y3, 1, -x3 * Y3, -y3 * Y3 }
                GirisMatrisi[5, 0] = 0;
                GirisMatrisi[5, 1] = 0;
                GirisMatrisi[5, 2] = 0;
                GirisMatrisi[5, 3] = x3;
                GirisMatrisi[5, 4] = y3;
                GirisMatrisi[5, 5] = 1;
                GirisMatrisi[5, 6] = -x3 * Y3;
                GirisMatrisi[5, 7] = -y3 * Y3;
                //{ x4, y4, 1, 0, 0, 0, -x4 * X4, -y4 * X4 }
                GirisMatrisi[6, 0] = x4;
                GirisMatrisi[6, 1] = y4;
                GirisMatrisi[6, 2] = 1;
                GirisMatrisi[6, 3] = 0;
                GirisMatrisi[6, 4] = 0;
                GirisMatrisi[6, 5] = 0;
                GirisMatrisi[6, 6] = -x4 * X4;
                GirisMatrisi[6, 7] = -y4 * X4;
                //{ 0, 0, 0, x4, y4, 1, -x4 * Y4, -y4 * Y4 } 
                GirisMatrisi[7, 0] = 0;
                GirisMatrisi[7, 1] = 0;
                GirisMatrisi[7, 2] = 0;
                GirisMatrisi[7, 3] = x4;
                GirisMatrisi[7, 4] = y4;
                GirisMatrisi[7, 5] = 1;
                GirisMatrisi[7, 6] = -x4 * Y4;
                GirisMatrisi[7, 7] = -y4 * Y4;
                double[,] matrisBTersi = MatrisTersiniAl(GirisMatrisi);

                double a00 = 0, a01 = 0, a02 = 0, a10 = 0, a11 = 0, a12 = 0, a20 = 0,a21 = 0, a22 = 0;
                a00 = matrisBTersi[0, 0] * X1 + matrisBTersi[0, 1] * Y1 +
               matrisBTersi[0, 2] *
               X2 + matrisBTersi[0, 3] * Y2 + matrisBTersi[0, 4] * X3 +
               matrisBTersi[0, 5] * Y3 +
                matrisBTersi[0, 6] * X4 + matrisBTersi[0, 7] * Y4;
                a01 = matrisBTersi[1, 0] * X1 + matrisBTersi[1, 1] * Y1 +
               matrisBTersi[1, 2] *
                X2 + matrisBTersi[1, 3] * Y2 + matrisBTersi[1, 4] * X3 +
               matrisBTersi[1, 5] * Y3 +
                matrisBTersi[1, 6] * X4 + matrisBTersi[1, 7] * Y4;
                a02 = matrisBTersi[2, 0] * X1 + matrisBTersi[2, 1] * Y1 +
               matrisBTersi[2, 2] *
                X2 + matrisBTersi[2, 3] * Y2 + matrisBTersi[2, 4] * X3 +
               matrisBTersi[2, 5] * Y3 +
                matrisBTersi[2, 6] * X4 + matrisBTersi[2, 7] * Y4;
                a10 = matrisBTersi[3, 0] * X1 + matrisBTersi[3, 1] * Y1 +
               matrisBTersi[3, 2] *
                X2 + matrisBTersi[3, 3] * Y2 + matrisBTersi[3, 4] * X3 +
               matrisBTersi[3, 5] * Y3 +
                matrisBTersi[3, 6] * X4 + matrisBTersi[3, 7] * Y4;
                a11 = matrisBTersi[4, 0] * X1 + matrisBTersi[4, 1] * Y1 +
               matrisBTersi[4, 2] *
                X2 + matrisBTersi[4, 3] * Y2 + matrisBTersi[4, 4] * X3 +
               matrisBTersi[4, 5] * Y3 +
                matrisBTersi[4, 6] * X4 + matrisBTersi[4, 7] * Y4;
                a12 = matrisBTersi[5, 0] * X1 + matrisBTersi[5, 1] * Y1 +
               matrisBTersi[5, 2] *
                X2 + matrisBTersi[5, 3] * Y2 + matrisBTersi[5, 4] * X3 +
               matrisBTersi[5, 5] * Y3 +
                matrisBTersi[5, 6] * X4 + matrisBTersi[5, 7] * Y4;
                a20 = matrisBTersi[6, 0] * X1 + matrisBTersi[6, 1] * Y1 +
               matrisBTersi[6, 2] *
                X2 + matrisBTersi[6, 3] * Y2 + matrisBTersi[6, 4] * X3 +
               matrisBTersi[6, 5] * Y3 +
                matrisBTersi[6, 6] * X4 + matrisBTersi[6, 7] * Y4;
                a21 = matrisBTersi[7, 0] * X1 + matrisBTersi[7, 1] * Y1 +
               matrisBTersi[7, 2] * X2 + matrisBTersi[7, 3] * Y2 + matrisBTersi[7, 4] * X3 +
               matrisBTersi[7, 5] * Y3 + matrisBTersi[7, 6] * X4 + matrisBTersi[7, 7] * Y4;
                a22 = 1;
                //------------------------- Perspektif düzeltme işlemi ------------------------

                PerspektifDuzelt(a00, a01, a02, a10, a11, a12, a20, a21, a22);
                sayac = 1;
                x1 = -1;
                y1 = -1;
                x2 = -1;
                y2 = -1;
                x3 = -1;
                y3 = -1;
                x4 = -1;
                y4 = -1;
            }
        }
        //================== Perspektif düzeltme işlemi =================
        public void PerspektifDuzelt(double a00, double a01, double a02,
       double a10, double a11, double a12, double a20,
       double a21, double a22)
        {
            Bitmap GirisResmi, CikisResmi;
            Color OkunanRenk;
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            double X, Y, z;
            for (int x = 0; x < (ResimGenisligi); x++)
            {
                for (int y = 0; y < (ResimYuksekligi); y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    z = a20 * x + a21 * y + 1;
                    X = (a00 * x + a01 * y + a02) / z;
                    Y = (a10 * x + a11 * y + a12) / z;
                    if (X > 0 && X < ResimGenisligi && Y > 0 && Y < ResimYuksekligi)
                        //Picturebox ın dışına çıkan kısımlar oluşturulmayacak.
                        CikisResmi.SetPixel((int)X, (int)Y, OkunanRenk);
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        // MATRİS TERSİNİ ALMA---------------------
        public double[,] MatrisTersiniAl(double[,] GirisMatrisi)
        {
            int MatrisBoyutu = Convert.ToInt16(Math.Sqrt(GirisMatrisi.Length));
            //matris boyutu içindeki eleman sayısı olduğu için kare matrisde 
            //karekökü matris boyutu olur.
            double[,] CikisMatrisi = new double[MatrisBoyutu, MatrisBoyutu]; //A nın 
                                                                             //tersi alındığında bu matris içinde tutulacak
                                                                             //--I Birim matrisin içeriğini dolduruyor 
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (i == j)
                        CikisMatrisi[i, j] = 1;
                    else
                        CikisMatrisi[i, j] = 0;
                }
            }
            //--Matris Tersini alma işlemi---------
            double d, k;
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                d = GirisMatrisi[i, i];
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (d == 0)
                    {
                        d = 0.0001; //0 bölme hata veriyordu. 
                    }
                    GirisMatrisi[i, j] = GirisMatrisi[i, j] / d;
                    CikisMatrisi[i, j] = CikisMatrisi[i, j] / d;
                }
                for (int x = 0; x < MatrisBoyutu; x++)
                {
                    if (x != i)
                    {
                        k = GirisMatrisi[x, i];
                        for (int j = 0; j < MatrisBoyutu; j++)
                        {
                            GirisMatrisi[x, j] = GirisMatrisi[x, j] - GirisMatrisi[i, j] * k;
                            CikisMatrisi[x, j] = CikisMatrisi[x, j] - CikisMatrisi[i, j] * k;
                        }
                    }
                }
            }
            return CikisMatrisi;
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            if (pictureBox1.Image != null)
            {
                DialogResult dialog = new DialogResult();
                dialog = MessageBox.Show("X: " + coordinates.X + " Y: " + coordinates.Y + "\nDeğerleri atansın mı?", "Uyarı", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (sayac == 1)
                    {
                        x1 = coordinates.X;
                        y1 = coordinates.Y;
                        sayac++;
                        label2.Text = coordinates.ToString();
                    }
                    else if (sayac == 2)
                    {
                        x2 = coordinates.X;
                        y2 = coordinates.Y;
                        sayac++;
                    }
                    else if (sayac == 3)
                    {
                        x3 = coordinates.X;
                        y3 = coordinates.Y;
                        sayac++;
                    }
                    else if (sayac == 4)
                    {
                        x4 = coordinates.X;
                        y4 = coordinates.Y;
                        sayac++;
                    }
                    try
                    {
                        if (sayac == 5) perspektifDüzeltmeToolStripMenuItem_Click(sender, e);
                    }
                    catch { }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void gradyentFiltresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int x, y;
            Color Renk;
            int P1, P2, P3, P4;
            for (x = 0; x < ResimGenisligi - 1; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
            {
                for (y = 0; y < ResimYuksekligi - 1; y++)
                {
                    Renk = GirisResmi.GetPixel(x, y);
                    P1 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y);
                    P2 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y + 1);
                    P3 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y + 1);
                    P4 = (Renk.R + Renk.G + Renk.B) / 3;
                    int Gx = Math.Abs(P1 - P4); //45 derece açı ile duran çizgileri bulur.
                    int Gy = Math.Abs(P2 - P3); //135 derece açı ile duran çizgileri bulur.
                    int RobertCrossDegeri = 0;
                    RobertCrossDegeri = Gx;
                    RobertCrossDegeri = Gy;
                    RobertCrossDegeri = Gx + Gy; //1. Formül
                    if (RobertCrossDegeri > 255) RobertCrossDegeri = 255; //Mutlak değer kullanıldığı içinnegatif değerler oluşmaz.
                                                     
                    CikisResmi.SetPixel(x, y, Color.FromArgb(RobertCrossDegeri, RobertCrossDegeri,
                   RobertCrossDegeri));
                }
            }
            pictureBox2.Image = CikisResmi;
        }
    }
}
