using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization.Formatters;

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

        public List<Page> ParseToPages(bool save_codes = false)
        {
            MatchCollection mcol = Regex.Matches(this.text, @"(\/[А-Яа-я]+[0-9]+)");
            string[] result = Regex.Split(this.text, @"\/[А-Яа-я]+[0-9]+", RegexOptions.IgnoreCase).Where(str => !String.IsNullOrEmpty(str)).ToArray();
            var numAnPages = result.Zip(mcol, (p, n) => new { Page = p, Number = n });
            List<Page> pages = new List<Page>();
            foreach(var nap in numAnPages)
            {
                Page page = new Page();
                page.num = Convert.ToInt32(Regex.Match(nap.Number.Value, @"([1-9]+[0-9]*)").Value);
                if (save_codes)
                    page.page = nap.Number.Value + "\n" + nap.Page;
                else
                    page.page = nap.Page;
                pages.Add(page);
            }
            return pages;
        }

        public void CutByPages(int from, int to)
        {
            Match m1 = Regex.Match(this.text, @$"(\/[А-Яа-я]+[0]*{from})");
            Match m2 = Regex.Match(this.text, @$"(\/[А-Яа-я]+[0]*{to + 1})");
            if (!m2.Success)
                return;
            else
                this.text = this.text.Substring(m1.Index - 1, m2.Index - 1);
        }

        public Text[] ParseToParts(int partsCnt)
        {
            this.pages = this.ParseToPages(true);
            //Text[] textes = new Text[partsCnt];
            int partLenght = (int) Math.Ceiling(this.pages.Count / (double)partsCnt);
            if (partLenght <= 0)
                partLenght = 1;
            Text[] textes = new Text[partsCnt];
            for (int i = 0; i < partsCnt; i++)
            {
                textes[i] = new Text($"Часть №{i + 1} произведения {this.name}", "");
            }
            int counter = 0;
            foreach (Page p in this.pages)
            {
                textes[counter / partLenght].text += p.page;
                counter++;
            }
            //var pages = Enumerable.Range(0, partsCnt)
            //    .Select(i => this.pages.Skip(i * partLenght).Take(partLenght));
            //for (int i = 0; i < pages.Count(); i++)
            //{
            //    for (int j = 0; j < page)
            //}
            return textes;
        }

        public void ParseToWords(WordList wl)
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
            for (int i = 0; i < words.Count; i++)
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
            this.words.Sort((obj1, obj2) => obj2.CompareTo(obj1));
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
        public string morph;

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
