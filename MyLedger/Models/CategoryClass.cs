namespace MyLedger.Models
{
    public class CategoryClass
    {
        public int categorycount { get; set; }       // Kategorideki işlem sayısı
        public string categoryname { get; set; }     // Kategori adı
        public decimal categorytotal { get; set; }   // Kategorinin toplam harcama tutarı
        public decimal percentage { get; set; }      // Kategorinin toplam harcamadaki oranı
    }
}
