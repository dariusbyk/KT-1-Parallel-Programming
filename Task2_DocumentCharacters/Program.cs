using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Task2_DocumentCharacters
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "document.txt");
            string[] textLines = File.ReadAllLines(filePath);
            List<LineInfo> lines = new List<LineInfo>();

            for (int i = 0; i < textLines.Length; i++)
            {
                lines.Add(new LineInfo(i + 1, textLines[i]));
            }

            Parallel.ForEach(lines, line =>
            {
                line.CharacterCount = line.Text.Length;
                line.ThreadId = Thread.CurrentThread.ManagedThreadId;
            });

            int totalCharacters = 0;
            Console.WriteLine("Задание 2. Подсчёт символов в строках документа");
            Console.WriteLine("Строк в документе: " + lines.Count);
            Console.WriteLine();

            foreach (LineInfo line in lines)
            {
                Console.WriteLine("Строка " + line.Number + ": " + line.CharacterCount
                    + " символов, поток " + line.ThreadId);
                totalCharacters += line.CharacterCount;
            }

            Console.WriteLine();
            Console.WriteLine("Всего символов: " + totalCharacters);
            Console.WriteLine("Пробелы учитываются, переносы строк не учитываются.");
        }
    }

    internal class LineInfo
    {
        public int Number;
        public string Text;
        public int CharacterCount;
        public int ThreadId;

        public LineInfo(int number, string text)
        {
            Number = number;
            Text = text;
        }
    }
}
