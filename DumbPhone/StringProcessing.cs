using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumbPhone
{
    class StringGuessr
    {
        /// <summary>
        /// Map of keys to the set of letters they can make.
        /// </summary>
        public static Dictionary<String, List<String>> numChar = new Dictionary<String, List<String>>()
        {
            {"1", new List<String>(){"1"} },
            {"2", new List<String>(){"a", "b", "c", "2"} },
            {"3", new List<String>(){"d", "e", "f", "3"} },
            {"4", new List<String>(){"g", "h", "i", "4"} },
            {"5", new List<String>(){"j", "k", "l", "5"} },
            {"6", new List<String>(){"m", "n", "o", "p", "6"} },
            {"7", new List<String>(){"q", "r", "s", "7"} },
            {"8", new List<String>(){"t", "u", "v", "8"} },
            {"9", new List<String>(){"w", "x", "y", "z", "9"} },
        };

        /// <summary>
        /// Hey! Get a dumbphone.
        /// </summary>
        public StringGuessr()
        {
        }

        /// <summary>
        /// Produces the list of strings associated with a key
        /// </summary>
        /// <param name="s">The Key.</param>
        /// <returns>List of single-character strings or the empty list.</returns>
        public List<String> getList(String s)
        {
            try
            {
                return numChar[s];
            }
            catch (KeyNotFoundException)
            {
                return new List<String>();
            }
        }
    }

    class StringPredictor
    {
        /// <summary>
        /// The actual data structure. Maps number key combinations to lists of words with that combination.
        /// </summary>
        public Dictionary<String, List<String>> words = new Dictionary<String, List<String>>();

        /// <summary>
        /// Map of letters to their corresponding number key
        /// </summary>
        public static Dictionary<string, int> charNum = new Dictionary<string, int>()
        {
            {"a", 2},
            {"b", 2},
            {"c", 2},
            {"d", 3},
            {"e", 3},
            {"f", 3},
            {"g", 4},
            {"h", 4},
            {"i", 4},
            {"j", 5},
            {"k", 5},
            {"l", 5},
            {"m", 6},
            {"n", 6},
            {"o", 6},
            {"p", 6},
            {"q", 7},
            {"r", 7},
            {"s", 7},
            {"t", 8},
            {"u", 8},
            {"v", 8},
            {"w", 9},
            {"x", 9},
            {"y", 9},
            {"z", 9},
        };

        /// <summary>
        /// Makes a new StringPredictor wrapped around the word file.
        /// </summary>
        /// <param name="filename">Location of the word file</param>
        public StringPredictor(String filename)
        {
            string[] wordList = System.IO.File.ReadAllLines(filename);

            for (int i = 0; i < wordList.Length; i++)
            {
                String searchKey = getKey(wordList[i]);
                if (words.Keys.Contains(searchKey))
                {
                    words[searchKey].Add(wordList[i]);
                }
                else
                {
                    words.Add(getKey(wordList[i]), new List<String>());
                    words[searchKey].Add(wordList[i]);
                }
            }
        }

        /// <summary>
        /// Uses LINQ to generate the list of words that match the prediction string.
        /// Runs pretty damn fast, if I do say so myself.
        /// </summary>
        /// <param name="search">The prediction string</param>
        /// <returns>The list of strings with that prefix</returns>
        public List<String> PredictWords(String search)
        {
            var predict = from word in words.Keys
                          where search.Length == word.Length &&
                          search == word.Substring(0, search.Length)
                          orderby word.Length ascending
                          select word;

            if (predict == null || predict.Count() <= 0)
            {
                predict = from word in words.Keys
                          where search.Length <= word.Length &&
                          search == word.Substring(0, search.Length)
                          orderby word.Length ascending
                          select word;
            }

            List<String> predictWords = new List<String>();

            foreach (String key in predict)
            {
                foreach (String value in words[key])
                {
                    predictWords.Add(value);
                }
            }

            return predictWords;
        }

        /// <summary>
        /// Uses a dictionary mapping letters to number keys to generate the key string for a given word.
        /// </summary>
        /// <param name="value">The word to process</param>
        /// <returns>The number key version of the passed word</returns>
        static String getKey(String value)
        {
            String key = "";
            for (int i = 0; i < value.Length; i++)
            {
                key += charNum[value.Substring(i, 1)];
            }

            return key;
        }

        /*
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            // Build the String Predictor
            sw.Start();
            StringPredictor predictor = new StringPredictor("\\\\conductor.honors.gccis.rit.edu\\home$\\isb4190\\documents\\visual studio 2012\\Projects\\StringTest\\StringTest\\words.txt");
            sw.Stop();
            Console.WriteLine("Build Time: " + sw.Elapsed + "\n");
            sw.Reset();

            Console.Write("Enter a number to search: ");
            String search = Console.ReadLine();

            //Get the list of strings.
            sw.Start();
            List<String> predictWords = predictor.PredictWords(search);
            sw.Stop();
            Console.WriteLine("\nTime elapsed: " + sw.Elapsed + "\n");

            //Output the strings.
            Char c = 'g';
            int index = 0;
            while (c != 'q' && c != 'Q' && index < predictWords.Count)
            {
                Console.WriteLine(predictWords[index]);
                index++;
                c = Console.ReadKey().KeyChar;
            }

            Console.WriteLine("Out of words");
            Console.ReadKey();
        }
        */
    }
}
