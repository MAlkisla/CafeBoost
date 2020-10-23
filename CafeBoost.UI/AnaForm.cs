using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CafeBoost.Data;
using CafeBoost.UI.Properties;

namespace CafeBoost.UI
{
    public partial class AnaForm : Form
    {
        int masaAdet = 20;
        KafeVeri db = new KafeVeri();


        public AnaForm()
        {
            InitializeComponent();
            OrnekUrunleriYukle();
            MasalariOlustur();
        }

        private void OrnekUrunleriYukle()
        {
            db.Urunler.Add(new Urun
            {
                UrunAd = "Kola",
                BirimFiyat = 6m
            });
            db.Urunler.Add(new Urun
            {
                UrunAd = "Ayran",
                BirimFiyat = 4m
            });
        }

        private void MasalariOlustur()
        {
            #region İmaj Listesinin Hazırlanması
            ImageList il = new ImageList();
            il.Images.Add("bos", Resources.Bos);
            il.Images.Add("dolu", Resources.Dolu);
            il.ImageSize = new Size(64, 64);
            lvwMasalar.LargeImageList = il;
            #endregion
            #region Masaların Oluşturulması
            ListViewItem lvi;
            for (int i = 1; i <= masaAdet; i++)
            {
                lvi = new ListViewItem("Masa " + i);
                lvi.ImageKey = "bos";
                lvi.Tag = i; // masaların ismi ilerde değişirse kodumuz patlamasın diye kod veriyoruz
                lvwMasalar.Items.Add(lvi);
            }
            #endregion
        }

        private void tsmiUrunler_Click(object sender, EventArgs e)
        {
            new UrunlerForm().ShowDialog();
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            new GecmisSiparislerForm(db).ShowDialog();
        }

        private void lvwMasalar_DoubleClick(object sender, EventArgs e)
        {
            int masaNo = (int)lvwMasalar.SelectedItems[0].Tag;
            Siparis siparis = AktifSiparisBul(masaNo);

            if (siparis == null)
            {
                siparis = new Siparis();
                siparis.MasaNo = masaNo;
                db.AktifSiparisler.Add(siparis);
                lvwMasalar.SelectedItems[0].ImageKey = "dolu";
            }
            SiparisForm frmSiparis = new SiparisForm(db, siparis);
            DialogResult dr = frmSiparis.ShowDialog();

            if (dr == DialogResult.OK) // (
            {
                lvwMasalar.SelectedItems[0].ImageKey = "bos";
            }
        }

        private Siparis AktifSiparisBul(int masaNo)
        {
            return db.AktifSiparisler.FirstOrDefault(x => x.MasaNo == masaNo);

            #region Yöntem1 : Foreach
            //foreach (Siparis item in db.AktifSiparisler)
            //{
            //    if (item.MasaNo == masaNo)
            //    {
            //        return item;
            //    }
            //}
            //return null; 
            #endregion
        }
    }
}
