using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;

namespace Gioco
{
        
    class Oggetto
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

    
    
    class Personaggio
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
    
    [Serializable]
    public struct Coordinate
        {   
            public int X { get; set; }
            public int Y { get; set; }
            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    class Stanza
    {
        public string Nome { get; set; }
        
        public string Descrizione { get; set; }
        
        public List<Oggetto> Oggetti { get; set; }
        
        public List<Personaggio> Personaggi { get; set; }
        
        public bool Visitata { get; set; }
        
        public bool RichiedeOggetto { get; set; }
        
        public bool Aperta { get; set; }
        
        public Oggetto OggettoRichiesto { get; set;}
        public int X { get; internal set; }
        public int Y { get; internal set; }

        public Stanza(string nome, string descrizione, bool richiedeOggetto, bool aperta)
        {
            Nome = nome;
            Descrizione = descrizione;
            Oggetti = new List<Oggetto>();
            Personaggi = new List<Personaggio>();
            RichiedeOggetto = richiedeOggetto;
            Aperta = aperta;
        }

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
        public void RilasciaOggetto(Oggetto oggetto, List<Oggetto> inventario)
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
            /// Imposta la porta in "aperta" e mostra il messaggio di apertura.
            /// </summary>
           public void ApriPorta()
        {
            Aperta = true;
            PrintSlowly("Hai aperto la porta con successo!");
        }
        public void MostraDescrizione()
        {
            PrintSlowly(Descrizione);
        } 
        public void AggiungiPersonaggio(string nome, string descrizione, string discorso, bool discorsoSi, Oggetto oggetto)
        {
            Personaggi.Add(new Personaggio(nome, descrizione, discorso, discorsoSi, oggetto));
        }
        public void AggiungiOggetto(string nome, string descrizione, int peso, bool raccoglibile)
        {
            Oggetti.Add(new Oggetto(nome, descrizione, peso, raccoglibile));
        }

        public void EsaminaOggetti()
        {
            PrintSlowly("Gli oggetti in questa stanza sono:");
            foreach (Oggetto oggetto in Oggetti)
            {
                PrintSlowly("- " + oggetto.Nome + ": " + oggetto.Descrizione);
            }
        }

        public void RaccogliOggetto(Oggetto oggetto, List<Oggetto> inventario)
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
        public void EsaminaPersonaggi()
        {
            PrintSlowly("I personaggi in questa stanza sono:");
            foreach (Personaggio personaggio in Personaggi)
            {
                PrintSlowly("-" + personaggio.Nome);
            }
        }

        public void ParlaConPersonaggio(Personaggio personaggio)
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

            public static void PrintSlowly(string text, int speed = 10)
        {
            foreach(char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(speed);
            }
            Console.WriteLine();
        }
    }

    class Program
    {
        static string InserisciNomeGiocatore()
    {
        PrintSlowly("Benvenuto! Inserisci il tuo nome:");
        string nome = Console.ReadLine();
        PrintSlowly("Ciao, " + nome + "! Buona partita!");
        return nome;
    }
        static void MostraInventario()
        {
            PrintSlowly("Oggetti nell'inventario:");

            if (inventario.Count == 0)
            {
                PrintSlowly("L'inventario è vuoto.");
            }
            else
            {
                foreach (Oggetto oggetto in inventario)
                {
                    Console.WriteLine("- " + oggetto.Nome + ": " + oggetto.Descrizione + " Peso: " + oggetto.Peso);
                }
            }
        }
        static Coordinate posizioneCorrente = new Coordinate(0, 0); // Coordinata iniziale della posizione corrente
        static Dictionary<Coordinate, Stanza> stanze = new Dictionary<Coordinate, Stanza>();
        static List<Oggetto> inventario = new List<Oggetto>();
        static int pesoLimiteInventario = 10; // Limite di peso dell'inventario
        static ObservableCollection<Stanza> ListaStanze = new ObservableCollection<Stanza>(stanze.Values);
    public class GameSaveData
    {
        public List<Oggetto> Inventario { get; set; }
        public Coordinate PosizioneCorrente {get; set; }
        public ObservableCollection<Stanza> ListaStanze { get; set; }
    }

