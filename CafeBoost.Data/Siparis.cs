using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CafeBoost.Data
{
    public class Siparis
    {
        public List<SiparisDetay> SiparisDetaylar { get; set; }
        public int MasaNo { get; set; }
        public DateTime? AcilisZamani { get; set; }
        public DateTime? KapanisZamani { get; set; }
        public SiparisDurum Durum { get; set; }
        public decimal OdenenTutar { get; set; }
        public string ToplamTutarTL { get { return $"{ToplamTutar():0.00}₺"; } } // => ToplamTutar() + "TL";

        public Siparis()
        {
            SiparisDetaylar = new List<SiparisDetay>(); 
            AcilisZamani = DateTime.Now; //başlangıç saati girmene gerek kalmaz. Boş olmasın diye
        }
        public decimal ToplamTutar()
        {
            return SiparisDetaylar.Sum(x => x.Tutar());
        }
        //public decimal ToplamTutar()
        //{
        //    decimal toplam = 0;
        //    foreach (var item in SiparisDetaylar)
        //    {
        //        toplam += item.Adet * item.BirimFiyat;
        //    }
        //    return toplam;
        //}
    }
}
