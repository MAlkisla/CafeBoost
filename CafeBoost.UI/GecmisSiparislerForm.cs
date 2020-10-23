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
    public partial class GecmisSiparislerForm : Form
    {
        private readonly KafeVeri db;

        public GecmisSiparislerForm(KafeVeri kafeVeri)
        {
            db = kafeVeri;
            InitializeComponent();
            dgvSiparisler.DataSource = db.GecmisSiparisler;
        }

        private void dgvSiparisler_SelectionChanged(object sender, EventArgs e)
        {
            // en az bir secili ürün var ise
            if(dgvSiparisler.SelectedRows.Count > 0)
            {
                // secili satırların ilkinin üzerindeki siparis nesnesi
                Siparis seciliSiparis = (Siparis)dgvSiparisler.SelectedRows[0].DataBoundItem;
                dgvSiparisDetaylar.DataSource = seciliSiparis.SiparisDetaylar;
            }
        }
    }
}
