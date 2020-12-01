using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace CafeBoost.Data
{
    public class CafeBoostContext : DbContext
    {
        public CafeBoostContext() : base("name=CafeBoostContext")
        {
            // output penceresinde çalışan sorguları göster
            // https://stackoverflow.com/questions/1412863/how-do-i-view-the-sql-generated-by-the-entity-framework
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<SiparisDetay>()
                .HasRequired(x => x.Urun)
                .WithMany(x => x.siparisDetaylar)
                .HasForeignKey(x => x.UrunId)
                .WillCascadeOnDelete(false);
        }

        public int MasaAdet { get; set; } = 20; //default değer
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylar { get; set; }
    }
}
