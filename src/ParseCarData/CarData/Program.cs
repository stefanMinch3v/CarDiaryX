using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarData
{
    class Program
    {
        static async Task Main()
        {
            var dict = new Dictionary<int, List<string>>();

            while (true)
            {
                Console.Write("Chose make id or STOP MAKE: ");
                var input = Console.ReadLine();

                if (input == "STOP!")
                {
                    break;
                }

                var makeId = int.Parse(input);

                dict.Add(makeId, new List<string>());

                while (true)
                {
                    var model = Console.ReadLine().Trim();

                    if (model == "STOP!")
                    {
                        break;
                    }

                    if (!Regex.IsMatch(model, "^[a-zA-Z0-9\\s!.-]*$"))
                    {
                        continue;
                    }

                    dict[makeId].Add(model);
                }
            }

            var index = 414;

            foreach (var kvp in dict)
            {
                var key = kvp.Key;
                var values = kvp.Value;

                for (int i = 0; i < values.Count; i++)
                {
                    var saveInput = $"({index}, {key}, '{values[i]}'),";

                    using (var writer = new StreamWriter("logs.txt", true))
                    {
                        await writer.WriteLineAsync(saveInput);
                    }

                    index++;
                }
            }

            Console.WriteLine("DONE!");
        }

        static async Task Correct()
        {
            var list = new List<string>();

            using (var reader = new StreamReader("correct.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var lineFromDocument = await reader.ReadLineAsync();
                    lineFromDocument = lineFromDocument.Remove(lineFromDocument.Length - 1);

                    var result = $"({lineFromDocument}),";
                    list.Add(result);
                }
            }

            foreach (var saveInput in list)
            {
                using (var writer = new StreamWriter("final.txt", true))
                {
                    await writer.WriteLineAsync(saveInput);
                }
            }
        }
    }
}