    private static void SaveGame(string fileName)
    {
        GameSaveData saveData = new GameSaveData
        {
            PosizioneCorrente = posizioneCorrente,
            Inventario = inventario,
            ListaStanze = new ObservableCollection<Stanza>(ListaStanze)
        };

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(fileName, json);

        Console.WriteLine("Il gioco è stato salvato correttamente.");
    }


    // Load the game state from a JSON file
    static void LoadGame(string fileName)
    {
        if (File.Exists(fileName))
        {
            string json = File.ReadAllText(fileName);
            GameSaveData saveData = JsonConvert.DeserializeObject<GameSaveData>(json);
            posizioneCorrente = saveData.PosizioneCorrente;
            inventario = saveData.Inventario;
            ListaStanze = saveData.ListaStanze;
            Console.WriteLine("Il gioco è stato caricato correttamente.");
        }
        else
        {
            Console.WriteLine("Nessun file di salvataggio trovato.");
        }
    }

    public static void Main()
        {
            PrintSlowly("Benvenuto nel gioco di testo Henry Howard Castle!");
            PrintSlowly("Sei nell'anno 1951 e questo è il tuo primo giorno di lavoro all'H. Howard Castle in Philadelphia.");
            PrintSlowly("Devi solo aprire la porta davanti a te per iniziare un nuovo capitolo della tua vita!");
            Console.WriteLine("Vuoi iniziare una nuova partita o continuare da un salvataggio? (nuova/continua)");
            string scelta = Console.ReadLine().ToLower();
            if (scelta == "nuova"){
                 int SpawnPg = Stanza.SpawnRandom();
                string nomeGiocatore = InserisciNomeGiocatore();
                Stanza stanzaStart = new Stanza("Ingresso", "Appena entrato nell'hotel noti che è un hotel modesto, alcune piante e un tappeto mostrano la via per la reception davanti a te.", false, true);
                stanzaStart.AggiungiOggetto("pianta", "Una comune pianta da soggiorno, ma comunque elegante.", 0, false);
                stanze.Add(new Coordinate(0, 0), stanzaStart);
                ListaStanze.Add(stanzaStart);

                Stanza stanzaHolmes = new Stanza("Stanza di H.Holmes", "Trovi la stanza in pieno disordine, pare sia passato un uragano, vedi una statua di marmo a pezzi vicino alla scrivania, fogli e libri ovunque, è sicuramente successo qualcosa...", true, false);
                stanzaHolmes.AggiungiOggetto("Bilancia", "Una bilancia placcata in ottone, forse si può appoggiare qualcosa sopra di essa.", 0, false);
                stanzaHolmes.AggiungiOggetto("Statua di marmo","Una scultura di marmo molto antica, deve essere costata molto, ma ormai è a pezzi.", 0, false);
                stanzaHolmes.AggiungiOggetto("Scrivania","La scrivania è piena di fogli sparsi, come se fosse passata una folata di vento", 0, false);
                stanzaHolmes.AggiungiOggetto("Orologio","Un orologio a cucù, pare non funzioni", 0, false);
                stanzaHolmes.AggiungiOggetto("Libreria","La libreria è mezza vuota, alcuni libri sono a terra, c'è una bilancia al centro di essa, strano..", 0, false);
                stanzaHolmes.OggettoRichiesto = new Oggetto("Cacciavite", "Un cacciavite con punta molto fina.", 1, true);
                stanze.TryAdd(new Coordinate(1, 0), stanzaHolmes);
                ListaStanze.Add(stanzaHolmes);

                Stanza stanzaPlayer = new Stanza("La tua stanza", "Entrando nella stanza trovi i tuoi vestiti da lavoro sul letto, è una stanza luminosa ma molto piccola, ma ha tutto l'occorrente per un soggiorno lavorativo.", true, false);
                stanzaPlayer.AggiungiOggetto("Scrivania", "Una scrivania in legno molto pesante, sembra esserci un biglietto sopra", 0, false);
                stanzaPlayer.AggiungiOggetto("Comodino", "Un comodino semplice, serve solo per appoggiarci la lampada.", 0, false);
                stanzaPlayer.AggiungiOggetto("Biglietto", "Sul biglietto c'è scritto: Sono la vecchia guardia, ho notato che il capo ha uno strano atteggiamento e molte volte entra nella sua stanza, ma bussando non risponde nessuno, dati gli ultimi avvenimenti mi sono licenziato e ho deciso di lavorare altrove. A sud della stanza del capo ho notato qualcosa di strano.. Spero questo biglietto ti sia utile. Buona Fortuna. ", 0, true);
                stanzaPlayer.AggiungiOggetto("Lampada", "Lampada molto usurata, non funziona bene la lampadina, dovrebbero cambiarla.", 0, false);
                stanzaPlayer.AggiungiOggetto("Rasoio", "Nel bagno c'è un rasoio usato, non credo lo utilizzerò.", 1, true);
                stanzaPlayer.AggiungiOggetto("Torcia", "Una torcia lasciata dalla precedente guardia.", 2, true);
                stanzaPlayer.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
                stanze.TryAdd(new Coordinate(1, 1), stanzaPlayer);
                ListaStanze.Add(stanzaPlayer);

                Stanza stanzaReception = new Stanza("Reception", "All'arrivo in reception vi trovate accolti da un ambiente accogliente, una signora si trova alla scrivania, ma sta parlando con un signore dalla faccia conosciuta.", false, true);
                stanze.TryAdd(new Coordinate(0, 1), stanzaReception);
                stanzaReception.AggiungiPersonaggio("Receptionist", "La receptionist ti accoglie con un sorriso, una ragazza molto raffinata.",$"Salve e benvenuto nell'hotel H.Holmes, lei deve essere {nomeGiocatore} la nuova guardia dell'hotel, il capo è al momento impegnato, ma ci pensiamo noi a lasciarle il passpartout, i panni da lavoro può trovarli nella sua stanza, buona permanenza!", true, new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true));
                stanzaReception.AggiungiPersonaggio("Signore", "Bob sembra molto preoccupato.", $"Buonasera io sono Bob, ma mi sembra di averla già vista da qualche parte... Ma certo! Tu sei {nomeGiocatore}, il mio vecchio compagno di scuola.. Non dovrei dirtelo, ma sono qui in incognito per svolgere delle indagini su questo hotel, credo che ci rivedremo presto, soggiornerò qui per un po'.", true, null);
                ListaStanze.Add(stanzaReception);

                Stanza stanzaPranzo = new Stanza("Sala da pranzo", "La sala da pranzo ha pochi tavoli disposti in fila, da qui puoi intravedere la cucina, il poliziotto sta mangiando seduto ad un tavolo..", true, false);
                stanzaPranzo.AggiungiOggetto("Tavolo","Il tavolo è apparecchiato elegantemente, posate d'argento e un centro tavola pieno di fiori.", 0, false);
                stanzaPranzo.AggiungiOggetto("Bicchiere di cristallo","Un bicchiere di vetro, sembra fatto a mano.", 0, false);
                stanzaPranzo.AggiungiPersonaggio("Poliziotto", "Bob sta cenando, non disturbarlo più di quanto tu abbia già fatto.",$"Ciao {nomeGiocatore}, come va? Sto mangiando al momento, ma non ho troppa fame ho una brutta sensazione, per favore prendi queste chiavi per sicurezza, potrebbero esserti utili..", true, new Oggetto("Chiave stanza poliziotto", "Una chiave per aprire la stanza del poliziotto", 1, true));
                stanzaPranzo.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
                stanze.TryAdd(new Coordinate(-1, 0), stanzaPranzo);
                ListaStanze.Add(stanzaPranzo);

                Stanza stanzaCucina = new Stanza("Cucina", "Un profumo delizioso ti accoglie in cucina, sui fuochi vi è appoggiato un pentolone pieno di sugo che sta cuocendo, ma non c'è nessuno al momento..", true, false);
                stanzaCucina.AggiungiOggetto("Coltello", "Un coltello affilato e pericoloso, sarebbe meglio lasciarlo qui.", 2, true);
                stanzaCucina.AggiungiOggetto("Carpaccio di carne", "Un carpaccio di carne fresco, molto invitante, strano lo abbiano lasciato qui.", 0, false);
                stanzaCucina.AggiungiOggetto("Pentola con sugo", "Una pentola piena di ragù lasciato a sobbollire, sembra delizioso.", 0, false);
                stanzaCucina.AggiungiPersonaggio("Cuoco", "Il cuoco ti mangia con lo sguardo, meglio non disturbarlo nuovamente...", "Che fai qui dentro? Non è permesso entrare a chi non lavora qua.. Ah, sei la guardia? Perdonami, ma se vuoi fare quel carpaccio te lo regalo io lo strumento adatto a farlo!", true, new Oggetto("Batticarne", "Un batticarne molto pesante, finalmente puoi preparare il tuo carpaccio!", 3, true));
                stanzaCucina.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
                stanze.TryAdd(new Coordinate(-1, 1), stanzaCucina);
                ListaStanze.Add(stanzaCucina);

                Stanza stanzaManutenzione = new Stanza("Stanza Lavanderia/Manutenzione", "uno sgabuzzino abbastanza angusto, qui vengono tenuti tutti gli utensili dell'hotel e la centralina elettrica generale.", true, false);
                stanzaManutenzione.AggiungiOggetto("Centralina", "Una centralina elettrica dell'hotel, c'è un pulsante senza nome..", 0, false);
                stanzaManutenzione.AggiungiOggetto("Carrello vestiti", "Un carrello con i vestiti sporchi, sembrano tanti rispetto al numero delle stanze.", 0, false);
                stanzaManutenzione.AggiungiOggetto("Armadietto", "Un armadietto per gli utensili.", 0, false);
                stanzaManutenzione.AggiungiOggetto("Cacciavite", "Un cacciavite con punta molto fina.", 1, true);
                stanzaManutenzione.OggettoRichiesto = new Oggetto("Passpartout", "Una chiave per aprire tutte le porte, tranne una...", 1, true);
                stanze.TryAdd(new Coordinate(-1, 2), stanzaManutenzione);
                ListaStanze.Add(stanzaManutenzione);

                Stanza stanzaPoliziotto = new Stanza("Stanza del poliziotto", "Quando entri nella stanza capisci che qualcosa non va, dalle coperte alla scrivania è tutto in disordine, deve essere successo qualcosa...", false, true);
                stanzaPoliziotto.AggiungiOggetto("Coperte", "Le coperte sono tutte disordinate, sembra ci sia stata un' urgenza.", 0, false);
                stanzaPoliziotto.AggiungiOggetto("Comodino", "Un comodino con un cassetto, ha un buco dove inserire una chiave.", 0, false);
                stanzaPoliziotto.AggiungiOggetto("Scrivania", "La scrivania è storta ed ha qualche graffio, strano..", 0, false);
                stanzaPoliziotto.AggiungiOggetto("Pistola", "Una pistola di servizio, strano non l'abbia portata con se..", 2, true);
                stanzaPoliziotto.OggettoRichiesto = new Oggetto("Chiave stanza poliziotto", "Una chiave per aprire la stanza del poliziotto", 1, true);
                stanze.TryAdd(new Coordinate(1, 2), stanzaPoliziotto);
                ListaStanze.Add(stanzaPoliziotto);

                Stanza stanzaTorture = new Stanza("Stanza torture", "Una volta appoggiato il batticarne e la statuetta sulla bilancia la libreria inizia a girare e trovi un'altra stanza. Entrando nella stanza segreta trovi Holmes che sta facendo a pezzi il poliziotto, che decidi di fare?", true, false);
                stanzaTorture.OggettoRichiesto = new Oggetto("Batticarne", "Un batticarne molto pesante, finalmente puoi preparare il tuo carpaccio!", 3, true);
                stanzaTorture.OggettoRichiesto = new Oggetto("Statuetta", "Una statuetta rappresentante un angelo con una tromba.", 3, true);
                stanzaTorture.AggiungiPersonaggio("Holmes", "Holmes ha degli occhi persi nel vuoto e noti tutto il suo intento omicida.",$"Salve {nomeGiocatore}, come vede sono impegnato. Come osa disturbarmi? Vieni più vicino..", true, null);
                stanze.TryAdd(new Coordinate(1, -1), stanzaTorture);
                ListaStanze.Add(stanzaTorture);

                Stanza stanzaSoggiorno = new Stanza("Soggiorno", "Il soggiorno è un ambiente tranquillo, ci sono delle poltrone e dei divani a contornarlo con un camino attaccato alla parete, delle decorazioni con animali imbalsamati sulle pareti.", false, true);
                stanzaSoggiorno.AggiungiOggetto("Statuetta", "Una statuetta rappresentante un angelo con una tromba.", 3, true);
                stanzaSoggiorno.AggiungiOggetto("Divano", "Divano in pelle, sembra molto vecchio la pelle è usurata.", 0, false);
                stanzaSoggiorno.AggiungiOggetto("Gufo imbalsamato", "Un gufo imbalsamato, ha uno sguardo abbastanza inquietante", 0, false);
                stanze.TryAdd(new Coordinate(0, 2), stanzaSoggiorno);
                ListaStanze.Add(stanzaSoggiorno);
                if (SpawnPg == 1)
                {
                    stanzaStart.AggiungiPersonaggio("Cameriera", "La cameriera sembra essere molto stanca, credo stia andando a casa.",$"Salve, lei deve essere {nomeGiocatore}, piacere di conoscerla ma ho appena staccato da lavoro, magari ci conosceremo meglio in un altro momento.", true, null);
                }
                else if(SpawnPg == 2)
                {
                    stanzaPranzo.AggiungiPersonaggio("Cameriera", "La cameriera è molto indaffarata al momento, sta servendo un tavolo.","Salve, come posso aiutarla? Voleva qualcosa da bere?", true, null);
                }
                else if(SpawnPg == 3)
                {
                    stanzaSoggiorno.AggiungiPersonaggio("Cameriera", "La cameriera è vestita da lavoro, credo stia iniziando il turno.","Salve, mi scusi ma sto andando di fretta, sono in ritardo per andare a lavoro.", true, null);
                }
                else
                {
                    stanzaReception.AggiungiPersonaggio("Cameriera", "La cameriera sembra stia chiedendo informazioni alla receptionist.","Salve, mi stavo chiedendo dove fosse il capo, avrei bisogno di parlargli.", true, null);
                }
                IniziaGioco();
            }
            else if(scelta == "continua")
            {   
                LoadGame("gioco.json");
                IniziaGioco();   
            }
            
