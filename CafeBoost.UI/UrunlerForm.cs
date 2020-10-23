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
        private readonly KafeVeri db;
        BindingList<Urun> blUrunler;

        public UrunlerForm(KafeVeri kafeVeri)
        {
            InitializeComponent();
            db = kafeVeri;
            blUrunler = new BindingList<Urun>(db.Urunler);
            dgvUrunler.DataSource = blUrunler;
        }
    }
}
