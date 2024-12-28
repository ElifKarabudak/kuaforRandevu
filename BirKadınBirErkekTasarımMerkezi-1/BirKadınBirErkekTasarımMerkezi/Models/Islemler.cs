namespace BirKadınBirErkekTasarımMerkezi.Models
{
    public class Islemler
    {
        public int ID { get; set; }
        public string Ad { get; set; }
        public int Sure { get; set; }
        public int Ucret {  get; set; }
        public int KisimID {  get; set; }
        public Kisim Kisim { get; set;}
    }
}
