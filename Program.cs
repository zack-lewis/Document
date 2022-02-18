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
            bool writeSuccess = false;
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
                    writeSuccess = writeToFile(documentContents, document);
                }
                catch (Exception ex) {
                    showException(ex);
                }
                
                
                if(writeSuccess) {
                    try {
                        // Get the Document Word Count
                        documentWordCount = getWordCount(document);

                        // If an exception does not occur, output “[filename] was successfully saved. The document contains [count] words.” and exit. [filename] and [count] are placeholders for the filename of the document and the number of words it contains.
                        Console.WriteLine($"'{ document }' was successfully saved. The document contains {documentWordCount} words.");
                    }  
                    catch(Exception ex) {
                        showException(ex);
                    }
                }
                
                // After the document is saved or fails to save, prompt the user if they want to save another document. If they do, run the program again. If not, exit the program.
                Console.WriteLine("Would you like to add more to a file? (y/N)");
                string input = Console.ReadLine();

                repeat = consoleStringtoBool(input);

            } while (repeat);

            // Console.Write("Press ENTER to exit");
            // Console.ReadLine();
        }

        static void showMessage(string msg) {
            // If fancy formatting is wanted, add it here
            Console.WriteLine($"-> { msg }");
        }

        static void showException(Exception ex) {
            Console.WriteLine("--------------------");
            Console.WriteLine("Something went wrong");
            Console.WriteLine("--------------------");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.GetType());
            Console.WriteLine(ex.StackTrace);
        }

        static bool consoleStringtoBool(string input) {
            bool output = false; 
            try {
                input = input.ToUpper();
            }
            catch(Exception ex) {
                showException(ex);
            }

            if(input == "Y" || input == "YES") {
                output = true;
            }
            else {
                output = false;
            }
            return output;
        }

        static int getWordCount(string document) {
            int output = 0;
            try {
                // Open file for reading
                using(StreamReader documentStream = new StreamReader(document)) {
                    if(documentStream.Peek() <= 0) {
                        return 0;
                    }
                    // Set character buffer for reading from file
                    char[] character = null;
                    // set to True if previous character was whitespace
                    bool prevWhiteSpace = false;
                    while(documentStream.Peek() >= 0) {
                        character = new char[1];
                        documentStream.Read(character, 0, character.Length);
                        // Check for whitespace
                        if(Char.IsWhiteSpace(character[0])) {
                            // Make sure we don't count multiple whitespace in a row as multiple words
                            if(!prevWhiteSpace) {
                                output++;
                                prevWhiteSpace = true;
                            }
                        }
                        else {
                            prevWhiteSpace = false;
                        }
                    }
                }
            }
            catch(IOException ex) {
                Console.WriteLine($"Error reading from file. This could be cause by an empty file. \n{ex.Message}");

            }
            catch(Exception ex) {
                showException(ex);
            }
            return output;
        }

        static bool writeToFile(List<string> documentInput, string documentName) {
            bool output = false;

            if(documentInput.Count <= 0) {
                return false;
            }

            try
            {
                // Open document for writing
                using(StreamWriter documentStream = new StreamWriter(documentName, true)) {
                    // Write each line individually
                    foreach(string line in documentInput) {
                        documentStream.WriteLine(line);
                    }
                }
                output = true;
            }
            catch(IOException ex) {
                output = false;
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
            catch (Exception ex) {
                output = false;
                showException(ex);
            }

            return output;
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
                    showMessage("Filename cannot contain invalid characters");
                }
                if(input == "") {
                    input = null;
                    showMessage("Filename cannot be blank");
                }
            } while(input == null);    
            
            // ensure file has proper extension
            docName = checkFileExtension(input);

            try
            {
                if(!File.Exists(docName)) {
                    Console.WriteLine($"Creating file '{ docName }'");
                    File.Create(docName).Close();    
                }
            }
            catch (System.Exception ex) {
                showException(ex);
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
                input = input.TrimEnd('.');
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
