namespace BirKadınBirErkekTasarımMerkezi.Models
{
    public class Randevu
    {
        public int Id {  get; set; }
        public string iletisimNumaraniz {  get; set; }
        public string? kullaniciID {  get; set; }

        public int IslemlerID {  get; set; }
        public Islemler Islemler { get; set; }

        public int UstalarID {  get; set; }
        public Ustalar Ustalar { get; set;}

        public DateTime Zaman {  get; set; }
        public bool Onay {  get; set; }
    }
}
