using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = "C:\\Users\\Ennea\\Desktop\\TMP.txt";
            Console.WriteLine(file);
            string tmpText = "";
            try
            {
                if (File.Exists(file))
                {
                    using (StreamReader sr = new StreamReader(file, Encoding.Default))
                    {
                        tmpText = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Text text = new Text("NVM RIGHT NOW!", tmpText);
            Console.WriteLine($"name = {text.name}\n");
            Console.WriteLine($"Text = \n {text.text}\n");
            List<Text> textl = text.ParseToParts(Environment.ProcessorCount);
            List<WordList> listWordList = new List<WordList>();
            for (int l = 0; l < Environment.ProcessorCount; l++)
            {
                listWordList.Add(new WordList());
            }
            var startTime = System.Diagnostics.Stopwatch.StartNew();
            Task[] taskArray = new Task[Environment.ProcessorCount];
            for (int i = 0; i < textl.Count(); i++)
            {
                parseParams pp = new parseParams(listWordList.ElementAt(i), textl.ElementAt(i));
                taskArray[i] = new Task(() => Parse(pp));
                taskArray[i].Start();
            }
            Task.WaitAll(taskArray);
            //text.Parse(wl);
            WordList main = JoinWordLists(listWordList);
            //main.Sort();
            Console.WriteLine(main.Print());
            startTime.Stop();
            var resultTime = startTime.Elapsed;
            Console.WriteLine($"Result time to complete code: {resultTime}\n");
            startTime = System.Diagnostics.Stopwatch.StartNew();
            for (int j = 0; j < Environment.ProcessorCount; j++)
            {
                Concordance c = new Concordance();
                c.text = textl[j];
                c.size = 1;
                c.words = new List<string>{ "степь", "хагана", "хвост"};
                taskArray[j] = new Task(() => Conc(c));
                taskArray[j].Start();
            }
            Task.WaitAll(taskArray);
            startTime.Stop();
            resultTime = startTime.Elapsed;
            Console.WriteLine($"Result time to complete code: {resultTime}\n");
        }

        public static WordList JoinWordLists(List<WordList> lwl)
        {
            WordList main = lwl[0];
            Parallel.For(1, lwl.Count, i =>
            {
                Parallel.For(0, lwl[i].count, j =>
                {
                    int n;
                    if ((n = main.IndexOf(lwl[i].words[j].word)) > 0)
                    {
                        main.words[n].count += lwl[i].words[j].count;
                    }
                    else
                    {
                        main.Add(lwl[i].words[j].word, lwl[i].words[j].count);
                    }
                });
            });
            //for(int s = 1; s < lwl.Count - 1; s++)
            //{
            //    for(int j = 0; j < lwl[s].count; j++)
            //    {
            //        int n;
            //      if ((n = main.IndexOf(lwl[s].words[j].word)) > 0)
            //        {
            //            main.words[n].count += lwl[s].words[j].count;
            //        }
            //      else
            //        {
            //            main.Add(lwl[s].words[j].word, lwl[s].words[j].count);
            //        }
            //    }
            //}
            return main;
        }

        public static void Parse(object x)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} begin work\n");
            parseParams pp = (parseParams)x;
            Text txt = pp.txt;
            WordList wl = pp.wl;
            txt.Parse(wl);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} complete work\n");
        }

        public static void Conc(object x)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} begin work\n");
            Concordance c = (Concordance)x;
            c.Produce();
            Console.WriteLine(c.result);
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} complete work\n");
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
