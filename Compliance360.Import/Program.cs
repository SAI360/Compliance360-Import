using System;
using System.Collections.Generic;

namespace Compliance360.ImportStarter
{
    /// <summary>
    /// This is a sample application that will POST a data file to the 
	/// C360 API for insertion/update to the designated C360 component.
    /// </summary>
    public class Program
    {
        /// <summary>y
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            var arguments = ParseArguments(args);

            var importer = new Importer(arguments);

            try 
            {
                importer.Import();
            }
            catch(Exception ex) 
            {
                Console.Out.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Parses the supplied arguments and populates the Arguments dictionary.
        /// </summary>
        /// <param name="args">The arguments to parse.</param>
        public static Dictionary<string, string> ParseArguments(string[] args)
        {
            string argKey = null;
            var arguments = new Dictionary<string, string>();

            foreach(var arg in args)
            {
                if (arg.StartsWith("-")) {
                    // this is an argument key like "-n" or "--filename"
                    argKey = arg;

                    // add the key to the dictionary
                    if (!arguments.ContainsKey(argKey)) {
                        arguments.Add(argKey, argKey);
                    }
                } else {
                    if (argKey != null) {
                        arguments[argKey] = arg;
                        argKey = null;
                    } else {
                        arguments[arg] = arg;
                    }
                }
            }

            return arguments;
        }
    }
}