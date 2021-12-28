using System;

namespace TextJustify
{
    class Program
    {
        static void Main(string[] args)
        {
            String inputFile = args[0];
            String outputFile = args[1];
            int columns = int.Parse(args[2]);

            StreamHandler streamHandler = new StreamHandler(inputFile, outputFile);
            Justifier justifier = new Justifier(streamHandler, columns);

            justifier.Justify();

            streamHandler.CloseStreams();
        }

    }
}
