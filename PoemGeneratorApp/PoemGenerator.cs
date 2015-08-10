using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemGeneratorApp
{
    public class PoemGenerator
    {
        private Dictionary<string, string[]> wordsArray = new Dictionary<string, string[]>();
        private Dictionary<string, string[]> rulesArray = new Dictionary<string, string[]>();

        List<string> LineRules = new List<string>();
        List<string> LineKeyRules = new List<string>();

        private Random rand = new Random();
        private int numLines = 0;
        public string poem;
        private string filePath;

        /// <summary>
        /// Sets the poem file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        public void SetPoemFile(string path)
        {
            filePath = Path.GetFullPath(path);
        }

        /// <summary>
        /// Reads the poem rules from a file.
        /// </summary>
        private void ReadPoemRules()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] sections = lines[i].Split(':');
                    string[] ruleDefinitions = sections[1].Trim().Split(' ');

                    switch (i)
                    {
                        case 0:
                            numLines = ruleDefinitions.Length;
                            break;
                        case 1:
                            LineRules = ruleDefinitions[0].Replace("<", "").Replace(">", "").Split('|').ToList();
                            LineKeyRules = ruleDefinitions[1].Split('|').ToList();
                            break;
                        default:
                            wordsArray.Add(sections[0], ruleDefinitions[0].Split('|'));
                            rulesArray.Add(sections[0], ruleDefinitions[1].Split('|'));
                            break;
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("The selected file was not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred reading the file. " + ex.Message);
            }
        }

        /// <summary>
        /// Generates the poem.
        /// </summary>
        /// <returns>The generator poem</returns>
        public string GeneratePoem()
        {
            ReadPoemRules();
            try
            {
                for (int i = 0; i < numLines; i++)
                {
                    bool startNewLine = false;
                    int randNum = rand.Next(LineRules.Count);
                    string rule = LineRules[randNum];

                    do
                    {
                        string word = GetWordByRule(rule);
                        poem += word + " ";

                        rule = GetRuleFromRule(rule);

                        if (rule.Contains("$"))
                        {
                            startNewLine = true;
                            poem += "\n";
                        }
                    } while (!startNewLine);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return poem;
        }

        /// <summary>
        /// Gets a randomly selected word by rule.
        /// </summary>
        /// <param name="rule">The rule whose list of rules to search.</param>
        /// <returns>A randomly selected word</returns>
        private string GetWordByRule(string rule)
        {
            int numWords = wordsArray[rule].Length;
            string word = wordsArray[rule][rand.Next(numWords)];

            return word;
        }

        /// <summary>
        /// Gets a randomly selected rule from a rule list.
        /// </summary>
        /// <param name="rule">The rule whose list of rules to search.</param>
        /// <returns>A randomly selected rule.</returns>
        private string GetRuleFromRule(string rule)
        {
            int numRules = rulesArray[rule].Length;
            string selectedRule = rulesArray[rule][rand.Next(numRules)];

            return selectedRule.Replace("<", "").Replace(">", "");
        }

        /// <summary>
        /// Clears the values of this instance.
        /// </summary>
        public void Clear()
        {
            wordsArray.Clear();
            rulesArray.Clear();
            poem = "";
            numLines = 0;
            LineRules.Clear();
            LineKeyRules.Clear();
        }
    }
}
