using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace FEC
{
    class FecParser
    {
        // BEGIN CONSTANT GLOBALS

        private const string PATH = "C:\\Users\\c12louis.bloom\\Documents\\FEC\\";
        private const string INPUT_FILENAME = "disambiguated_identities.json";
        private const string LOG_FILENAME = "log.txt";
        private const string INPUT_PATH = PATH + INPUT_FILENAME;
        private const string LOG_PATH = PATH + LOG_FILENAME;

        // END CONSTANT GLOBALS


        // BEGIN MUTABLE GLOBALS

        // END MUTABLE GLOBALS

        public void HandleRecord(string json, int recordNum)
        {
            try
            {
                FecClasses.DisambiguatedId record = JsonConvert.DeserializeObject<FecClasses.DisambiguatedId>(json);
            }
            catch (Exception e)
            {
                using (StreamWriter w = File.AppendText(LOG_PATH))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Error processing record #" + recordNum);
                    sb.AppendLine(e.Message);
                    sb.AppendLine(json);
                    //string prettyJson = GetPrettyPrintedJson(json);
                    //sb.AppendLine(prettyJson);
                    Log(sb.ToString(), w);
                }
            }
        }

        // Given a JSON string, returns a prettified JSON string
        public static string GetPrettyPrintedJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        private void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            w.WriteLine();
            w.WriteLine(logMessage);
            w.WriteLine("-------------------------------------------------------------");
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Parsing " + INPUT_FILENAME + "...");

            // Clears the log file
            System.IO.File.WriteAllText(LOG_PATH, String.Empty);

            FecParser parser = new FecParser();

            // Reads through every line in the JSON file and passes it to a
            // JSON record handler
            string line;
            int count = 0;
            System.IO.StreamReader sr = new System.IO.StreamReader(INPUT_PATH);
            while ((line = sr.ReadLine()) != null)
            {
                if (count % 1000 == 0)
                    Console.WriteLine("Processing element " + count + "...");
                parser.HandleRecord(line, count);

                count++;
            }
            sr.Close();

            Console.WriteLine("*****************");
            Console.WriteLine("Finished parsing!");
            Console.WriteLine("*****************");

            // Pauses program execution until the user hits Enter
            Console.ReadLine();
        }
    }
}
