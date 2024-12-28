namespace BirKadınBirErkekTasarımMerkezi.Models
{
    public class UstalarinYapabildigiIslemler
    {
        public int Id {  get; set; }
        
        public int UstalarID {  get; set; }
        public Ustalar Ustalar { get; set; }

        public int IslemlerID {  get; set; }
        public Islemler Islemler { get; set; }  
    }
}
