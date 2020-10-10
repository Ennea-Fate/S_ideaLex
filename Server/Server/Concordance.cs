using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Server
{
    class Concordance
    {
        public Text text;
        public List<string> words;
        public int size;
        public string result;

        public void Produce()
        {
            result = "";
            this.text.pages = this.text.ParseToPages();
            foreach (var page in this.text.pages)
            {
                page.ParseToSentences();
            }

            foreach (string word in words)
            {
                result += $"{word}\n";
                foreach (var page in this.text.pages)
                {
                    foreach(var sentence in page.sentences)
                    {
                        if(sentence.sentence.Contains(word))
                        {
                            result += $"Страница {page.num}, предложение {sentence.num}:\n";
                            for (int i = -this.size; i <= this.size; i++)
                            {
                                if((page.cnt > (sentence.num + i)) && ((sentence.num + 1) > 0))
                                {
                                    result += $"{page.sentences.ElementAt(sentence.num + i).sentence}";
                                }
                            }
                            result += "\n";
                        }
                    }
                }
            }

        }
    }

    class Page
    {
        public int num;
        public int cnt;
        public string page;
        public List<Sentence> sentences;

        public void ParseToSentences()
        {
            sentences = new List<Sentence>();
            string[] matches = Regex.Split(this.page, @"((?<=[\.!\?\n:])\s+)|(\s+\n+)");
            matches = matches.Where(str => !string.IsNullOrEmpty(str.Trim())).ToArray();
            for (int i = 0; i < matches.Length; i++)
            {
                sentences.Add(new Sentence(i, matches[i]));
                this.cnt++;
            }
        }

        public Text ToText()
        {
            string name = ($"Страница {this.num}");
            string text = this.page;
            return new Text(name, text);
        }
    }

    class Sentence
    {
        public int num;
        public string sentence;

        public Sentence(int num, string sentence)
        {
            this.num = num;
            this.sentence = sentence;
        }
    }

}
