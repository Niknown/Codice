using System;
using System.Collections.Generic;
namespace Gioco
{
    /// <summary>
    /// classe usata per le stanze nel gioco.
    /// </summary>
    public class Stanza
    {
        public string Nome { get; set; }
        public string Descrizione { get; set; }    
        public List<Oggetto> Oggetti { get; set; }        
        public List<Personaggio> Personaggi { get; set; }    
        public bool Visitata { get; set; }    
        public bool RichiedeOggetto { get; set; }
        public bool Aperta { get; set; }        
        public Oggetto OggettoRichiesto { get; set;}
        public Stanza(string nome, string descrizione, bool richiedeOggetto, bool aperta)
        {
            Nome = nome;
            Descrizione = descrizione;
            Oggetti = new List<Oggetto>();
            Personaggi = new List<Personaggio>();
            RichiedeOggetto = richiedeOggetto;
            Aperta = aperta;
        }
        /// <summary>
        /// Metodo per creare un numero casuale da abbinare a una stanza.
        /// </summary>
        /// <returns>spawnPg</returns>
        public static int SpawnRandom()
        {
            int spawnPg = 0;
            Random random = new Random();
            spawnPg = random.Next(1,5);
            switch(spawnPg)
            {
                case 1:
                    spawnPg = 1;
                    break;
                case 2:
                    spawnPg = 2;
                    break;
                case 3:
                    spawnPg = 3;
                    break;
                case 4:
                    spawnPg = 4;
                    break;    
            }
            return spawnPg;
        }
            /// <summary>
            /// Metodo per rilasciare gli oggetti nella stanza in cui ci troviamo.
            /// </summary>
            /// <param name="oggetto"></param>
            /// <param name="inventario"></param>
        public void ReleaseItem(Oggetto oggetto, List<Oggetto> inventario)
        {
            if (inventario.Contains(oggetto))
            {
                inventario.Remove(oggetto);
                Oggetti.Add(oggetto);
                PrintSlowly("Hai rilasciato l'oggetto: " + oggetto.Nome);
            }
            else
            {
                PrintSlowly("L'oggetto specificato non è presente nell'inventario.");
            }
        }
        /// <summary>
        /// Metodo per impostare la porta su aperta = true.
        /// </summary>
           public void OpenDoor()
        {
            Aperta = true;
            PrintSlowly($"Hai aperto la porta con successo con {OggettoRichiesto.Nome}!");
        }
        /// <summary>
        /// Metodo per mostrare la descrizione della stanza la prima volta che entriamo.
        /// </summary>
        public void ShowDescription()
        {
            PrintSlowly(Descrizione);
        } 
        /// <summary>
        /// Metodo per aggiungere un personaggio ad una stanza.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="descrizione"></param>
        /// <param name="discorso"></param>
        /// <param name="discorsoSi"></param>
        /// <param name="oggetto"></param>
        public void AddNpc(string nome, string descrizione, string discorso, bool discorsoSi, Oggetto oggetto)
        {
            Personaggi.Add(new Personaggio(nome, descrizione, discorso, discorsoSi, oggetto));
        }
        /// <summary>
        /// Metodo per aggiungere un oggetto ad una stanza.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="descrizione"></param>
        /// <param name="peso"></param>
        /// <param name="raccoglibile"></param>
        public void AddItem(string nome, string descrizione, int peso, bool raccoglibile)
        {
            Oggetti.Add(new Oggetto(nome, descrizione, peso, raccoglibile));
        }
        /// <summary>
        /// Metodo per esaminare gli oggetti nella stanza corrente.
        /// </summary>
        public void ShowRoomItems()
        {
            PrintSlowly("Gli oggetti in questa stanza sono:");
            foreach (Oggetto oggetto in Oggetti)
            {
                PrintSlowly("- " + oggetto.Nome + ": " + oggetto.Descrizione);
            }
        }
        /// <summary>
        /// Metodo per raccogliere gli oggetti presenti nella stanza corrente.
        /// </summary>
        /// <param name="oggetto"></param>
        /// <param name="inventario"></param>
        public void PickUpItems(Oggetto oggetto, List<Oggetto> inventario)
        {
            if (oggetto.Raccoglibile)
            {
                Oggetti.Remove(oggetto);
                inventario.Add(oggetto);
                PrintSlowly("Hai ottenuto l'oggetto: " + oggetto.Nome + " Peso: " + oggetto.Peso);
            }
            else
            {
                PrintSlowly("Non puoi ottenere questo oggetto.");
            }
        }
        /// <summary>
        /// Metodo per esaminare i personaggi presenti nella stanza corrente.
        /// </summary>
        public void ShowRoomNpcs()
        {
            PrintSlowly("I personaggi in questa stanza sono:");
            foreach (Personaggio personaggio in Personaggi)
            {
                PrintSlowly("-" + personaggio.Nome);
            }
            PrintSlowly("Scrivi (parla) per interagire con il personaggio desiderato.");
        }
        /// <summary>
        /// Metodo per parlare con i personaggi presenti nella stanza corrente se hanno un discorso da fare.
        /// </summary>
        /// <param name="personaggio"></param>
        public void TalkToNpcs(Personaggio personaggio)
        {
            if (personaggio.DiscorsoSi)
            {
                PrintSlowly(personaggio.Discorso);
                personaggio.DiscorsoSi = false;
            }
            else
            {
                PrintSlowly(personaggio.Descrizione + "\n");
                PrintSlowly("Hai già parlato con questo personaggio.");
            }
        }
            /// <summary>
            /// Metodo per stampare caratteri lentamente.
            /// </summary>
            /// <param name="text"></param>
            /// <param name="speed"></param>
            public static void PrintSlowly(string text, int speed = 1)
        {
            foreach(char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(speed);
            }
            Console.WriteLine();
        }
    }
}