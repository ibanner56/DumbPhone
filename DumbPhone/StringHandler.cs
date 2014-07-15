// Author: Isaac Banner

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DumbPhone
{
    /// <summary>
    /// Manages button presses and makes the appropriate calls to 
    /// the string production classes.
    /// </summary>
    class StringHandler
    {
        private StringPredictor predictor;
        private StringGuessr guessr;

        /// <summary>
        /// Holds the finalized words
        /// </summary>
        private List<String> sentence;

        /// <summary>
        /// Predictive fields
        /// </summary>
        private String currentWord;
        private List<String> currList;
        private IEnumerator<String> wordEnumerator;

        /// <summary>
        /// Non-predictive fields
        /// </summary>
        private DateTime lastTime;
        private String lastButton;

        /// <summary>
        /// Creates a new String Handler
        /// </summary>
        /// <param name="s">The file location of the dictionary</param>
        public StringHandler(String file)
        {
            try
            {
                this.predictor = new StringPredictor(file);
                this.guessr = new StringGuessr();
                sentence = new List<String>();
            }
            catch (FileNotFoundException e)
            {
                Console.Error.WriteLine(e.Data);
                Environment.Exit(0);

            }
        }

        /// <summary>
        /// Generates a full sentence based off of words entered
        /// and the current word the enumerator is on.
        /// </summary>
        /// <returns>The full sentence</returns>
        public String GetSentence()
        {
            String result = "";
            for (int i = 0; i < sentence.Count; i++ )
            {
                result += sentence[i] + " ";
            }

            if (wordEnumerator != null && wordEnumerator.Current != null
                    && currentWord != "")
            {
                return result + wordEnumerator.Current;
            }
            else if (currentWord != null)
            {
                for (int i = 0; i < currentWord.Length; i++)
                {
                    result += "-";
                }
                return result;
            }
            else return "";
        }

        /// <summary>
        /// Builds the sentence based on the values input.
        /// </summary>
        /// <returns>The sentence that has been entered.</returns>
        public String DumbGetSentence()
        {
            String result = "";
            for (int i = 0; i < sentence.Count; i++)
            {
                result += sentence[i] + " ";
            }

            return result + currentWord;
        }

        /// <summary>
        /// Builds the sentence based on previous user input.
        /// </summary>
        /// <param name="s">The complete sentence</param>
        public void DumbButtonPress(String s)
        {
            s = s.Substring(1, 1);
            if ((DateTime.Now - lastTime).TotalMilliseconds < 500 
                        && lastButton == s && s != "1")
            {
                    wordEnumerator.MoveNext();
                    if(wordEnumerator.Current == null)
                    {
                        wordEnumerator.Reset();
                        wordEnumerator.MoveNext();
                    }
                    currentWord = currentWord.Substring(0, currentWord.Length - 1);
                    currentWord += wordEnumerator.Current;
            }
            else
            {
                lastButton = s;
                currList = guessr.getList(s);
                wordEnumerator = currList.GetEnumerator();
                wordEnumerator.MoveNext();
                currentWord += wordEnumerator.Current;
            }

            lastTime = DateTime.Now;
        }

        /// <summary>
        /// Finishes the word in dumb mode
        /// </summary>
        public void DumbSpace()
        {
            sentence.Add(currentWord);
            Reset();
        }

        /// <summary>
        /// Removes a character in dumb mode
        /// </summary>
        public void DumbBackSpace()
        {
            if (currentWord != null && currentWord.Length > 0)
            {
                currentWord = currentWord.Substring(0, currentWord.Length - 1);
            }
            else if (sentence.Count > 0)
            {
                currentWord = sentence[sentence.Count - 1];
                sentence.RemoveAt(sentence.Count - 1);
            }
        }

        /// <summary>
        /// Removes a letter or a full word, depending on whether the 
        /// editor is in the middle of a word.
        /// </summary>
        public void Backspace()
        {
            if (currentWord != null && currentWord.Length > 0)
            {
                currentWord = currentWord.Substring(0, currentWord.Length - 1);
                currList = predictor.PredictWords(currentWord);
                wordEnumerator = currList.GetEnumerator();
                wordEnumerator.MoveNext();
            }
            else if(sentence.Count > 0)
            {
                sentence.RemoveAt(sentence.Count - 1);
            }
        }

        /// <summary>
        /// Cycles between words that fit the current prefix.
        /// </summary>
        public void Cycle()
        {
            if (wordEnumerator != null)
            {
                wordEnumerator.MoveNext();
                if (wordEnumerator.Current == null)
                {
                    wordEnumerator.Reset();
                    wordEnumerator.MoveNext();
                }
            }
        }

        /// <summary>
        /// Adds a space and locks in the current word.
        /// </summary>
        public void Space()
        {
            if (wordEnumerator != null 
                    && wordEnumerator.Current != null)
            {
                sentence.Add(wordEnumerator.Current);
                Reset();
            }
        }

        /// <summary>
        /// Adds a letter to the current word and updates the word list.
        /// </summary>
        /// <param name="s">The letter to add</param>
        public void AddLetter(String s)
        {
            s = s.Substring(1, 1);
            currentWord += s;
            currList = predictor.PredictWords(currentWord);
            wordEnumerator = currList.GetEnumerator();
            wordEnumerator.MoveNext();
        }

        /// <summary>
        /// Resets the fields.
        /// </summary>
        public void Reset()
        {
            currList = null;
            currentWord = "";
            wordEnumerator = null;
            lastButton = "";
            lastTime = DateTime.Now;
        }

        /// <summary>
        /// This part's a secret.
        /// </summary>
        /// <param name="uri">The old URI</param>
        /// <returns>The new URI</returns>
        public String GetNewUri(String uri)
        {
            int track = int.Parse(uri.Substring(9, 1)) + 1;
            if (track > 3)
                track = 1;
            String p1 = uri.Substring(0, 9)
                + track + uri.Substring(10, 4);
            return p1;
        }
    }
}
