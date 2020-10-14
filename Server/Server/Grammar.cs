using System;
using System.Collections.Generic;
using System.Text;
using LemmatizerNET;
using System.IO;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Server
{
    class Grammar
    {
        public WordList wordList;
        public int size;
        public string result;
        private static List<string[]> decodeList = new List<string[]>();
        private static string[] newLineSeparator = { Environment.NewLine };
        private static string[] plateSheetSmall = { "С", "П", "МС", "Г", "ПРИЧАСТИЕ", "ДЕЕПРИЧАСТИЕ", "ИНФИНИТИВ", "МС-ПРЕДК", "МС-П", "ЧИСЛ", "ЧИСЛ-П", "Н", "ПРЕДК", "ПРЕДЛ", "СОЮЗ", "МЕЖД", "ЧАСТ", "ВВОД", "КР_ПРИЛ", "КР_ПРИЧАСТИЕ", "мр", "жр", "ср", "од", "но", "ед", "мн", "им", "рд", "дт", "вн", "тв", "пр", "зв", "2", "св", "нс", "пе", "нп", "дст", "стр", "нст", "прш", "буд", "пвл", "1л", "2л", "3л", "0", "кр", "сравн", "имя", "фам", "отч", "лок", "орг", "кач", "вопр", "относ", "дфст", "опч", "жарг", "арх", "проф", "аббр", "безбл", "*", "", "разг" };
        private static string[] plateSheetFull = { "Существительное", "Прилагательное", "местоимение-существительное", "глагол в личной форме", "причастие", "деепричастие",
                                           "инфинитив", "местоимение-предикатив", "местоименное прилагательное", "числительное (количественное)", "порядковое числительное", "	наречие",
                                           "предикатив", "предлог", "союз", "междометие", "частица", "вводное слово",
                                           "краткое прилагательное", "краткое причастие", "муж. род", "жен. род", "сред. род", "одуш.",
                                           "неодуш.", "ед. ч.", "мн. ч.", "Им. п.", "Род. п.", "Дат. п.",
                                           "Вн. п.", "Тв. п.", "Пр. п.", "Зв. п.", "2-ой падеж", "сов. вид",
                                           "не сов. вид", "переходный", "непереходный", "действ. зал.", "страд. зал.", "наст. время",
                                           "прош. время", "будущ. время", "повелительная форма", "1-е лицо", "2-е лицо", "3-е лицо",
                                           "неизменяемое", "краткое", "сравн. форма", "имя", "отчество", "фамилия",
                                           "локативное", "организация", "кач. прилаг.", "вопросительное", "относительное", "не имеет мнж. ч.",
                                           "опечатка", "жаргонизм", "архаиз", "профессионализм", "аббревиатура", "безличный", " ", "", "разговорное"};
        private static string[] plateSheetForDecode = ReadFile(@"C:\RML" + @"\Dicts\Morph\rgramtab.tab").Split(newLineSeparator, StringSplitOptions.RemoveEmptyEntries);


        public Grammar()
        {
        }

        public void PrepareDecodeList()
        {
            foreach (string str in plateSheetForDecode)
            {
                string[] tmp = str.Split(' ', ',');
                if (tmp[0] != "//")
                    decodeList.Add(tmp);
            }
            foreach (string[] str in decodeList)
            {
                for (int i = 2; i < str.Length; i++)
                {
                    int tmp = Array.IndexOf(plateSheetSmall, str[i]);
                    if (tmp >= 0)
                    {
                        str[i] = plateSheetFull[Array.IndexOf(plateSheetSmall, str[i])];
                    }
                }
            }
        }

        public WordList MorphAnalize()
        {
            LemmatizerNET.MorphLanguage lang = LemmatizerNET.MorphLanguage.Russian;
            LemmatizerNET.ILemmatizer lemma = LemmatizerNET.LemmatizerFactory.Create(lang);
            LemmatizerNET.FileManager manager = LemmatizerNET.FileManager.GetFileManager(@"C:\RML");
            lemma.LoadDictionariesRegistry(manager);

            Parallel.ForEach<Word>(wordList.words, current =>
            {
                LemmatizerNET.IParadigmCollection paradigma = lemma.CreateParadigmCollectionFromForm(current.word.ToLower(), false, false);
                if (paradigma.Count > 0)
                {
                    current.morph = decypherAncode(paradigma[0].SrcAncode);
                }
                else
                {
                    current.morph = "не опознаная форма слова";
                }
            });
            return wordList;
        }

        private string decypherAncode(string ancode)
        {
            //Console.WriteLine($"Ancode in = {ancode}");
            ancode = ancode.Substring(0, 2);
            //Console.WriteLine($"Ancode after = {ancode}");
            string decoded = "";
            for (int i = 0; i < decodeList.Count; i++)
            {
                if (ancode == decodeList[i][0])
                {
                    for (int j = 2; j < decodeList[i].Length; j++)
                    {
                        decoded += " ";
                        decoded += decodeList[i][j];
                        //Console.WriteLine($"Decoded = {decoded}");
                    }
                }
            }
            return decoded;
        }

        private static string ReadFile(string file)
        {
            string text = "";
            try
            {
                if (File.Exists(file))
                {
                    using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("windows-1251")))
                    {
                        text = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return text;
        }
    }
}
