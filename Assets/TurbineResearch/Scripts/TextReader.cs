using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TurbineResearch.Scripts
{
    public class TextReader
    {
        
        /* Reads a csv file with comma delimiters
         * INPUT:
         *      path - The path of the csv file. On Windows, right click the file and copy the file path
         *      skipLines (optional) - Skip the first 'n' lines of the file
         * OUTPUT:
         *      A 2D array of the values in a float list
         */
        
        public List<List<float>> ReadCSVFile(string path, int skipLines, char delimiter, int animationFrameLimit)
        {
            StreamReader strReader = new StreamReader(path);
            List<List<float>> table = new List<List<float>>();

            for (int i = 0; i < skipLines; i++)
            {
                strReader.ReadLine();
            }
            
            string dataString;
            int loopCounter = 0;
            while ((dataString = strReader.ReadLine()) != null && loopCounter < animationFrameLimit)
            {
                dataString = dataString.Trim();
                string[] dataValues = System.Text.RegularExpressions.Regex.Split(dataString, @"\s+");
                table.Add(new List<float>());
                foreach (string data in dataValues)
                {
                    table[loopCounter].Add(float.Parse(data));
                }

                loopCounter += 1;
            }

            return table;
        }
    
    }
}
