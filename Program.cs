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
         
            // Display “Document” followed by a blank line.
            displayWelcome();

            // Prompt the user for the name of the document. The user must enter a document name.
            document = getDocumentName();   

            // Prompt the user for the content that is to be in the document.
            // Check to see if the document name provided by the user already ends in “.txt”, do not append “.txt” to the name to create the filename. If it already has “.txt” on the end, use it as-is.
            // Save the content to a file in the current directory.
            // If an exception occurs, output the exception message.
            // If an exception does not occur, output “[filename] was successfully saved. The document contains [count] words.” and exit. [filename] and [count] are placeholders for the filename of the document and the number of words it contains.
            // After the document is saved or fails to save, prompt the user if they want to save another document. If they do, run the program again. If not, exit the program.

            Console.Write("Press ENTER to exit");
            Console.ReadLine();
        }

        static void displayWelcome() {
            Console.WriteLine("Document\n");
        }

        static string getDocumentName() {
            // input from console
            string input;
            // final document name
            string docName = "";

            do {
                Console.Write("Document Name: ");
                input = Console.ReadLine();
                if(containsInvalidCharacters(input)) {
                    input = null;
                }
            } while(input == null);    
            
            docName = checkFileExtension(input);

            try
            {
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

            if(input.EndsWith(".txt")) {
                output = input;
            }
            else {
                output = input + ".txt";
            }

            return output;
        }

        static bool containsInvalidCharacters(string input) {
            string[] invalids = {"<",">",":","\"","/","\\","|","?","*"};
            List<string> invalidCharacters = new List<string>(invalids);
            foreach(string character in invalidCharacters) {
                if(input.Contains(character)){
                    return true;
                }
            }
            
            return false;
        }
    }
}
