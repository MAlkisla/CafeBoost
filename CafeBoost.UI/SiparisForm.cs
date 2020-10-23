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

namespace CafeBoost.UI
{
    public partial class SiparisForm : Form
    {
        public event EventHandler<MasaTasimaEventArgs> MasaTasindi;
        private readonly KafeVeri db;
        private readonly Siparis siparis;
        private readonly AnaForm anaForm;
        private readonly BindingList<SiparisDetay> blsiparisDetaylar;

        public SiparisForm(KafeVeri kafeVeri, Siparis siparis)
        {
            db = kafeVeri;
            this.siparis = siparis; //this class seviyesinde ki siparisi kullanır
            InitializeComponent();
            dgvSiparisDetaylar.AutoGenerateColumns = false;
            MasalariListele();
            UrunleriListele();
            MasaNoGuncelle();
            OdemeTutariGuncelle();

            blsiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar); // list haber vermio binding list değişiklikleri haber verir.
            blsiparisDetaylar.ListChanged += BlsiparisDetaylar_ListChanged;
            dgvSiparisDetaylar.DataSource = blsiparisDetaylar;
        }

        private void MasalariListele()
        {
            cboMasalar.Items.Clear();

            for (int i = 1; i <= db.MasaAdet; i++)
            {
                if (!db.AktifSiparisler.Any(x => x.MasaNo == i)) // sınav sorusu
                {
                    cboMasalar.Items.Add(i);
                }

            }
        }

        private void MasaNoGuncelle()
        {
            Text = $"Masa {siparis.MasaNo:00} - Sipariş Detayları (Açılış: {siparis.AcilisZamani.Value.ToShortTimeString()})";
            lblMasaNo.Text = siparis.MasaNo.ToString("00");
        }

        private void UrunleriListele() //F4 ten dropdown list yap
        {
            cboUrun.DataSource = db.Urunler;
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            Urun secilenUrun = (Urun)cboUrun.SelectedItem;
            int adet = (int)nudAdet.Value;
            //SiparisDetay detay = blsiparisDetaylar.FirstOrDefault(x => x.UrunAd == secilenUrun.UrunAd);

            //if (detay !=null)
            //{
            //    detay.Adet += adet;
            //    blsiparisDetaylar.ResetBindings();
            //}
            //else                                                        // ekle butonuna bastığında var olanın üzerine eklemesi için  o yoksa ekle varsa üzerine ekle kodu
            //{
            //    detay = new SiparisDetay()
            //    {
            //        UrunAd = secilenUrun.UrunAd,
            //        BirimFiyat = secilenUrun.BirimFiyat,
            //        Adet = adet
            //    };
            //    blsiparisDetaylar.Add(detay);
            //}

            SiparisDetay detay = new SiparisDetay()
            {
                UrunAd = secilenUrun.UrunAd,
                BirimFiyat = secilenUrun.BirimFiyat,
                Adet = adet
            };
            blsiparisDetaylar.Add(detay);
        }

        private void BlsiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        {
            OdemeTutariGuncelle();
        }

        private void OdemeTutariGuncelle()
        {
            lblOdemeTutari.Text = siparis.ToplamTutarTL;
        }

        private void dgvSiparisDetaylar_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seçili detayları silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void btnAnasayfa_Click(object sender, EventArgs e)
        {
            //Close();
            DialogResult = DialogResult.Cancel;
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Siparis iptal edilerek masa kapatılacaktır.Emin misiniz?", "İptal Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                SiparisKapat(SiparisDurum.Iptal);
            }
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Ödeme alındıysa masa kapatılacaktır.Emin misiniz?", "Ödeme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                SiparisKapat(SiparisDurum.Odendi, siparis.ToplamTutar());
            }
        }

        private void SiparisKapat(SiparisDurum siparisDurum, decimal odenenTutar = 0)
        {
            siparis.OdenenTutar = odenenTutar;
            siparis.KapanisZamani = DateTime.Now;
            siparis.Durum = siparisDurum;
            db.AktifSiparisler.Remove(siparis);
            db.GecmisSiparisler.Add(siparis);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
            if (cboMasalar.SelectedIndex < 0) return;
            int hedef = (int)cboMasalar.SelectedItem;
            int kaynak = siparis.MasaNo;
            siparis.MasaNo = hedef;
            MasaNoGuncelle();
            MasalariListele();

            MasaTasimaEventArgs args = new MasaTasimaEventArgs()
            {
                EskiMasaNo = kaynak,
                YeniMasaNo = hedef
            };
            MasaTasindiginda(args);


        }

        protected virtual private void MasaTasindiginda(MasaTasimaEventArgs args)
        {
            if (MasaTasindi != null)
            {
                MasaTasindi(this, args);
            }
        }
    }
}