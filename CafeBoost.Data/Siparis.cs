using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CafeBoost.Data
{
    [Table("Siparisler")]
    public class Siparis
    {
        public int Id { get; set; }
        public int MasaNo { get; set; }
        public DateTime? AcilisZamani { get; set; }
        public DateTime? KapanisZamani { get; set; }
        public SiparisDurum Durum { get; set; }
        public decimal OdenenTutar { get; set; }
        public string ToplamTutarTL { get { return $"{ToplamTutar():0.00}₺"; } } // => ToplamTutar() + "TL";

        public virtual ICollection<SiparisDetay> SiparisDetaylar { get; set; }
        public Siparis()
        {
            SiparisDetaylar = new HashSet<SiparisDetay>(); 
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
