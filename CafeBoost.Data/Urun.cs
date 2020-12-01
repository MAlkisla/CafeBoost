using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CafeBoost.Data
{
    [Table("Urunler")]
    public class Urun
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string UrunAd { get; set; }

        public decimal BirimFiyat { get; set; }
        public override string ToString()
        {
            return ($"{UrunAd} ({BirimFiyat}) ₺");
        }
        public ICollection<SiparisDetay> siparisDetaylar { get; set; }
    }
}
