using System;
using System.Collections.Generic;
using System.IO;

namespace Document
{
    class Program
    {
        static void Main(string[] args)
        {
            string document = "";
            bool repeat = false;  
            int documentWordCount = 0;       
            // Display “Document” followed by a blank line.
            displayWelcome();

            do {
                // Prompt the user for the name of the document. The user must enter a document name.
                // Check to see if the document name provided by the user already ends in “.txt”, do not append “.txt” to the name to create the filename. If it already has “.txt” on the end, use it as-is.
                document = getDocumentName();   

                // Prompt the user for the content that is to be in the document.
                List<string> documentContents = new List<string>();
                documentContents = getDocumentContents();
                
                // Save the content to a file in the current directory.
                // If an exception occurs, output the exception message.
                try {
                    writeToFile(documentContents, document);
                }
                catch (System.Exception ex) {
                    Console.WriteLine(ex.StackTrace);
                    throw;
                }
                
                documentWordCount = getWordCount(document);
                
                // If an exception does not occur, output “[filename] was successfully saved. The document contains [count] words.” and exit. [filename] and [count] are placeholders for the filename of the document and the number of words it contains.
                Console.WriteLine($"'{ document }' was successfully saved. The document contains {documentWordCount} words.");

                // After the document is saved or fails to save, prompt the user if they want to save another document. If they do, run the program again. If not, exit the program.

            } while (repeat);

            Console.Write("Press ENTER to exit");
            Console.ReadLine();
        }


        static int getWordCount(string document) {
            int output = 0;
            char[] splitters = new char[] {' ', '\r', '\n'};
            using(StreamReader documentStream = new StreamReader(document)) {
                do {
                    string nextLine = documentStream.ReadLine();
                    nextLine.Split(splitters,StringSplitOptions.RemoveEmptyEntries);
                    output += nextLine.Length;
                    Console.WriteLine(nextLine);
                } while(!documentStream.EndOfStream);
            }
            return output;
        }

        static void writeToFile(List<string> documentInput, string documentName) {
            try
            {
                // Open document for writing
                using(StreamWriter documentStream = new StreamWriter(documentName, true)) {
                    // Write each line individually
                    foreach(string line in documentInput){
                        documentStream.WriteLine(line);
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        static List<string> getDocumentContents() {
            List<string> output = new List<string>();
            string input = "";
            // CTRL-Z and enter will return null, new lines do not return null
            Console.WriteLine("Enter the contents of the document. CTRL-Z and Enter to exit");

            do {
                input = Console.ReadLine();
                output.Add(input);
            } while(input != null);

            return output;
        }

        static void displayWelcome() {
            // display custom welcome screen
            // method can be tweaked depending on project requirements
            Console.WriteLine("Document\n");
        }

        static string getDocumentName() {
            // input from console
            string input;
            // final document name
            string docName = "";
            // get user input from console
            do {
                Console.Write("Document Name: ");
                input = Console.ReadLine();
                // check for invalid characters
                if(containsInvalidCharacters(input)) {
                    input = null;
                }
            } while(input == null);    
            
            // ensure file has proper extension
            docName = checkFileExtension(input);

            try
            {
                // USING statement automatically disposes of streamreader object
                using(StreamReader docStream = new StreamReader(docName)){
                    // Attempt to read the first line, ensuring access to the file is available. 
                    docStream.ReadLine();
                }
            }
            catch(FileNotFoundException){ 
                // If the file is not found, create it. Inform the user
                Console.WriteLine($"Creating file '{ docName }'");
                File.Create(docName);
            }
            catch (System.Exception) {
                // There shouldn't be any other exceptions but if so, throw them up the chain
                throw;
            }

            return docName;
        }

        static string checkFileExtension(string input) {
            string output;

            // check for .txt ending on input string
            if(input.EndsWith(".txt")) {
                output = input;
            }
            else {
                // append .txt to end of string if it does not exist
                output = input + ".txt";
            }

            return output;
        }

        static bool containsInvalidCharacters(string input) {
            // list of invalid characters in most environments
            string[] invalids = {"<",">",":","\"","/","\\","|","?","*"};
            List<string> invalidCharacters = new List<string>(invalids);

            // check input for each character in list
            foreach(string character in invalidCharacters) {
                if(input.Contains(character)){
                    return true;
                }
            }
            
            return false;
        }
    }
}
