using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace DDconsole
{
    class Program
    {
        /* TO-DO LIST
         * 1. Fix display in player creation + reduce ugly hard-coding
         * 2. Link abilities to health or mana
         * 3. Add attacks to each player
         * 4. Establish different kinds of damage
         * 5. Add items to each player's inventory
         * 6. 
         * 7. 
         * 
         * 
         */

        //declarations
        private static int playerCount;
        private static Player[] players;

        static void Main(string[] args)
        {
            //settings
            const int playerMinimum = 2, playerMaximum = 8, abilMinimum = 2,
                raceclassMinimum = 2;

            Console.Title = "Dungeons & Dragons Console";

            #region initialization
            ErrorWrite("Step 1. Set number of players for the entire game.","");

            playerCount = 0;
            while (!CheckBound(Prompt("Input number of players (" + playerMinimum + "-" + playerMaximum + ")"), 
                out playerCount, playerMinimum, playerMaximum)) { }

            players = new Player[playerCount]; //empty array of players
            #endregion

            Debug.WriteLine("Initialization complete; {0} players", playerCount);

            #region define abilities
            bool done = false;

            ErrorWrite("Step 2. Type the name of an ability to add it to the list.",
                       "        To remove a name, type its index number.","",
                       "        Basic health, mana, and money are built in.", "",
                       "        Type \"done\" to continue to the race editor.","");

            List<string> abilList = new List<string>();

            while (!done)
            {
                Console.Clear();
                Console.WriteLine("ABILITIES ({0}):", abilList.Count);

                for (int i = 0; i < abilList.Count; i++)
                {
                    Console.WriteLine("{0}. {2} ({1})", (i + 1), abilList[i], abilList[i].Substring(0, 3).ToUpper());
                }

                Console.WriteLine(); //visual empty line

                string input = Console.ReadLine().ToLower();
                int index;

                if (int.TryParse(input, out index))
                {
                    if (index < 1 || index > abilList.Count)
                        ErrorWrite("Index out of range");
                    else
                        abilList.RemoveAt(index - 1);
                }
                else
                {
                    if (input.Length < 4)
                    {
                        ErrorWrite("Ability name must be at least 4 characters.");
                        continue;
                    }

                    if (input == "done")
                    {
                        done = abilList.Count > abilMinimum;

                        if (!done)
                            ErrorWrite("At least " + abilMinimum + " abilities are required.");

                        continue;
                    }

                    abilList.Add(input);
                    Debug.WriteLine("Adding " + input + " to abilities.");
                    continue;
                }
            }
            string[] abilNames = abilList.ToArray<string>();
            #endregion

            Debug.WriteLine("Ability definitions complete; {0} new abilities", abilNames.Length);

            #region define races and classes
            ErrorWrite("Step 3. Type a name to begin creating a new race.","",
                       "        The race editor will ask for an integer modifier",
                       "        for each ability defined in the previous step.", "",
                       "        These modifiers will automatically factor into an",
                       "        individual character's ability stats.", "",
                       "        The base for all abilities begins as 10, or 100%.",
                       "        Inputting a 1 will start an ability at 11 base, or",
                       "        110% (10% race bonus.)  Negative integers work the",
                       "        same way: -3 starts an ability at 10 - 3 = 7 or 70%.","",
                       "        Abilities ultimately cannot evaluate to lower than",
                       "        zero or higher than twenty.","",
                       "        Consider that class bonuses will factor in too!","",
                       "        Type \"done\" to continue to the class editor, which",
                       "        works exactly the same way.", "");

            var races = RaceClassFunc(abilNames, "race", raceclassMinimum);
            var classes = RaceClassFunc(abilNames, "class", raceclassMinimum);
            #endregion

            Debug.WriteLine("Race and class definitions complete; {0} races, {1} classes", races.Count, classes.Count);

            #region create players
            ErrorWrite("Step 4. Fill in the information for each player.", "", 
                       "        NB: \"Player\" refers to a real-world person, while \"character\"",
                       "        refers to the digital profile the player controls.","");

            for (int i = 0; i < playerCount; i++)
            {
                Console.Clear();
                Console.WriteLine("CREATING PLAYER {0}:", i + 1);
                Console.WriteLine();

                players[i] = new Player();

                players[i].Name = Prompt("Input the name of Player " + (i + 1));
                Console.WriteLine();

                players[i].CharName = Prompt("Input the name of " + players[i].Name + "'s character");
                Console.Clear();

                #region display the options
                //display the options
                Console.WriteLine("RACES:\t\t\tCLASSES:");
                Console.WriteLine();
                for (int j = 0; j < Math.Max(races.Count, classes.Count); j++)
                {
                    var currentRace = races.ElementAtOrDefault<KeyValuePair<string, Dictionary<string, sbyte>>>(j);
                    var currentClass = classes.ElementAtOrDefault<KeyValuePair<string, Dictionary<string, sbyte>>>(j);

                    //write names
                    if (races.Count > j)
                        Console.Write(TabWrite(string.Format("{0}. {1}", j + 1, Capitalize(currentRace.Key)), 3));
                    else
                        Console.Write("\t\t\t");

                    if (classes.Count > j)
                        Console.Write("{0}. {1}", j + 1, currentClass.Key);

                    Console.WriteLine();

                    //write properties
                    if (races.Count > j)
                    {
                        string s = "    ";

                        foreach (var pair in currentRace.Value)
                        {
                            s += pair.Key.Substring(0, 3).ToUpper();

                            if (pair.Value >= 0)
                                s += "+";

                            s += pair.Value + " ";

                            Debug.WriteLine("s = " + s);
                        }

                        Console.Write(TabWrite(s, 5));
                    }
                    else
                        Console.Write("\t\t\t");

                    if (classes.Count > j)
                    {
                        string s = "    ";

                        foreach (var pair in currentClass.Value)
                        {
                            s += pair.Key.Substring(0, 3).ToUpper();

                            if (pair.Value >= 0)
                                s += "+";

                            s += pair.Value + " ";

                            Debug.WriteLine("s = " + s);
                        }

                        Console.WriteLine(TabWrite(s, 5));
                    }
                }
                Console.WriteLine();
                #endregion

                int raceIndex, classIndex;

                if (!CheckBound(Prompt("Input race number from above"), out raceIndex, 1, races.Count)
                    || !CheckBound(Prompt("Input class number from above"), out classIndex, 1, classes.Count))
                {
                    ErrorWrite("Invalid race or class numbers -- start again!");
                    continue;
                }

                players[i].RaceName = races.Keys.ElementAt<string>(raceIndex - 1);
                players[i].ClassName = races.Keys.ElementAt<string>(classIndex - 1);
                players[i].CharRace = races.Values.ElementAt<Dictionary<string, sbyte>>(raceIndex - 1);
                players[i].CharClass = races.Values.ElementAt<Dictionary<string, sbyte>>(classIndex - 1);

                int health, mana, money;

                if (!CheckBound(Prompt("Input health value for " + players[i].Name), out health, 1, 1000)
                    || !CheckBound(Prompt("Input mana value for " + players[i].Name), out mana, 1, 1000)
                    || !CheckBound(Prompt("Input money value for " + players[i].Name), out money, 0, int.MaxValue))
                {
                    ErrorWrite("Invalid health, mana, or money values -- start again!");
                    continue;
                }

                players[i].Initialize(abilNames);

                Console.Clear();
            }
            #endregion

            Debug.WriteLine("Player creation complete; {0} players created", playerCount);
        }

        public static Dictionary<string, Dictionary<string, sbyte>> RaceClassFunc(string[] myAbilNames, string type, byte minimum)
        {
            var dic = new Dictionary<string, Dictionary<string, sbyte>>();
            string[] abilNames = myAbilNames;
            bool esc = false;

            while (!esc)
            {
                Console.Clear();
                Console.WriteLine("Input name for {1} {0}:", dic.Count + 1, type);
                string input = Console.ReadLine().ToLower(); //note: race names won't be capitalized

                //special cases
                if (input == "done")
                {
                    if (dic.Count < minimum)
                    {
                        ErrorWrite("At least " + minimum + " " + type + " definitions are required.");
                        continue;
                    }
                    else
                    {
                        esc = true;
                        continue;
                    }
                }
                else if (dic.ContainsKey(input))
                {
                    ErrorWrite("A " + type + " \"" + input + "\" already exists.");
                    continue;
                }

                //populate
                dic.Add(input, new Dictionary<string, sbyte>());
                Console.WriteLine();

                for (int i = 0; i < abilNames.Length; i++)
                {
                    Console.Write("{0}: ", abilNames[i].Substring(0, 3).ToUpper());
                    int value;

                    if (!CheckBound(Console.ReadLine(), out value, -10, 10))
                    {
                        i--;
                        continue;
                    }

                    dic[input].Add(abilNames[i], (sbyte)value);
                }
            }

            return dic;
        }

        public static string Prompt(string dialogue)
        {
            Console.WriteLine();
            Console.WriteLine(dialogue + ": ");
            return Console.ReadLine();
        }

        public static bool CheckBound(string input, out int num, int lowerBound, int upperBound)
        {
            if (!int.TryParse(input, out num))
            {
                ErrorWrite("Please input an integer");
                return false;
            }

            if (num <= lowerBound || num >= upperBound)
            {
                ErrorWrite("Please input an integer between " + lowerBound + " and " + upperBound);
                return false;
            }

            return true;
        }

        public static void ErrorWrite(params string[] dialogues)
        {
            Console.Clear();

            foreach (string s in dialogues)
                Console.WriteLine(s);

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
            Console.Clear();
        }

        public static string TabWrite(string s, int tabs)
        {
            if (s.Length > 8 * tabs)
                return s.Substring(0, 8 * tabs);

            //return (s + ("\t" * (int)Math.Ceiling((8 * tabs - s.Length) / 8d)));
            return s + String.Concat(Enumerable.Repeat("\t", (int)Math.Ceiling((8 * tabs - s.Length) / 8d)));
        }

        /*public static void DrawRaceClass()
        {

        }*/

        public static void DrawPlayerProfile(Player p)
        {
            /* PROFILE FOR GADOL (Jimmy):
             * 
             * Race: Gadolinian
             * Class: Mage
             * 
             * CON: [||||||||||||        ]
             * 
             */
            Console.Clear();

            Console.WriteLine("PROFILE FOR {0} ({1}):", p.CharName.ToUpper(), Capitalize(p.Name));
            Console.WriteLine();
        }

        public static string Capitalize(string s)
        {
            return s.Substring(0, 1).ToUpper() + s.Remove(0, 1);
        }
    }
}
