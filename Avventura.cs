using log4net;
using Newtonsoft.Json;
using System.Reflection;
using System.Configuration;
using System.Timers;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Gioco
{      
    /// <summary>
    /// Classe principale del programma
    /// </summary>
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static System.Timers.Timer aTimer = new System.Timers.Timer(5000);

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        _log.Info("Timeout Reached, closing the application");
        aTimer.Stop();
        Environment.Exit(1);
    }
        static bool giocoNuovo = true;
        static Coordinate coordinateStart = new Coordinate(0, 0);
        static Coordinate coordinateHolmes = new Coordinate(1, 0);
        static Coordinate coordinatePranzo = new Coordinate(-1, 0);
        static Coordinate coordinateReception = new Coordinate(0, 1);
        static Coordinate coordinateCucina = new Coordinate(-1, 1);
        static Coordinate coordinatePlayer = new Coordinate(1, 1);
        static Coordinate coordinateSoggiorno = new Coordinate(0, 2);
        static Coordinate coordinateManutenzione = new Coordinate(-1, 2);
        static Coordinate coordinatePoliziotto = new Coordinate(1, 2);
        static Coordinate coordinateTorture = new Coordinate(1, -1);
        static Coordinate posizioneCorrente = new Coordinate(0, 0);
        static Stanza stanzaStart = new Stanza("Ingresso", "Appena entrato nell'hotel noti che è un hotel modesto, alcune piante e un tappeto mostrano la via per la reception davanti a te.", false, true);
        static Stanza stanzaHolmes = new Stanza("Stanza di H.Holmes", "Trovi la stanza in pieno disordine, pare sia passato un uragano, vedi una statua di marmo a pezzi vicino alla scrivania, fogli e libri ovunque, è sicuramente successo qualcosa...", true, false);
        static Stanza stanzaPlayer = new Stanza("La tua stanza", "Entrando nella stanza trovi i tuoi vestiti da lavoro sul letto, è una stanza luminosa ma molto piccola, ma ha tutto l'occorrente per un soggiorno lavorativo.", true, false);
        static Stanza stanzaReception = new Stanza("Reception", "All'arrivo in reception vi trovate accolti da un ambiente accogliente, una signora si trova alla scrivania, ma sta parlando con un signore dalla faccia conosciuta.", false, true);
        static Stanza stanzaPranzo = new Stanza("Sala da pranzo", "La sala da pranzo ha pochi tavoli disposti in fila, da qui puoi intravedere la cucina, il poliziotto sta mangiando seduto ad un tavolo..", true, false);
        static Stanza stanzaCucina = new Stanza("Cucina", "Un profumo delizioso ti accoglie in cucina, sui fuochi vi è appoggiato un pentolone pieno di sugo che sta cuocendo, ma non c'è nessuno al momento..", true, false);
        static Stanza stanzaManutenzione = new Stanza("Stanza Lavanderia/Manutenzione", "uno sgabuzzino abbastanza angusto, qui vengono tenuti tutti gli utensili dell'hotel e la centralina elettrica generale.", true, false);
        static Stanza stanzaPoliziotto = new Stanza("Stanza del poliziotto", "Quando entri nella stanza capisci che qualcosa non va, dalle coperte alla scrivania è tutto in disordine, deve essere successo qualcosa...", true, false);
        static Stanza stanzaTorture = new Stanza("Stanza torture", "Una volta appoggiato il batticarne e la statuetta sulla bilancia la libreria inizia a girare e trovi un'altra stanza. Entrando nella stanza segreta trovi Holmes che sta facendo a pezzi il poliziotto, che decidi di fare?", true, false);
        static Stanza stanzaSoggiorno = new Stanza("Soggiorno", "Il soggiorno è un ambiente tranquillo, ci sono delle poltrone e dei divani a contornarlo con un camino attaccato alla parete, delle decorazioni con animali imbalsamati sulle pareti.", false, true);
        static Dictionary<Coordinate, Stanza> stanze = new Dictionary<Coordinate, Stanza>();
        static List<Oggetto> inventario = new List<Oggetto>();
        static int pesoLimiteInventario = 10; // Limite di peso dell'inventario

    /// <summary>
    /// Classe per il salvataggio e caricamento del gioco.
    /// </summary>
    public class GameSaveData
    {
        public List<Oggetto> Inventario { get; set; }
        public Coordinate PosizioneCorrente { get; set; }
        public Stanza StanzaStart { get; set; }
        public Stanza StanzaHolmes { get; set; }
        public Stanza StanzaPranzo { get; set; }
        public Stanza StanzaReception { get; set; }
        public Stanza StanzaCucina { get; set; }
        public Stanza StanzaPlayer { get; set; }
        public Stanza StanzaSoggiorno { get; set; }
        public Stanza StanzaManutenzione { get; set; }
        public Stanza StanzaPoliziotto { get; set; }
        public Stanza StanzaTorture { get; set; }

    }
    /// <summary>
    /// Metodo per serializzare e quindi salvare lo stato del gioco.
    /// </summary>
    /// <param name="fileName"></param>
    private static void SaveGame(string fileName)
    {
        GameSaveData saveData = new GameSaveData
        {
            PosizioneCorrente = posizioneCorrente,
            Inventario = inventario,
            StanzaStart = stanzaStart,
            StanzaHolmes = stanzaHolmes,
            StanzaPranzo = stanzaPranzo,
            StanzaCucina = stanzaCucina,
            StanzaReception = stanzaReception,
            StanzaPlayer = stanzaPlayer,
            StanzaPoliziotto = stanzaPoliziotto,
            StanzaSoggiorno = stanzaSoggiorno,
            StanzaManutenzione = stanzaManutenzione,
            StanzaTorture = stanzaTorture,

        };

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(fileName, json);
        Logger.Log("Hai salvato la partita correttamente.");
        PrintSlowly("Il gioco è stato salvato correttamente.");
    }
    /// <summary>
    /// Metodo per deserializzare e quindi caricare lo stato del gioco.
    /// </summary>
    /// <param name="fileName"></param>
    private static void LoadGame(string fileName)
    {
        if (File.Exists(fileName))
        {
            string json = File.ReadAllText(fileName);
            GameSaveData saveData = JsonConvert.DeserializeObject<GameSaveData>(json);
            posizioneCorrente = saveData.PosizioneCorrente;
            inventario = saveData.Inventario;
            stanzaStart = saveData.StanzaStart;
            stanzaHolmes = saveData.StanzaHolmes;
            stanzaPranzo = saveData.StanzaPranzo;
            stanzaCucina = saveData.StanzaCucina;
            stanzaReception = saveData.StanzaReception;
            stanzaPlayer = saveData.StanzaPlayer;
            stanzaPoliziotto = saveData.StanzaPoliziotto;
            stanzaSoggiorno = saveData.StanzaSoggiorno;
            stanzaManutenzione = saveData.StanzaManutenzione;
            stanzaTorture = saveData.StanzaTorture;
            
            Logger.Log("Hai caricato il gioco correttamente.");
            PrintSlowly("Il gioco è stato caricato correttamente.");
            giocoNuovo = false;
            NewGame();
        }
        else
        {
            Logger.Log("Non hai file di salvataggio.");
            PrintSlowly("Nessun file di salvataggio trovato.");
        }
    }

    /// <summary>
        /// Metodo usato per inserire il nome del giocatore all'inizio del gioco.
        /// </summary>
        /// <returns> string Nome </returns>
        static string InsertPlayerName()
    {
        PrintSlowly("Benvenuto! Inserisci il tuo nome:");
        string nome = Console.ReadLine();
        PrintSlowly("Ciao, " + nome + "! Buona partita!");
        return nome;
    }
        /// <summary>
        /// Mostra l'inventario del giocatore in game
        /// </summary>
        static void ShowInventory()
        {
            PrintSlowly("Oggetti nell'inventario(PESO MAX: 10):");

            if (inventario.Count == 0)
            {
                PrintSlowly("L'inventario è vuoto.");
            }
            else
            {
                foreach (Oggetto oggetto in inventario)
                {
                    PrintSlowly("- " + oggetto.Nome + ": " + oggetto.Descrizione + " Peso: " + oggetto.Peso);
                }
            }
        }
    public static void Main()
        {
            PrintSlowly("Benvenuto nel gioco di testo Henry Howard Castle!");
            PrintSlowly("Sei nell'anno 1951 e questo è il tuo primo giorno di lavoro all'H. Howard Castle in Philadelphia.");
            PrintSlowly("Devi solo aprire la porta davanti a te per iniziare un nuovo capitolo della tua vita!");
            PrintSlowly("Vuoi iniziare una nuova partita o continuare da un salvataggio? (nuova/continua)");
            bool sceltaPartita = true;
            do
            {
                string scelta = Console.ReadLine().ToLower();
                if (scelta == "nuova")
                {
                    NewGame();
                    sceltaPartita = false;
                }
                else if(scelta == "continua")
                {   
                    LoadGame("salvataggio.json");
                    sceltaPartita = false;
                }
                else
                {
                    PrintSlowly("Scelta non valida. Inserire nuova o continua.");
                    sceltaPartita = true;
                }
            } while(sceltaPartita);
            /// <summary>
            /// Metodo per stampare i caratteri più lentamente.
            /// </summary>
            /// <param name="text"></param>
            /// <param name="speed"></param>
        static void PrintSlowly(string text, int speed = 1)
        {
            foreach(char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(speed);
            }
            Console.WriteLine();
        }
    }
    /// <summary>
    /// Metodo che viene invocato per inizializzare il gioco con switch case per le varie scelte.
    /// </summary>
    public static void StartGame()
    {
                Logger.Log("paolo");
                bool continuaGioco = true;
                while (continuaGioco)
                {
                    if (stanze.TryGetValue(posizioneCorrente, out Stanza stanzaCorrente))
                    {
                        if (!stanzaCorrente.Visitata)
                        {
                            stanzaCorrente.ShowDescription();
                            if(posizioneCorrente.Equals(new Coordinate(1, -1)))
                            {
                                stanzaCorrente.ShowDescription();
                                stanzaCorrente.Visitata = true;
                                bool sceltaValida = false;
                                do
                                {
                                PrintSlowly("Cosa desideri fare?");
                                PrintSlowly("1. Affrontare Holmes");
                                PrintSlowly("2. Scappare dalla stanza");
                                PrintSlowly("3. Sparare a Holmes con la pistola");
                                string sceltaFinale = Console.ReadLine();
                                if (sceltaFinale == "1" || sceltaFinale == "Affrontare Holmes")
                                {
                                    PrintSlowly("Ti tuffi verso Holmes e cerchi di disarmarlo, lui si gira prontamente e ti accoltella. Sanguinante ti accasci a terra.");
                                    PrintSlowly("Holmes: Sembra che dovrò trovare una nuova guardia notturna, per adesso mi divertirò con loro due.");
                                    PrintSlowly("Hai perso.");
                                    continuaGioco = false;
                                    sceltaValida = false;
                                    return;
                                }   
                                else if (sceltaFinale == "2" || sceltaFinale == "Scappare dalla stanza")
                                {
                                    PrintSlowly("Provi a scappare da dove sei entrato, ma la porta segreta dietro di te si è chiusa, e non c'è modo di uscire\nHolmes ti acchiappa e ti butta a terra, prendendoti a coltellate.");
                                    PrintSlowly("Hai perso.");
                                    continuaGioco = false;
                                    sceltaValida = false;
                                    return;
                                }
                                else if (sceltaFinale == "3" || sceltaFinale == "Sparare a Holmes con la pistola")
                                {
                                    if (inventario.Any(o => o.Nome == "Pistola"))
                                    {
                                        PrintSlowly("Decidi di tirare fuori la pistola e spari un colpo secco ad Holmes dritto nel cuore.\nHolmes si accascia a terra sanguinante guardandoti con gli occhi sbarrati.");
                                        PrintSlowly("Dopo lo shock iniziale decidi di uscire dalla stanza e chiamare la polizia.");
                                        PrintSlowly("Fortunatamente te la sei cavato, dopo varie ricerche fu accertato che Holmes aveva già ucciso e torturato 33 vittime.");
                                        PrintSlowly("Complimenti, hai finito il gioco. Grazie per aver giocato a H.Holmes Castle!");
                                        continuaGioco = false;
                                        sceltaValida = false;
                                        return;
                                    }
                                    else
                                    {
                                        PrintSlowly("Non hai la pistola nell'inventario. Holmes mentre cercavi qualcosa per fermarlo, ti scatta addosso e accoltellandoti ti uccide.");
                                        PrintSlowly("Hai perso.");
                                        continuaGioco = false;
                                        sceltaValida = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    PrintSlowly("Scelta non valida.");
                                    sceltaValida = true;
                                }
                                } while(sceltaValida);
                            }
                            //stanzaCorrente.ShowDescription();
                            stanzaCorrente.Visitata = true;
                        }
                            PrintSlowly("Sei nella stanza: " + stanzaCorrente.Nome);
                            PrintSlowly("Puoi esaminare gli oggetti (esamina), raccogliere un oggetto (raccogli),rilasciare un oggetto(rilascia) o spostarti in una direzione (nord, est, sud, ovest). Scrivere (aiuto) per altri comandi.");
                            string input = Console.ReadLine().ToLower(); 
                            switch (input)
                            {
                                case "esamina": // esamina gli oggetti nella stanza.
                                    Logger.Log($"Hai esaminato gli oggetti nella stanza {stanzaCorrente.Nome}.");
                                    stanzaCorrente.ShowRoomItems();
                                    break;
                                case "personaggi": // esamina i personaggi nella stanza.
                                    Logger.Log($"Hai esaminato i personaggi nella stanza {stanzaCorrente.Nome}.");
                                    stanzaCorrente.ShowRoomNpcs();
                                    break;
                                case "inventario": // mostra l'inventario del giocatore.
                                    Logger.Log("Hai visualizzato l'inventario.");
                                    ShowInventory();
                                    break;
                                case "parla": // parli con i personaggi presenti nella stanza.
                                    PrintSlowly("Con quale personaggio vuoi parlare?");
                                    string personaggioInput = Console.ReadLine().ToLower();
                                    Personaggio personaggioParlare = stanzaCorrente.Personaggi.Find(o => o.Nome.ToLower() == personaggioInput);
                                    if(personaggioParlare != null)
                                    {
                                        Logger.Log($"Hai parlato con {personaggioParlare.Nome}.");
                                        stanzaCorrente.TalkToNpcs(personaggioParlare);
                                        if (personaggioParlare.OggettoAssegnato != null)
                                        {
                                            inventario.Add(personaggioParlare.OggettoAssegnato);
                                            PrintSlowly("Hai ottenuto l'oggetto: " + personaggioParlare.OggettoAssegnato.Nome + " Peso: " + personaggioParlare.OggettoAssegnato.Peso);
                                            Logger.Log($"Hai ottenuto l'oggetto {personaggioParlare.OggettoAssegnato.Nome} da {personaggioParlare.Nome}.");
                                            personaggioParlare.OggettoAssegnato = null; // Remove the object from the character
                                        }
                                    }
                                    else
                                    {
                                        PrintSlowly("Non ci sono personaggi con quel nome con cui parlare.");
                                        Logger.Log("Non hai trovato personaggi con cui parlare.");
                                    }
                                    break;
                                case "raccogli": // raccogli l'oggetto desiderato se presente nella stanza.
                                    PrintSlowly("Quale oggetto vuoi raccogliere?");
                                    string oggettoInput = Console.ReadLine().ToLower();
                                    Oggetto oggettoRaccogliere = stanzaCorrente.Oggetti.Find(o => o.Nome.ToLower() == oggettoInput);
                                    if (oggettoRaccogliere != null)
                                    {
                                        if (InventoryWeight() + oggettoRaccogliere.Peso <= pesoLimiteInventario)
                                        {
                                            stanzaCorrente.PickUpItems(oggettoRaccogliere, inventario);
                                            Logger.Log($"Hai raccolto l'oggetto {oggettoRaccogliere.Nome}.");
                                        }
                                        else
                                        {
                                            Logger.Log("Hai l'inventario pieno.");
                                            PrintSlowly("L'inventario è troppo pieno per raccogliere questo oggetto.");
                                        }
                                    }
                                    else
                                    {
                                        PrintSlowly("L'oggetto non è presente in questa stanza.");
                                        Logger.Log($"L'oggetto non è presente nella stanza {stanzaCorrente.Nome}.");
                                    }
                                    break;
                                case "rilascia": // rilasci l'oggetto desiderato nella stanza corrente se presente nell'inventario.
                                    PrintSlowly("Quale oggetto vuoi rilasciare?");
                                    string oggettoRilasciaInput = Console.ReadLine().ToLower();
                                    Oggetto oggettoRilascia = inventario.Find(o => o.Nome.ToLower() == oggettoRilasciaInput);
                                    if (oggettoRilascia != null)
                                    {
                                        stanzaCorrente.ReleaseItem(oggettoRilascia, inventario);
                                        Logger.Log($"Hai rilasciato l'oggetto {oggettoRilascia.Nome}.");
                                    }
                                    else
                                    {
                                        Program.PrintSlowly($"L'oggetto {oggettoRilascia} non è presente nell'inventario.");
                                        Logger.Log($"L'oggetto {oggettoRilascia} non è presente nell'inventario.");
                                    }
                                    break;
                                case "nord": // vai a nord
                                    Coordinate nuovaPosizioneNord = new Coordinate(posizioneCorrente.X, posizioneCorrente.Y + 1);
                                    if (!stanze.ContainsKey(nuovaPosizioneNord))
                                    {   
                                        Logger.Log("Sei andato nella direzione sbagliata. La stanza non esiste.");
                                        PrintSlowly("Non puoi andare in questa direzione, la stanza non esiste.");
                                    }
                                    else
                                    {
                                        Stanza stanzaDestinazioneNord = stanze[nuovaPosizioneNord];
                                        if (!stanzaDestinazioneNord.Aperta)
                                        {

                                            Oggetto oggettoRichiesto = stanzaDestinazioneNord.OggettoRichiesto; // Verifica se l'inventario contiene un oggetto adatto per aprire la stanza
                                            bool oggettoRichiestoPresente = inventario.Any(oggetto => oggetto.Nome == oggettoRichiesto.Nome);
                                            if (oggettoRichiestoPresente)
                                            {
                                                PrintSlowly("La porta è chiusa. Vuoi utilizzare un oggetto per aprirla? (sì/no)");
                                                string risposta = Console.ReadLine().ToLower();
                                                if (risposta == "sì" || risposta == "si")
                                                {
                                                    stanzaDestinazioneNord.OpenDoor();
                                                    Logger.Log("Hai aperto la porta con successo.");
                                                    posizioneCorrente = nuovaPosizioneNord;
                                                }
                                                else
                                                {
                                                    Logger.Log("Hai deciso di non aprire la porta con l'oggetto che avevi.");
                                                    PrintSlowly("Decidi di non utilizzare un oggetto per aprire la porta.");
                                                }
                                            }
                                            else
                                            {
                                                Logger.Log("La porta che volevi aprire era chiusa.");
                                                PrintSlowly("La porta è chiusa. Devi trovare un oggetto adatto per aprirla.");
                                            }
                                        }
                                        else
                                        {
                                            posizioneCorrente = nuovaPosizioneNord;
                                            Logger.Log($"Sei entrato nella stanza {stanzaDestinazioneNord.Nome}");
                                        }
                                    }
                                    break;
                                case "est": // vai a est
                                    Coordinate nuovaPosizioneEst = new Coordinate(posizioneCorrente.X + 1, posizioneCorrente.Y);
                                    if (!stanze.ContainsKey(nuovaPosizioneEst))
                                    {
                                        Logger.Log("Sei andato nella direzione sbagliata. La stanza non esiste.");
                                        PrintSlowly("Non puoi andare in questa direzione, la stanza non esiste.");
                                    }
                                    else
                                    {
                                        Stanza stanzaDestinazioneEst = stanze[nuovaPosizioneEst];
                                        if (!stanzaDestinazioneEst.Aperta)
                                        {
                                            Oggetto oggettoRichiesto = stanzaDestinazioneEst.OggettoRichiesto;
                                            bool oggettoRichiestoPresente = inventario.Any(oggetto => oggetto.Nome == oggettoRichiesto.Nome);
                                            if (oggettoRichiestoPresente)
                                            {
                                                PrintSlowly("La porta è chiusa. Vuoi utilizzare un oggetto per aprirla? (sì/no)");
                                                string risposta = Console.ReadLine().ToLower();

                                                if (risposta == "sì" || risposta == "si")
                                                {
                                                    stanzaDestinazioneEst.OpenDoor();
                                                    Logger.Log("Hai aperto la porta con successo.");
                                                    posizioneCorrente = nuovaPosizioneEst;
                                                }
                                                else
                                                {
                                                    Logger.Log("Hai deciso di non aprire la porta con l'oggetto che avevi.");
                                                    PrintSlowly("Decidi di non utilizzare un oggetto per aprire la porta.");
                                                }
                                            }
                                            else
                                            {
                                                Logger.Log("La porta che volevi aprire era chiusa.");
                                                PrintSlowly("La porta è chiusa. Devi trovare un oggetto adatto per aprirla.");
                                            }
                                        }
                                        else
                                        {
                                            posizioneCorrente = nuovaPosizioneEst;
                                            Logger.Log($"Sei entrato nella stanza {stanzaDestinazioneEst.Nome}");
                                        }
                                    }
                                    break;
                                case "sud": // vai a sud
                                    Coordinate nuovaPosizioneSud = new Coordinate(posizioneCorrente.X, posizioneCorrente.Y - 1);
                                    if (!stanze.ContainsKey(nuovaPosizioneSud))
                                    {
                                        Logger.Log("Sei andato nella direzione sbagliata. La stanza non esiste.");
                                        PrintSlowly("Non puoi andare in questa direzione, la stanza non esiste.");
                                    }
                                    else
                                    {
                                        Stanza stanzaDestinazioneSud = stanze[nuovaPosizioneSud];
                                        if (!stanzaDestinazioneSud.Aperta)
                                        {
                                            Oggetto oggettoRichiesto = stanzaDestinazioneSud.OggettoRichiesto;
                                            bool oggettoRichiestoPresente = inventario.Any(oggetto => oggetto.Nome == oggettoRichiesto.Nome);
                                            if (oggettoRichiestoPresente)
                                            {
                                                PrintSlowly("La porta è chiusa. Vuoi utilizzare un oggetto per aprirla? (sì/no)");
                                                string risposta = Console.ReadLine().ToLower();

                                                if (risposta == "sì" || risposta == "si")
                                                {
                                                    Oggetto oggettoApertura = inventario.FirstOrDefault(oggetto => stanzaCorrente.RichiedeOggetto);
                                                    stanzaDestinazioneSud.OpenDoor();
                                                    Logger.Log("Hai aperto la porta con successo.");
                                                    posizioneCorrente = nuovaPosizioneSud;
                                                }
                                                else
                                                {
                                                    Logger.Log("Hai deciso di non aprire la porta con l'oggetto che avevi.");
                                                    PrintSlowly("Decidi di non utilizzare un oggetto per aprire la porta.");
                                                }
                                            }
                                            else
                                            {
                                                Logger.Log("La porta che volevi aprire era chiusa.");
                                                PrintSlowly("La porta è chiusa. Devi trovare un oggetto adatto per aprirla.");
                                            }
                                        }
                                        else
                                        {                                                
                                            posizioneCorrente = nuovaPosizioneSud;
                                            Logger.Log($"Sei entrato nella stanza {stanzaDestinazioneSud.Nome}");
                                        }
                                    }
                                    break;
                                case "ovest": // vai a ovest
                                    Coordinate nuovaPosizioneOvest = new Coordinate(posizioneCorrente.X - 1, posizioneCorrente.Y);
                                    if (!stanze.ContainsKey(nuovaPosizioneOvest))
                                    {
                                        Logger.Log("Sei andato nella direzione sbagliata. La stanza non esiste.");
                                        PrintSlowly("Non puoi andare in questa direzione, la stanza non esiste.");
                                    }
                                    else
                                    {
                                        Stanza stanzaDestinazioneOvest = stanze[nuovaPosizioneOvest];
                                        if (!stanzaDestinazioneOvest.Aperta)
                                        {
                                            Oggetto oggettoRichiesto = stanzaDestinazioneOvest.OggettoRichiesto;
                                            bool oggettoRichiestoPresente = inventario.Any(oggetto => oggetto.Nome == oggettoRichiesto.Nome);
                                            if (oggettoRichiestoPresente)
                                            {
                                                PrintSlowly("La porta è chiusa. Vuoi utilizzare un oggetto per aprirla? (sì/no)");
                                                string risposta = Console.ReadLine().ToLower();

                                                if (risposta == "sì" || risposta == "si")
                                                {
                                                    Oggetto oggettoApertura = inventario.FirstOrDefault(oggetto => stanzaCorrente.RichiedeOggetto);
                                                    stanzaDestinazioneOvest.OpenDoor();
                                                    Logger.Log("Hai aperto la porta con successo.");
                                                    posizioneCorrente = nuovaPosizioneOvest;
                                                }
                                                else
                                                {
                                                    Logger.Log("Hai deciso di non aprire la porta con l'oggetto che avevi.");
                                                    PrintSlowly("Decidi di non utilizzare un oggetto per aprire la porta.");
                                                }
                                            }
                                            else
                                            {
                                                Logger.Log("La porta che volevi aprire era chiusa.");
                                                PrintSlowly("La porta è chiusa. Devi trovare un oggetto adatto per aprirla.");
                                            }
                                        }
                                        else
                                        {
                                            posizioneCorrente = nuovaPosizioneOvest;
                                            Logger.Log($"Sei entrato nella stanza {stanzaDestinazioneOvest.Nome}");
                                        }
                                    }
                                    break;
                                case "esci": // esci dal gioco senza salvare.
                                    continuaGioco = false;
                                    PrintSlowly("Hai deciso di uscire dal gioco. Arrivederci.");
                                    Logger.Log("Sei uscito dal gioco.");
                                    break;
                                case "aiuto": // helper per i comandi
                                    Logger.Log("Hai richiesto un helper per i comandi.");
                                    PrintSlowly("Altri comandi disponibili:\n -inventario: mostra gli oggetti nel proprio inventario.\n -esci: esci dal gioco.\n -personaggi: mostra l'elenco dei personaggi nella stanza.\n -parla: interagisci con i personaggi. \n -OBIETTIVO DEL GIOCO: Cercare di raccogliere gli oggetti adatti per entrare nella stanza segreta del cattivo e sconfiggerlo.");
                                    break;
                                case "salva": // salvi il gioco.
                                    SaveGame("salvataggio.json");
                                    PrintSlowly("Hai salvato la partita.");
                                    break;
                                default: // default per qualsiasi comando diverso da quelli proposti.
                                    Logger.Log("Hai inserito un comando non valido.");
                                    PrintSlowly("Comando non valido.");
                                    break;
                            }
                            if (!stanze.ContainsKey(posizioneCorrente))
                            {
                                Coordinate posizionePrecedente = posizioneCorrente;
                                PrintSlowly("Non puoi andare in questa direzione, la stanza non esiste.");
                                posizioneCorrente = posizionePrecedente; // Reimposta sulla posizione precedente
                            }
                        }
                    }
                }
        /// <summary>
        /// Metodo per iniziare una nuova partita quando scrivi "nuova".
        /// </summary>
        public static void NewGame()
        {
            Logger.Log("Hai iniziato una nuova partita.");

            int SpawnPg = Stanza.SpawnRandom();
            
            string nomeGiocatore = InsertPlayerName();
            if(giocoNuovo){
            Logger.Log("Vengono instanziate le stanze, i personaggi e gli oggetti.");
            stanze.Add(coordinateStart, stanzaStart);
            
            stanzaHolmes.OggettoRichiesto = new Oggetto("Cacciavite", "Un cacciavite con punta molto fina.", 1, true);
            stanze.Add(coordinateHolmes, stanzaHolmes);

            stanzaPlayer.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
            stanze.Add(coordinatePlayer, stanzaPlayer);

            stanze.Add(coordinateReception, stanzaReception);

            stanzaPranzo.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
            stanze.Add(coordinatePranzo, stanzaPranzo);

            stanzaCucina.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
            stanze.Add(coordinateCucina, stanzaCucina);

            stanzaManutenzione.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
            stanze.Add(coordinateManutenzione, stanzaManutenzione);

            stanzaPoliziotto.OggettoRichiesto = new Oggetto("Chiave stanza poliziotto", "Una chiave per aprire la stanza del poliziotto", 1, true);
            stanze.Add(coordinatePoliziotto, stanzaPoliziotto);

            stanzaTorture.OggettoRichiesto = new Oggetto("Batticarne", "Un batticarne molto pesante, finalmente puoi preparare il tuo carpaccio!", 3, true);
            stanzaTorture.OggettoRichiesto = new Oggetto("Statuetta", "Una statuetta rappresentante un angelo con una tromba.", 3, true);
            stanze.Add(coordinateTorture, stanzaTorture);

            stanze.Add(coordinateSoggiorno, stanzaSoggiorno);
            stanzaStart.AddItem("pianta", "Una comune pianta da soggiorno, ma comunque elegante.", 0, false);

            stanzaHolmes.AddItem("Bilancia", "Una bilancia placcata in ottone, forse si può appoggiare qualcosa sopra di essa.", 0, false);
            stanzaHolmes.AddItem("Statua di marmo","Una scultura di marmo molto antica, deve essere costata molto, ma ormai è a pezzi.", 0, false);
            stanzaHolmes.AddItem("Scrivania","La scrivania è piena di fogli sparsi, come se fosse passata una folata di vento", 0, false);
            stanzaHolmes.AddItem("Orologio","Un orologio a cucù, pare non funzioni", 0, false);
            stanzaHolmes.AddItem("Libreria","La libreria è mezza vuota, alcuni libri sono a terra, c'è una bilancia al centro di essa, strano..", 0, false);
            
            stanzaPlayer.AddItem("Scrivania", "Una scrivania in legno molto pesante, sembra esserci un biglietto sopra", 0, false);
            stanzaPlayer.AddItem("Comodino", "Un comodino semplice, serve solo per appoggiarci la lampada.", 0, false);
            stanzaPlayer.AddItem("Biglietto", "Sul biglietto c'è scritto: Sono la vecchia guardia, ho notato che il capo ha uno strano atteggiamento e molte volte entra nella sua stanza, ma bussando non risponde nessuno, dati gli ultimi avvenimenti mi sono licenziato e ho deciso di lavorare altrove. A sud della stanza del capo ho notato qualcosa di strano.. Spero questo biglietto ti sia utile. Buona Fortuna. ", 0, true);
            stanzaPlayer.AddItem("Lampada", "Lampada molto usurata, non funziona bene la lampadina, dovrebbero cambiarla.", 0, false);
            stanzaPlayer.AddItem("Rasoio", "Nel bagno c'è un rasoio usato, non credo lo utilizzerò.", 1, true);
            stanzaPlayer.AddItem("Torcia", "Una torcia lasciata dalla precedente guardia.", 2, true);

            stanzaReception.AddNpc("Receptionist", "La receptionist ti accoglie con un sorriso, una ragazza molto raffinata.",$"Salve e benvenuto nell'hotel H.Holmes, lei deve essere {nomeGiocatore} la nuova guardia dell'hotel, il capo è al momento impegnato, ma ci pensiamo noi a lasciarle il passpartout, i panni da lavoro può trovarli nella sua stanza, buona permanenza!", true, new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true));
            stanzaReception.AddNpc("Signore", "Paul sembra molto preoccupato.", $"Buonasera io sono Paul, ma mi sembra di averla già vista da qualche parte... Ma certo! Tu sei {nomeGiocatore}, il mio vecchio compagno di scuola.. Non dovrei dirtelo, ma ho visto un poliziotto che conosco che si aggira per l'hotel, l'ho visto entrare adesso in sala da pranzo. Bisogna stare attenti di questi tempi..", true, null); 
            
            stanzaPranzo.AddItem("Tavolo","Il tavolo è apparecchiato elegantemente, posate d'argento e un centro tavola pieno di fiori.", 0, false);
            stanzaPranzo.AddItem("Bicchiere di cristallo","Un bicchiere di vetro, sembra fatto a mano.", 0, false);
            stanzaPranzo.AddNpc("Poliziotto", "Il poliziotto sta cenando, non disturbarlo più di quanto tu abbia già fatto.",$"Salve.. Come va? Sto mangiando al momento... Ah ma sei {nomeGiocatore}! Mi ricordo di te nell'esercito! Che ci fai qui? Hai cambiato vita? ...Sei la nuova guardia notturna? Ahahah, allora prendi anche le chiavi della mia stanza per sicurezza, potrebbero esserti utili..", true, new Oggetto("Chiave stanza poliziotto", "Una chiave per aprire la stanza del poliziotto", 1, true));

            stanzaCucina.AddItem("Coltello", "Un coltello affilato e pericoloso, sarebbe meglio lasciarlo qui.", 2, true);
            stanzaCucina.AddItem("Carpaccio di carne", "Un carpaccio di carne fresco, molto invitante, strano lo abbiano lasciato qui.", 0, false);
            stanzaCucina.AddItem("Pentola con sugo", "Una pentola piena di ragù lasciato a sobbollire, sembra delizioso.", 0, false);
            stanzaCucina.AddNpc("Cuoco", "Il cuoco ti mangia con lo sguardo, meglio non disturbarlo nuovamente...", "Che fai qui dentro? Non è permesso entrare a chi non lavora qua.. Ah, sei la guardia? Perdonami, ma se vuoi fare quel carpaccio te lo regalo io lo strumento adatto a farlo!", true, new Oggetto("Batticarne", "Un batticarne molto pesante, finalmente puoi preparare il tuo carpaccio!", 3, true));
            
            stanzaManutenzione.AddItem("Centralina", "Una centralina elettrica dell'hotel, c'è un pulsante senza nome..", 0, false);
            stanzaManutenzione.AddItem("Carrello vestiti", "Un carrello con i vestiti sporchi, sembrano tanti rispetto al numero delle stanze.", 0, false);
            stanzaManutenzione.AddItem("Scopa", "Una scopa usata.", 0, false);
            stanzaManutenzione.AddItem("Cavi elettrici", "Dei cavi di ricambio per sistemi elettrici.", 1, true);
            stanzaManutenzione.AddItem("Cacciavite", "Un cacciavite con punta molto fina.", 1, true);

            stanzaPoliziotto.AddItem("Coperte", "Le coperte sono tutte disordinate, sembra ci sia stata un' urgenza.", 0, false);
            stanzaPoliziotto.AddItem("Comodino", "Un comodino con un cassetto, ha un buco dove inserire una chiave.", 0, false);
            stanzaPoliziotto.AddItem("Scrivania", "La scrivania è storta ed ha qualche graffio, strano..", 0, false);
            stanzaPoliziotto.AddItem("Pistola", "Una pistola di servizio, strano non l'abbia portata con se..", 2, true);
            
            stanzaTorture.AddNpc("Holmes", "Holmes ha degli occhi persi nel vuoto e noti tutto il suo intento omicida.",$"Salve {nomeGiocatore}, come vede sono impegnato. Come osa disturbarmi? Vieni più vicino..", true, null);
      
            stanzaSoggiorno.AddItem("Statuetta", "Una statuetta rappresentante un angelo con una tromba.", 3, true);
            stanzaSoggiorno.AddItem("Divano", "Divano in pelle, sembra molto vecchio la pelle è usurata.", 0, false);
            stanzaSoggiorno.AddItem("Gufo imbalsamato", "Un gufo imbalsamato, ha uno sguardo abbastanza inquietante", 0, false);
            //spawniamo il personaggio casuale Cameriera in una stanza di queste 4
            if (SpawnPg == 1)
            {
                stanzaStart.AddNpc("Cameriera", "La cameriera sembra essere molto stanca, credo stia andando a casa.",$"Salve, lei deve essere {nomeGiocatore}, piacere di conoscerla ma ho appena staccato da lavoro, magari ci conosceremo meglio in un altro momento.", true, null);
            }
            else if(SpawnPg == 2)
            {
                stanzaPranzo.AddNpc("Cameriera", "La cameriera è molto indaffarata al momento, sta servendo un tavolo.","Salve, come posso aiutarla? Voleva qualcosa da bere?", true, null);
            }
            else if(SpawnPg == 3)
            {
                stanzaSoggiorno.AddNpc("Cameriera", "La cameriera è vestita da lavoro, credo stia iniziando il turno.","Salve, mi scusi ma sto andando di fretta, sono in ritardo per andare a lavoro.", true, null);
            }
            else
            {
                stanzaReception.AddNpc("Cameriera", "La cameriera sembra stia chiedendo informazioni alla receptionist.","Salve, mi stavo chiedendo dove fosse il capo, avrei bisogno di parlargli.", true, null);
            }
            }
            else
            {
                stanze.Add(coordinateStart, stanzaStart);
            
            stanzaHolmes.OggettoRichiesto = new Oggetto("Cacciavite", "Un cacciavite con punta molto fina.", 1, true);
            stanze.Add(coordinateHolmes, stanzaHolmes);

            stanzaPlayer.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
            stanze.Add(coordinatePlayer, stanzaPlayer);

            stanze.Add(coordinateReception, stanzaReception);

            stanzaPranzo.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
            stanze.Add(coordinatePranzo, stanzaPranzo);

            stanzaCucina.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
            stanze.Add(coordinateCucina, stanzaCucina);

            stanzaManutenzione.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
            stanze.Add(coordinateManutenzione, stanzaManutenzione);

            stanzaPoliziotto.OggettoRichiesto = new Oggetto("Chiave stanza poliziotto", "Una chiave per aprire la stanza del poliziotto", 1, true);
            stanze.Add(coordinatePoliziotto, stanzaPoliziotto);

            stanzaTorture.OggettoRichiesto = new Oggetto("Batticarne", "Un batticarne molto pesante, finalmente puoi preparare il tuo carpaccio!", 3, true);
            stanzaTorture.OggettoRichiesto = new Oggetto("Statuetta", "Una statuetta rappresentante un angelo con una tromba.", 3, true);
            stanze.Add(coordinateTorture, stanzaTorture);

            stanze.Add(coordinateSoggiorno, stanzaSoggiorno);
            }
            Logger.Log("Viene chiamato il metodo StartGame.");
            StartGame();
        }
        /// <summary>
        /// Metodo per stampare i caratteri lentamente.
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
        /// <summary>
        /// Metodo per calcolare il peso totale degli oggetti nell'inventario.
        /// </summary>
        /// <returns>int pesoTotale</returns>
        static int InventoryWeight()
        {
            int pesoTotale = 0;

            foreach (Oggetto oggetto in inventario)
            {
                pesoTotale += oggetto.Peso;
            }

            return pesoTotale;
        }
    }
    
}
    

