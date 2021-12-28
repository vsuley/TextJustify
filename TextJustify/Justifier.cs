using System;
using System.Collections.Generic;
using System.Text;

namespace TextJustify
{
    internal class Justifier
    {
        public Justifier(StreamHandler streamHandler, int columns)
        {
            StreamHandler = streamHandler;
            Columns = columns;
        }

        public StreamHandler StreamHandler { get; }
        public int Columns { get; }

        internal void Justify()
        {
            StreamHandler.Initialize();

            String carryOver = null;
            List<string> lineWords = new List<string>();
            int charCount = -1; // -1 to account for no space after last word.
            while (!StreamHandler.EoF)
            {
                // Either grab the last left-over word or read a new one in
                // from the file.
                string word;
                if (!String.IsNullOrEmpty(carryOver))
                {
                    word = carryOver;
                    carryOver = null;
                }
                else
                {
                    word = StreamHandler.ReadWord();
                }
                lineWords.Add(word);
                charCount += word.Length + 1; // exta 1 for space after word.

                // If adding the last word pushed over the column count, reflow
                // the last word.
                if (charCount > Columns)
                {
                    int lastIndex = lineWords.Count - 1;
                    carryOver = lineWords[lastIndex];
                    lineWords.RemoveAt(lastIndex);
                    charCount -= (carryOver.Length + 1);

                    string line = this.LayoutWords(lineWords, charCount);
                    lineWords.Clear();
                    charCount = -1;
                    StreamHandler.WriteLine(line);
                }
                // In case the char count is perfectly aligned at column limit.
                else if (charCount == Columns)
                {
                    string line = this.LayoutWords(lineWords, charCount);
                    lineWords.Clear();
                    charCount = -1;
                    StreamHandler.WriteLine(line);
                }
            }

            // Write any leftover words that didn't meet the column limit.
            if (lineWords.Count > 0)
            {
                string line = this.LayoutWords(lineWords, charCount);
                lineWords.Clear();
                charCount = -1;
                StreamHandler.Write(line);
            }
        }

        private string LayoutWords(List<string> lineWords, int charCount)
        {
            string line = "";

            if (charCount == this.Columns)
            {
                // Perfect alignment, no extra spaces needed.
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < lineWords.Count - 1; i++)
                {
                    sb.Append(lineWords[i]);
                    sb.Append(" ");
                }
                sb.Append(lineWords[lineWords.Count - 1]);
                line = sb.ToString();
            }
            else if (charCount < this.Columns)
            {
                // Few columns short, we need to sprinkle in extra spaces.
                StringBuilder sb = new StringBuilder();
                int rawSpaces = lineWords.Count - 1;
                int extraSpaces = this.Columns - charCount;
                string[] adjustedSpaces =
                    this.DistributeSpaces(rawSpaces, extraSpaces);
                for (int i = 0; i < rawSpaces; i++)
                {
                    sb.Append(lineWords[i]);
                    sb.Append(adjustedSpaces[i]);
                }
                sb.Append(lineWords[lineWords.Count - 1]);
                line = sb.ToString();
            }
            else
            {
                throw new Exception("Invalid layout request.");
            }

            return line;
        }

        private string[] DistributeSpaces(int rawSpaces, int extraSpaces)
        {
            int totalSpaces = rawSpaces + extraSpaces;
            string[] adjustedSpaces = new string[rawSpaces];

            for (int i = 0; i < totalSpaces; i++)
            {
                adjustedSpaces[i % rawSpaces] += " ";
            }

            return adjustedSpaces;
        }
    }
}
