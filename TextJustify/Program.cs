using System;

namespace TextJustify
{
    class Program
    {
        static int Main(string[] args)
        {
            int columns;
            String inputFile = null, outputFile = null;
            bool overwriteInput = false;

            if (args.Length == 1)
            {
                // Assume that only the input file was specified and nothing else
                columns = 80;
                inputFile = args[0];
                outputFile = inputFile + ".swp"; // I don't think this is being used.
                overwriteInput = true;
                
            } else if (args.Length == 2)
            {
                // Assume that input and output filenames were specified and
                // cols were omitted. Obviously this is not ideal because
                // maybe the user wants to overwrite the input file but does
                // want to specify custom number of columns. However, this will
                // do for now.
                inputFile = args[0];
                outputFile = args[1];
                columns = 80;
                overwriteInput = false;
            }
            else if (args.Length == 3)
            {
                // All specifiable inputs were added.
                inputFile = args[0];
                outputFile = args[1];
                columns = int.Parse(args[2]);
                overwriteInput = false;
            }
            else
            {
                Console.WriteLine("Incorrect number of parameters.");
                PrintHelpText();
                return 1;
            }

            StreamParser streamParser = new StreamParser(
                inputFile,
                outputFile,
                overwriteInput);
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
                "\n$> TextJustify <input file> [output file] [width]" +
                "\n" +
                "\nIf an output file is not specified then the program goes" +
                "\ninto file overwrite mode and will replace the input file" +
                "\nwith the result. Make sure you save the file *before*" +
                "\ninvoking the program." +
                "\n" +
                "\nIf  the number of  colunms  is not  explicitly specified" +
                "\nthen 80 is used as the default value.");
        }
    }
}
