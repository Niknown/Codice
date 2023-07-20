using System;

namespace Gioco
{
    /// <summary>
    /// classe usata per i personaggi nel gioco.
    /// </summary>
    public class Personaggio
    {  
        public string Nome { get; set; }       
        public string Descrizione { get; set; }
        public bool DiscorsoSi { get; set; }
        public string Discorso { get; set; } 
        public Oggetto OggettoAssegnato { get; set; }
        public Personaggio(string nome, string descrizione, string discorso, bool discorsoSi, Oggetto oggettoAssegnato)
        {
            Nome = nome;
            Descrizione = descrizione;
            Discorso = discorso;
            DiscorsoSi = discorsoSi;
            OggettoAssegnato = oggettoAssegnato;
        }
    }
}