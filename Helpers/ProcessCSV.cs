using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sicem_Blazor.Helpers;

public class ProcessCSV
{
    public static readonly string csvPattern = @",(?=(?:[^""]*""[^""]*"")*[^""]*$)";

    /// <summary>
    /// Parses a CSV file line by line and splits each line into columns, trimming quotes.
    /// </summary>
    /// <param name="file">The path to the CSV file.</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    /// <exception cref="IOException">Thrown if there is an issue reading the file.</exception>
    public static IEnumerable<string[]> LoadFile(string file)
    {
        var response = new List<string[]>();
        if(!File.Exists(file))
        {
            throw new FileNotFoundException($"The file '{file}' does not exist.");
        }
        foreach (var line in File.ReadLines(file))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            string[] columns = Regex.Split(line, csvPattern)
                .Select( item => item.Trim('\"'))
                .ToArray();
            response.Add(columns);
        }
        return response;
    }

}
