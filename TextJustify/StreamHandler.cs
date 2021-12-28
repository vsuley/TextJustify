using System;
using System.IO;
using System.Text;

namespace TextJustify
{
    internal class StreamHandler
    {
        public StreamHandler(string inputFile, string outputFile)
        {
            InputFile = inputFile;
            OutputFile = outputFile;
        }

        public string InputFile { get; }
        public string OutputFile { get; }
        public bool EoF { get => this.InputStream.EndOfStream; }
        public StreamReader InputStream { get; private set; }
        public StreamWriter OutputStream { get; private set; }
        public bool PreviousWasWhitespace { get; private set; }

        internal void CloseStreams()
        {
            this.InputStream.Close();
            this.OutputStream.Close();
        }

        internal void Initialize()
        {
            this.InputStream = new StreamReader(this.InputFile);
            this.OutputStream = new StreamWriter(this.OutputFile);
        }

        /// <summary>
        /// This method helps the system read words from the input stream. It
        /// does not modify the stream in any way aside from advancing it. The
        /// method reads the input stream character at a time and evaluates when
        /// a word boundary is reached.
        /// </summary>
        /// <returns>
        /// A word without surrounding whitespace. If a sequence contains multi-
        /// ple whitespace characters, the first one is removed but each follow-
        /// ing whitespace is returned as an individual word.
        /// </returns>
        internal string ReadWord()
        {
            StringBuilder sb = new StringBuilder();
            char c;

            while (!this.InputStream.EndOfStream)
            {
                c = (char)this.InputStream.Read();
                string stringSoFar = sb.ToString();
                bool isCurrentWhiteSpace = Char.IsWhiteSpace(c);
                bool isStringSoFarEmpty = String.IsNullOrEmpty(stringSoFar);

                // Consequetive whitespaces
                if(this.PreviousWasWhitespace
                    && isCurrentWhiteSpace)
                {
                    this.PreviousWasWhitespace = true;
                    continue;
                    // sb.Append(c);
                    // return sb.ToString();
                }

                // If starting with a whitespace
                if(isStringSoFarEmpty
                    && isCurrentWhiteSpace
                    && !this.PreviousWasWhitespace)
                {
                    this.PreviousWasWhitespace = true;
                    continue;
                }

                // A word boundary
                if(isCurrentWhiteSpace
                    && !isStringSoFarEmpty
                    && !this.PreviousWasWhitespace)
                {
                    this.PreviousWasWhitespace = true;
                    return stringSoFar;
                }

                // Start or middle of a word
                if(!isCurrentWhiteSpace)
                {
                    sb.Append(c);
                    this.PreviousWasWhitespace = false;
                    continue;
                }
            }

            return null;
        }

        internal void WriteLine(string line)
        {
            this.OutputStream.WriteLine(line);
        }

        internal void Write(string line)
        {
            this.OutputStream.Write(line);
        }
    }
}