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
    public partial class UrunlerForm : Form
    {
        private readonly CafeBoostContext db;
        BindingList<Urun> blUrunler;

        public UrunlerForm(CafeBoostContext cafeBoostContext)
        {
            InitializeComponent();
            db = cafeBoostContext;
            blUrunler = new BindingList<Urun>(db.Urunler.ToList());
            dgvUrunler.DataSource = blUrunler;
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            string urunAd = txtUrunAdi.Text.Trim();

            if (urunAd == string.Empty)
            {
                errorProvider1.SetError(txtUrunAdi, "Ürün adı girmediniz.");
                return;
            }

            if (UrunVarMi(urunAd))
            {
                errorProvider1.SetError(txtUrunAdi, "Ürün zaten tanımlı.");
                return;
            }

            errorProvider1.SetError(txtUrunAdi, "");

            blUrunler.Add(new Urun() 
            {
                UrunAd = urunAd, 
                BirimFiyat = nudBirimFiyat.Value 
            });

            txtUrunAdi.Clear();
            nudBirimFiyat.Value = 0;
        }

        private void dgvUrunler_Validating(object sender, CancelEventArgs e)
        {
            if (txtUrunAdi.Text.Trim()== "")
            {
                errorProvider1.SetError(txtUrunAdi, "Ürün adı girmediniz");
            }
            else
            {
                errorProvider1.SetError(txtUrunAdi, "");
            }

            //if (txtUrunAdi.Text.Trim() !="")
            //{
            //    errorProvider1.SetError(txtUrunAdi, "");
            //}
        }

        private void dgvUrunler_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            Urun urun = (Urun)dgvUrunler.Rows[e.RowIndex].DataBoundItem;
            string mevcutDeger = dgvUrunler.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (!dgvUrunler.IsCurrentCellDirty || e.FormattedValue.ToString() == mevcutDeger)
            {
                return;
            }

            if (e.ColumnIndex == 0)
            {
                if (e.FormattedValue.ToString() == "")
                {
                    MessageBox.Show("Ürün adı boş bırakılamaz.");
                    e.Cancel = true;
                }

                if (BaskaUrunVarMi(e.FormattedValue.ToString(), urun))
                {
                    MessageBox.Show("Bu ürün zaten mevcut.");
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == 1)
            {
                decimal birimFiyat;
                bool gecerliMi =decimal.TryParse(e.FormattedValue.ToString(), out birimFiyat);

                if (!gecerliMi || birimFiyat < 0)
                {
                    MessageBox.Show("Geçersiz fiyat");
                    e.Cancel = true;
                }
            }
        }

        private bool UrunVarMi(string urunAd)
        {
            return db.Urunler.Any(x => x.UrunAd.Equals(urunAd, StringComparison.CurrentCultureIgnoreCase));  // hiç varmı
        }

        private bool BaskaUrunVarMi(string urunAd,Urun urun)
        {
            return db.Urunler.Any(x => x.UrunAd.Equals(urunAd, StringComparison.CurrentCultureIgnoreCase)&& x != urun);  
        }
    }
}
