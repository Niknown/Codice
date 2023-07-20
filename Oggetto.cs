namespace Gioco
{
    /// <summary>
    /// classe usata per gli oggetti nel gioco.
    /// </summary>
    public class Oggetto
    {
        public string Nome { get; set; }
        public string Descrizione { get; set; }    
        public int Peso { get; set; }   
        public bool Raccoglibile { get; set; }
        public Oggetto(string nome, string descrizione, int peso, bool raccoglibile)
        {
            Nome = nome;
            Descrizione = descrizione;
            Peso = peso;
            Raccoglibile = raccoglibile;
        }
    }  
}