using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Drawing;

namespace Server
{
    class Text
    {
        public string name;
        public string text;
        public List<Page> pages;

        public Text(string name, string text)
        {
           this.name = name;
           this.text = text;
        }

        public List<Page> ParseToPages()
        {
            MatchCollection mcol = Regex.Matches(this.text, @"(\/[А-Яа-я]+[0-9]+)");
            string[] result = Regex.Split(this.text, @"\/[А-Яа-я]+[0-9]+", RegexOptions.IgnoreCase);
            var numAnPages = result.Zip(mcol, (p, n) => new { Page = p, Number = n });
            List<Page> pages = new List<Page>();
            foreach(var nap in numAnPages)
            {
                Page page = new Page();
                page.num = Convert.ToInt32(Regex.Match(nap.Number.Value, @"([1-9]+[0-9]*)").Value);
                page.page = nap.Page;
                pages.Add(page);
            }
            return pages;
        }

        public List<Text> ParseToParts(int partsCnt)
        {
            List<Text> textL = new List<Text>();
            int divider = this.text.Length / partsCnt;
            int lpoint = 0;
            int backDiff = 0;
            Regex regex = new Regex(@"(\/[А-Яа-я]+[0-9]+)");
            for (int i = 0; i < partsCnt - 1; i++)
            {
                Match m = regex.Match(this.text, divider * (i + 1) - backDiff);
                int length = m.Index - lpoint - 1;
                textL.Add(new Text($"Часть текста {this.name} под номером {i + 1}", this.text.Substring(lpoint, length)));
                lpoint = m.Index - 1;
                backDiff = lpoint - (divider * (i + 1));
            }
            textL.Add(new Text($"Часть текста {this.name} под номером {partsCnt}", this.text.Substring(lpoint)));
            return textL;
        }

        public void Parse(WordList wl)
        {
            Match m;
            string HRefPatter = @"([[^\wА-Яа-я\'\-]+)";
            m = Regex.Match(text, HRefPatter, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            while(m.Success)
            {
                int index = wl.IndexOf(m.Groups[1].Value);
                if (index < 0)
                {
                    wl.Add(m.Groups[1].Value, 1);
                    wl.count++;
                    //Console.WriteLine("Добавленно новое слово!\n");
                }
                if (index >= 0)
                {
                    wl.Increase(m.Groups[1].Value);
                    //Console.WriteLine("Увеличен счетчик уже имеющегося слова!\n");
                }
                m = m.NextMatch();
            }
        }
    }

    class WordList
    {
        //public ConcurrentQueue<Word> words;
        public List<Word> words;
        public int count;

        public WordList()
        {
            //words = new ConcurrentQueue<Word>();
            words = new List<Word>();
            count = 0;
        }
        public int IndexOf(string cword)
        {
            for (int i = 0; i < count; i++)
            {
                if((words.ElementAt(i).word.CompareTo(cword)) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Sort()
        {
            this.words.Sort((obj1, obj2) => obj1.count.CompareTo(obj2.count));
        }

        public void Add(string word, int count = 0)
        {
            //words.Enqueue(new Word(word, count));
            words.Add(new Word(word, count));
        }

        public void Increase(string word)
        {
            words.ElementAt(this.IndexOf(word)).count += 1;
        }

        public string Print()
        {
            string tmp = "";
            for(int i = 0; i < count; i++)
            {
                tmp += (words.ElementAt(i).Print() + "\n");
            }
            return tmp;
        }
    }
    class Word : IComparable<Word>
    {
        public string word;
        public int count;

        public Word(string word, int count)
        {
            this.word = word;
            this.count = count;
        }

        public int CompareTo(Word other)
        {
            if (Object.ReferenceEquals(other, this))
                return 0;
            else if (Object.ReferenceEquals(other, null))
                return 1;

            return this.count - other.count;
        }
        public string Print()
        {
            return (word + " -=- " + count);
        }
    }
}
