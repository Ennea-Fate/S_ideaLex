using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Collections.Concurrent;
using System.Data;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string dbConnectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ideaLex_developmentContext>();
            var options = optionsBuilder
                .UseNpgsql(dbConnectionString)
                .Options;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Console.WriteLine("Server successfuly start\n");
            new FunctionalServer(3002);
        }

        class Client
        {
            private void SendError(TcpClient Client, int Code)
            {
                Client.Close();
            }

            public Client(TcpClient Client)
            {
                Console.WriteLine($"Task №{Task.CurrentId} begin to work (client task)\n");
                string Request = "";
                byte[] Buffer = new byte[1024];
                int Count;
                while ((Count = Client.GetStream().Read(Buffer, 0, Buffer.Length)) > 0)
                {
                    Request += Encoding.Default.GetString(Buffer, 0, Count);
                    if(Request.IndexOf("\r\n\r\n") >= 0 || Request.Length > 4096)
                    {
                        break;
                    }
                }

                if(Request.Contains("vq"))
                {
                    Console.WriteLine($"Task №{Task.CurrentId} begin to do the VQ! (client task)\n");
                    string[] vq_request = Request.Substring(0, Request.Length - 9).Split("/").Where(str => !string.IsNullOrEmpty(str)).ToArray();
                    VqAsync(Convert.ToInt32(vq_request[1]));
                }
                if(Request.Contains("concordance"))
                {
                    Console.WriteLine($"Task №{Task.CurrentId} begin to do Conc! (client task)\n");
                    ConcordanceAsync(Request);
                }
                if(Request.Contains("grammar"))
                {
                    Console.WriteLine($"Task №{Task.CurrentId} begin to do Grammar! (client task)\n");
                    string[] grammar_request = Request.Split("/").Where(str => !string.IsNullOrEmpty(str)).ToArray();
                    GrammarAsync(Convert.ToInt32(grammar_request[1]));
                }
                Client.Close();
                Console.WriteLine($"Task №{Task.CurrentId} comple work (client task)\n");
            }
        }
        class FunctionalServer
        {
            TcpListener Listener;
            public FunctionalServer(int port)
            {
                Listener = new TcpListener(IPAddress.Any, port);
                Listener.Start();

                while (true)
                {
                    TcpClient client = Listener.AcceptTcpClient();
                    Task task = new Task(() => ClientThread(client));
                    task.Start();
                }
            }

            static void ClientThread(Object StateInfo)
            {
                new Client((TcpClient) StateInfo);
            }

            ~FunctionalServer()
            {
                if(Listener != null)
                {
                    Listener.Stop();
                }
            }
        }

        public static async void VqAsync(int textId)
        {
            Console.WriteLine($"Task №{Task.CurrentId} begin to work INSIDE VQ! (client task)\n");
            using (ideaLex_developmentContext db = new ideaLex_developmentContext())
            {
                var fv = db.FrequencyVocabularies.Where(f => f.Id == textId).FirstOrDefault();
                var book = db.Books.Where(b => b.Id == fv.BookId).FirstOrDefault();
                Text text = new Text(book.Name, book.Text);
                WordList[] wordListes = new WordList[Environment.ProcessorCount];
                Text[] textes = text.ParseToParts(Environment.ProcessorCount);
                Task<WordList>[] taskArray = new Task<WordList>[Environment.ProcessorCount];
                for (int i = 0; i < (Environment.ProcessorCount); i++)
                {
                    int elementid = i;
                    Task<WordList> task = Task.Run(() => Frequency(textes.ElementAt(elementid)));
                    taskArray[i] = task;
                    if (i == ((Environment.ProcessorCount) - 1))
                        break;
                }
                Task.WaitAll(taskArray);
                for (int i = 0; i < (Environment.ProcessorCount); i++)
                {
                    wordListes[i] = taskArray[i].Result;
                }
                WordList main = JoinWordLists(wordListes);
                main.Sort();
                string result = main.Print();
                fv.Text = result;
                db.Update(fv);
                db.SaveChanges();
            }
        }

        public static void ConcordanceAsync(string concordance_params)
        {
            Console.WriteLine($"Task №{Task.CurrentId} begin to work INSIDE CONCORDANCE! (client task)\n");
            string[] _params = concordance_params.Split("/");
            bool lemma = _params.Last().Contains("true");
            string word = _params[_params.Length - 2];
            var tmp = _params[2].Split(";");
            Tuple<int, int, int>[] books_params = new Tuple<int, int, int>[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                string[] _tmp = tmp[i].Split(":");
                books_params[i] = new Tuple<int, int, int>(Convert.ToInt32(_tmp[0]), Convert.ToInt32(_tmp[1]), Convert.ToInt32(_tmp[2]));
            }
            using (ideaLex_developmentContext db = new ideaLex_developmentContext())
            {
                Text[] textes = new Text[books_params.Length];
                Books[] books = new Books[books_params.Length];
                for(int i = 0; i < books_params.Length; i++)
                {
                    books[i] = db.Books.Where(b => b.Id == books_params[i].Item1).FirstOrDefault();
                    Text tmp_text = new Text(books[i].Name, books[i].Text);
                    tmp_text.CutByPages(books_params[i].Item2, books_params[i].Item3);
                    textes[i] = tmp_text;
                    //Console.WriteLine($"Name = {textes[i].name}\n{textes[i].text}\n");
                }

                Concordance[][] concordances = new Concordance[textes.Length][];
                int recommendedPartsCount = Environment.ProcessorCount / concordances.Length;
                Text[][] textes_parts = new Text[concordances.Length][];
                for (int i = 0; i < textes.Length; i++)
                {
                    concordances[i] = new Concordance[recommendedPartsCount];
                    textes_parts[i] = textes[i].ParseToParts(recommendedPartsCount);
                    for (int j = 0; j < recommendedPartsCount; j++)
                    {
                        concordances[i][j] = new Concordance();
                        textes_parts[i][j].name = textes[i].name;
                        concordances[i][j].text = textes_parts[i][j];
                        concordances[i][j].words.Add(word);
                        concordances[i][j].size = 1;
                    }
                }
                Task<Concordance>[][] taskArray = new Task<Concordance>[textes.Length][];
                for (int i = 0; i < textes.Length; i++)
                {
                    taskArray[i] = new Task<Concordance>[recommendedPartsCount];
                    for (int j = 0; j < recommendedPartsCount; j++)
                    {
                        Concordance c = concordances[i][j];
                        Task<Concordance> task = Task.Run(() => Conc(c));
                        taskArray[i][j] = task;
                        if (j == (recommendedPartsCount - 1))
                            break;
                    }
                    if (i == ((Environment.ProcessorCount) - 1))
                        break;
                }
                List<Task<Concordance>> TaskList = new List<Task<Concordance>>();
                for (int i = 0; i < concordances.Length; i++)
                {
                    for (int j = 0; j < concordances[i].Length; j++)
                    {
                        TaskList.Add(taskArray[i][j]);
                    }
                }
                Task.WaitAll(TaskList.ToArray());
                string result = "";
                for (int i = 0; i < textes.Length; i++)
                {
                    result += $"Произведение: {textes[i].name}\n";
                    for (int j = 0; j < recommendedPartsCount; j++)
                    {
                        result += concordances[i][j].result;
                        result += "\n";
                    }
                }
                Concordances ces = new Concordances
                {
                    Concordance = result,
                    IsItLemma = lemma,
                    Width = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                db.Concordances.Add(ces);
                db.SaveChanges();
            }
        }

        public static void GrammarAsync(int textId)
        {
            Console.WriteLine($"Task №{Task.CurrentId} begin to work INSIDE GRAMMAR! (client task)\n");
            using (ideaLex_developmentContext db = new ideaLex_developmentContext())
            {
                var grammar = db.GrammarDictionaries.Where(g => g.Id == textId).FirstOrDefault();
                var book = db.Books.Where(b => b.Id == grammar.BookId).FirstOrDefault();
                Text text = new Text(book.Name, book.Text);
                WordList[] wordListes = new WordList[Environment.ProcessorCount];
                Text[] textes = text.ParseToParts(Environment.ProcessorCount);
                Task<WordList>[] taskArray = new Task<WordList>[Environment.ProcessorCount];
                for (int i = 0; i < (Environment.ProcessorCount); i++)
                {
                    int elementid = i;
                    Task<WordList> task = Task.Run(() => Frequency(textes.ElementAt(elementid)));
                    taskArray[i] = task;
                    if (i == ((Environment.ProcessorCount) - 1))
                        break;
                }
                Task.WaitAll(taskArray);
                for (int i = 0; i < (Environment.ProcessorCount); i++)
                {
                    wordListes[i] = taskArray[i].Result;
                }
                WordList main = JoinWordLists(wordListes);
                main.Sort();
                int count = main.count;
                Grammar g = new Grammar();
                g.PrepareDecodeList();
                g.wordList = main;
                g.MorphAnalize();
                string result = "";
                foreach(Word w in main.words)
                {
                    result += w.word;
                    result += " - ";
                    result += w.morph.Trim() + "\n";
                }
                grammar.Dictionary = result;
                db.Update(grammar);
                db.SaveChanges();
            }
        }
        public static WordList JoinWordLists(WordList[] wordListes)
        {
            ConcurrentDictionary<string, int> concDict = new ConcurrentDictionary<string, int>();
            //ConcurrentQueue<WordList> main = new ConcurrentQueue<WordList>();
            //WordList main = new WordList();
            Parallel.For(0, wordListes.Length, i =>
            {
                Parallel.For(0, wordListes[i].count, j =>
                {
                    int n = 0;
                    if (concDict.ContainsKey(wordListes[i].words[j].word))
                    {
                        int value = 0;
                        concDict.TryGetValue(wordListes[i].words[j].word, out value);
                        concDict[wordListes[i].words[j].word] = wordListes[i].words[j].count + value;
                    }
                    else
                    {
                        concDict.TryAdd(wordListes[i].words[j].word, wordListes[i].words[j].count);
                    }
                });
            });
            WordList main = new WordList();
            foreach (var pair in concDict)
            {
                main.Add(pair.Key, pair.Value);
                main.count++;
            }
            return main;
        }

        public static WordList Frequency(Text txt)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} begin work\n");
            WordList wl = new WordList();
            txt.ParseToWords(wl);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} complete work\n");
            return wl;
        }

        public static Concordance Conc(Concordance c)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} begin work\n");
            c.Produce();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} complete work\n");
            return c;
        }

        public class parseParams
        {
            public WordList wl;
            public Text txt;

            public parseParams(WordList wl, Text txt)
            {
                this.wl = wl;
                this.txt = txt;
            }
        }

    }
}
