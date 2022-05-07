using System;
using System.IO;
using System.Text;

namespace TextJustify
{
    internal class StreamParser
    {
        public StreamParser(string inputFile, string outputFile,
            bool overwriteInput)
        {
            InputFile = inputFile;
            OutputFile = outputFile;
            OverwriteInput = overwriteInput;
        }

        public string InputFile { get; }
        public string OutputFile { get; }
        public bool OverwriteInput { get; private set; }
        public bool EoF { get => this.InputStream.EndOfStream; }
        public StreamReader InputStream { get; private set; }
        public StreamWriter OutputStream { get; private set; }
        public bool LookForNewPara { get; private set; }

        internal void CloseStreams()
        {
            this.InputStream.Close();
            this.OutputStream.Close();

            if (OverwriteInput)
            {
                // We will create a swap file in case the user
                // is not happy with TJ's changes and want their
                // original back. Hide this backup file using a '.'
                // at the filename start and give it a '.backup'
                // extension.
                string backupFile = this.InputFile + ".backup";
                if (backupFile[0] != '.')
                {
                    backupFile = '.' + backupFile;
                }
                File.Replace(this.OutputFile, this.InputFile, backupFile);
            }
        }

        internal void Initialize()
        {
            this.InputStream = new StreamReader(this.InputFile);
            this.OutputStream = new StreamWriter(this.OutputFile);
            this.LookForNewPara = false;
        }

        /// <summary>
        /// This method helps the system read words from the input stream. It
        /// does not modify the stream in any way aside from advancing it. The
        /// method reads the input stream character at a time and evaluates when
        /// a word boundary is reached.
        /// - Consecutive spaces are ignored. Spaces are only good as word 
        ///   boundaries.
        /// - A single newline is treated as a space, but 2 adjacent newlines are
        ///   considered a new paragraph. (So, 4 consecutive newlines will be 2
        ///   paragraphs).
        /// </summary>
        /// <returns>
        /// A word without surrounding spaces or a newline character when such
        /// condition is met.
        /// </returns>
        internal string ReadWord()
        {
            StringBuilder sb = new StringBuilder();
            bool midWord = false;
            char c;

            while (!this.InputStream.EndOfStream)
            {
                c = (char)this.InputStream.Read();

                if (c == '\r')
                {
                    // Ignore carriage returns. This seems to be followed by \n
                    // in most normal cases and that \n should be enough for 
                    // the logic to work properly.
                    continue;
                }
                if (c == ' ')
                {
                    this.LookForNewPara = false;
                    if (midWord)
                    {
                        return sb.ToString();
                    } else
                    {
                        // Eat spaces for dinner.
                        continue;
                    }
                }

                if (c == '\n' || c == '\r')
                {
                    if (midWord)
                    {
                        this.LookForNewPara = true;
                        return sb.ToString();
                    }

                    if (this.LookForNewPara)
                    {
                        this.LookForNewPara = false;
                        return "\n";
                    }

                    this.LookForNewPara = true;
                    continue;
                }

                this.LookForNewPara = false;
                midWord = true;
                sb.Append(c);
            }

            // Ending up here means the last 'word' in the file ended with an EOF.
            // Just return what we have so far.
            return sb.ToString();
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