     static void PrintSlowly(string text, int speed = 10)
    {
        foreach(char c in text)
        {
            Console.Write(c);
            System.Threading.Thread.Sleep(speed);
        }
        Console.WriteLine();
    }

        static int CalcolaPesoInventario()
        {
            int pesoTotale = 0;

            foreach (Oggetto oggetto in inventario)
            {
                pesoTotale += oggetto.Peso;
            }

            return pesoTotale;
        }
    }
    public static void IniziaGioco()
    {
                bool continuaGioco = true;
                while (continuaGioco)
                {
                    if (stanze.TryGetValue(posizioneCorrente, out Stanza stanzaCorrente))
                    {
                        if (!stanzaCorrente.Visitata) // Mostra la descrizione solo alla prima entrata
                        {
                            stanzaCorrente.MostraDescrizione();
                            stanzaCorrente.Visitata = true;
                        }
                            PrintSlowly("Sei nella stanza: " + stanzaCorrente.Nome);
                            PrintSlowly("Puoi esaminare gli oggetti (esamina), raccogliere un oggetto (raccogli),rilasciare un oggetto(rilascia) o spostarti in una direzione (nord, est, sud, ovest). Scrivere (aiuto) per altri comandi.");
                            string input = Console.ReadLine().ToLower(); 
                            switch (input)
                            {
                                case "esamina":
                                    stanzaCorrente.EsaminaOggetti();
                                    break;
                                case "personaggi":
                                    stanzaCorrente.EsaminaPersonaggi();
                                    break;
                                case "inventario":
                                    MostraInventario();
                                    break;
                                case "parla":
                                    PrintSlowly("Con quale personaggio vuoi parlare?");
                                    string personaggioInput = Console.ReadLine().ToLower();
                                    Personaggio personaggioParlare = stanzaCorrente.Personaggi.Find(o => o.Nome.ToLower() == personaggioInput);
                                    if(personaggioParlare != null)
                                    {
                                        stanzaCorrente.ParlaConPersonaggio(personaggioParlare);
                                        if (personaggioParlare.OggettoAssegnato != null)
                                        {
                                            inventario.Add(personaggioParlare.OggettoAssegnato);
                                            PrintSlowly("Hai ottenuto l'oggetto: " + personaggioParlare.OggettoAssegnato.Nome + " Peso: " + personaggioParlare.OggettoAssegnato.Peso);
                                            personaggioParlare.OggettoAssegnato = null; // Remove the object from the character
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(personaggioParlare.Descrizione);
                                        Console.WriteLine("Non ci sono personaggi con cui parlare.");
                                    }
                                    break;
                                case "raccogli":
                                    Console.WriteLine("Quale oggetto vuoi raccogliere?");
                                    string oggettoInput = Console.ReadLine().ToLower();
                                    Oggetto oggettoRaccogliere = stanzaCorrente.Oggetti.Find(o => o.Nome.ToLower() == oggettoInput);
                                    if (oggettoRaccogliere != null)
                                    {
                                        if (CalcolaPesoInventario() + oggettoRaccogliere.Peso <= pesoLimiteInventario)
                                        {
                                            stanzaCorrente.RaccogliOggetto(oggettoRaccogliere, inventario);
                                        }
                                        else
                                        {
                                            PrintSlowly("L'inventario è troppo pieno per raccogliere questo oggetto.");
                                        }
                                    }
                                    else
                                    {
                                        PrintSlowly("L'oggetto specificato non è presente in questa stanza.");
                                    }
                                    break;
                                case "rilascia":
                                    Console.WriteLine("Quale oggetto vuoi rilasciare?");
                                    string oggettoRilasciaInput = Console.ReadLine().ToLower();
                                    Oggetto oggettoRilascia = inventario.Find(o => o.Nome.ToLower() == oggettoRilasciaInput);
                                    if (oggettoRilascia != null)
                                    {
                                        stanzaCorrente.RilasciaOggetto(oggettoRilascia, inventario);
                                    }
                                    else
                                    {
                                        Program.PrintSlowly("L'oggetto specificato non è presente nell'inventario.");
                                    }
                                    break;
                                case "nord":
                                    Coordinate nuovaPosizioneNord = new Coordinate(posizioneCorrente.X, posizioneCorrente.Y + 1);
                                    if (!stanze.ContainsKey(nuovaPosizioneNord))
                                    {
                                        PrintSlowly("Non puoi andare in questa direzione, la stanza non esiste.");
                                    }
                                    else
                                    {
                                        Stanza stanzaDestinazioneNord = stanze[nuovaPosizioneNord];
                                        if (!stanzaDestinazioneNord.Aperta)
                                        {

                                            Oggetto oggettoRichiesto = stanzaDestinazioneNord.OggettoRichiesto;// Verifica se l'inventario contiene un oggetto adatto per aprire la stanza
                                            bool oggettoRichiestoPresente = inventario.Any(oggetto => oggetto.Nome == oggettoRichiesto.Nome);
                                            if (oggettoRichiestoPresente)
                                            {
                                                PrintSlowly("La porta è chiusa. Vuoi utilizzare un oggetto per aprirla? (sì/no)");
                                                string risposta = Console.ReadLine().ToLower();

                                                if (risposta == "sì")
                                                {
                                                    // Utilizza l'oggetto per aprire la stanza
                                                    stanzaDestinazioneNord.ApriPorta();
                                                    posizioneCorrente = nuovaPosizioneNord;
                                                }
                                                else
                                                {
                                                    PrintSlowly("Decidi di non utilizzare un oggetto per aprire la porta.");
                                                }
                                            }
                                            else
                                            {
                                                PrintSlowly("La porta è chiusa. Devi trovare un oggetto adatto per aprirla.");
                                            }
                                        }
                                        else
                                        {
                                            posizioneCorrente = nuovaPosizioneNord;
                                        }
                                    }
                                    break;
                                case "est":
                                    Coordinate nuovaPosizioneEst = new Coordinate(posizioneCorrente.X + 1, posizioneCorrente.Y);
                                    if (!stanze.ContainsKey(nuovaPosizioneEst))
                                    {
                                        PrintSlowly("Non puoi andare in questa direzione, la stanza non esiste.");
                                    }
                                    else
                                    {
                                        Stanza stanzaDestinazioneEst = stanze[nuovaPosizioneEst];
                                        if (!stanzaDestinazioneEst.Aperta)
                                        {
                                            // Verifica se l'inventario contiene un oggetto adatto per aprire la stanza
                                            Oggetto oggettoRichiesto = stanzaDestinazioneEst.OggettoRichiesto;
                                            bool oggettoRichiestoPresente = inventario.Any(oggetto => oggetto.Nome == oggettoRichiesto.Nome);
                                            if (oggettoRichiestoPresente)
                                            {
                                                PrintSlowly("La porta è chiusa. Vuoi utilizzare un oggetto per aprirla? (sì/no)");
                                                string risposta = Console.ReadLine().ToLower();

                                                if (risposta == "sì")
                                                {
                                                    // Utilizza l'oggetto per aprire la stanza
                                                    stanzaDestinazioneEst.ApriPorta();
                                                    posizioneCorrente = nuovaPosizioneEst;
                                                }
                                                else
                                                {
                                                    PrintSlowly("Decidi di non utilizzare un oggetto per aprire la porta.");
                                                }
                                            }
                                            else
                                            {
                                                PrintSlowly("La porta è chiusa. Devi trovare un oggetto adatto per aprirla.");
                                            }
                                        }
                                        else
                                        {
                                            posizioneCorrente = nuovaPosizioneEst;
                                        }
                                    }
                                    break;
                                case "sud":
                                    Coordinate nuovaPosizioneSud = new Coordinate(posizioneCorrente.X, posizioneCorrente.Y - 1);
                                    if (!stanze.ContainsKey(nuovaPosizioneSud))
                                    {
                                        PrintSlowly("Non puoi andare in questa direzione, la stanza non esiste.");
                                    }
                                    else
                                    {
                                        Stanza stanzaDestinazioneSud = stanze[nuovaPosizioneSud];
                                        if (!stanzaDestinazioneSud.Aperta)
                                        {
                                            // Verifica se l'inventario contiene un oggetto adatto per aprire la stanza
                                            Oggetto oggettoRichiesto = stanzaDestinazioneSud.OggettoRichiesto;
                                            bool oggettoRichiestoPresente = inventario.Any(oggetto => oggetto.Nome == oggettoRichiesto.Nome);
                                            if (oggettoRichiestoPresente)
                                            {
                                                PrintSlowly("La porta è chiusa. Vuoi utilizzare un oggetto per aprirla? (sì/no)");
                                                string risposta = Console.ReadLine().ToLower();

                                                if (risposta == "sì")
                                                {
                                                    // Utilizza l'oggetto per aprire la stanza
                                                    Oggetto oggettoApertura = inventario.FirstOrDefault(oggetto => stanzaCorrente.RichiedeOggetto);
                                                    stanzaDestinazioneSud.ApriPorta();
                                                    posizioneCorrente = nuovaPosizioneSud;
                                                }
                                                else
                                                {
                                                    PrintSlowly("Decidi di non utilizzare un oggetto per aprire la porta.");
                                                }
                                            }
                                            else
                                            {
                                                PrintSlowly("La porta è chiusa. Devi trovare un oggetto adatto per aprirla.");
                                            }
                                        }
                                        else
                                        {
                                            posizioneCorrente = nuovaPosizioneSud;
                                        }
                                    }
                                    break;
                                case "ovest":
                                    Coordinate nuovaPosizioneOvest = new Coordinate(posizioneCorrente.X - 1, posizioneCorrente.Y);
                                    if (!stanze.ContainsKey(nuovaPosizioneOvest))
                                    {
                                        PrintSlowly("Non puoi andare in questa direzione, la stanza non esiste.");
                                    }
                                    else
                                    {
                                        Stanza stanzaDestinazioneOvest = stanze[nuovaPosizioneOvest];
                                        if (!stanzaDestinazioneOvest.Aperta)
                                        {
                                            // Verifica se l'inventario contiene un oggetto adatto per aprire la stanza
                                            Oggetto oggettoRichiesto = stanzaDestinazioneOvest.OggettoRichiesto;
                                            bool oggettoRichiestoPresente = inventario.Any(oggetto => oggetto.Nome == oggettoRichiesto.Nome);
                                            if (oggettoRichiestoPresente)
                                            {
                                                PrintSlowly("La porta è chiusa. Vuoi utilizzare un oggetto per aprirla? (sì/no)");
                                                string risposta = Console.ReadLine().ToLower();

                                                if (risposta == "sì")
                                                {
                                                    // Utilizza l'oggetto per aprire la stanza
                                                    Oggetto oggettoApertura = inventario.FirstOrDefault(oggetto => stanzaCorrente.RichiedeOggetto);
                                                    stanzaDestinazioneOvest.ApriPorta();
                                                    posizioneCorrente = nuovaPosizioneOvest;
                                                }
                                                else
                                                {
                                                    PrintSlowly("Decidi di non utilizzare un oggetto per aprire la porta.");
                                                }
                                            }
                                            else
                                            {
                                                PrintSlowly("La porta è chiusa. Devi trovare un oggetto adatto per aprirla.");
                                            }
                                        }
                                        else
                                        {
                                            posizioneCorrente = nuovaPosizioneOvest;
                                        }
                                    }
                                    break;
                                case "esci":
                                    continuaGioco = false;
                                    PrintSlowly("Hai deciso di uscire dal gioco. Arrivederci.");
                                    break;
                                case "aiuto":
                                    PrintSlowly("Altri comandi disponibili:\n -inventario: mostra gli oggetti nel proprio inventario.\n -esci: esci dal gioco.\n -personaggi: mostra l'elenco dei personaggi nella stanza.\n -parla: interagisci con i personaggi. \n -rilascia: rilascia oggetti nella stanza in cui ti trovi.");
                                    break;
                                case "salva":
                                    SaveGame("gioco.json");
                                    PrintSlowly("Hai salvato la partita.");
                                    break;
                                default:
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
        public static void PrintSlowly(string text, int speed = 10)
    {
        foreach(char c in text)
        {
            Console.Write(c);
            System.Threading.Thread.Sleep(speed);
        }
        Console.WriteLine();
    }
        static int CalcolaPesoInventario()
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
