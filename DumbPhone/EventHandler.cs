using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DumbPhone
{
    class EventHandler
    {
        private StringHandler stringHandler;
        private Boolean predictive;

        /// <summary>
        /// Control Shell to wrap around the String Handler and make the 
        /// appropriate function calls
        /// </summary>
        /// <param name="filename">The location of the file for text prediction</param>
        public EventHandler(String filename)
        {
            stringHandler = new StringHandler(filename);
            predictive = false;
        }

        /// <summary>
        /// Changes between prediction modes and resets the string processor.
        /// </summary>
        public void TogglePredictive()
        {
            stringHandler.Reset();
            predictive = !predictive;
        }

        /// <summary>
        /// Secret music stuff. Don't ask questions.
        /// </summary>
        /// <param name="filename">URL of current track</param>
        /// <returns>URL of next track</returns>
        public String GetNewUri(String filename)
        {
            return stringHandler.GetNewUri(filename);
        }

        /// <summary>
        /// Calls the appropriate button function and returns the result.
        /// </summary>
        /// <param name="sender">The button that was pressed</param>
        /// <param name="e">Fancy event things</param>
        /// <returns></returns>
        public String ButtonClick(Object sender, RoutedEventArgs e)
        {
            if (predictive)
            {
                return predictiveButtonClick(sender, e);
            }
            else
            {
                return nonpredictiveButtonClick(sender, e);
            }
        }

        /// <summary>
        /// Runs the dumb text generator.
        /// </summary>
        /// <param name="sender">The button pressed</param>
        /// <param name="e">C# event params</param>
        /// <returns>The sentance after the button press was registered.</returns>
        public String nonpredictiveButtonClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if (((String)b.Name) == "backspace")
            {
                stringHandler.DumbBackSpace();
            }
            else if (((String)b.Name) == "cycle") { }
            else if (((String)b.Name) == "space")
            {
                stringHandler.DumbSpace();
            }
            else
            {
                stringHandler.DumbButtonPress((String)b.Name);
            }

            return stringHandler.DumbGetSentence();
        }

        /// <summary>
        /// Runs the predictive text generator.
        /// </summary>
        /// <param name="sender">The button pressed</param>
        /// <param name="e">C# event params</param>
        /// <returns>The sentance after the button press was registered.</returns>
        public String predictiveButtonClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            if (((String)b.Name) == "backspace")
            {
                stringHandler.Backspace();
            }
            else if (((String)b.Name) == "cycle")
            {
                stringHandler.Cycle();
            }
            else if (((String)b.Name) == "space")
            {
                stringHandler.Space();
            }
            else if (((String)b.Name) == "b1")
            {
                return "";
            }
            else
            {
                stringHandler.AddLetter((String)b.Name);
            }

            return stringHandler.GetSentence();
        }

    }
}
