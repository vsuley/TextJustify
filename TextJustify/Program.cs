using System;

namespace TextJustify
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine("Incorrect number of parameters.");
                PrintHelpText();
                return 1;
            }

            int columns = 80;
            if (args.Length == 3)
            {
                columns = int.Parse(args[2]);
            }
            String inputFile = args[0];
            String outputFile = args[1];

            StreamParser streamParser = new StreamParser(inputFile, outputFile);
            Justifier justifier = new Justifier(streamParser, columns);

            justifier.Justify();
            streamParser.CloseStreams();

            return 0;
        }

        static void PrintHelpText()
        {
            Console.WriteLine(
                "\n=========================================================" +
                "\nTextJustify  is a  simple  program that justify  aligns a" +
                "\ngiven input file and writes the output to a  second file." +
                "\n" +
                "\nUsage:" +
                "\n$> TextJustify <input file> <output file> [width]" +
                "\n" +
                "\n(If  the number of  colunms  is not  explicitly specified" +
                "\nthen 80 is used as the default value.)");
        }
    }
}
