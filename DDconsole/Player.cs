using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDconsole
{
    class Player
    {
        //fields
        private string name, charName, raceName, className;
        private Dictionary<string, sbyte> charRace, charClass;
        private Ability[] abilities;

        private int health, mana, money;

        private bool initialized;

        //constructor
        public Player()
        {
            initialized = false;
            
            name = string.Empty;
            charName = string.Empty;
            charRace = null;
            charClass = null;
        }

        //getters and setters
        public string Name { get { return name; } set { name = value; } }
        public string CharName { get { return charName; } set { charName = value; } }
        public string RaceName { get { return raceName; } set { raceName = value; } }
        public string ClassName { get { return className; } set { className = value; } }
        public Dictionary<string, sbyte> CharRace { get { return charRace; } set { charRace = value; } }
        public Dictionary<string, sbyte> CharClass { get { return charClass; } set { charClass = value; } }

        public int Health { get { return health; } set { health = value; } }
        public int Mana { get { return mana; } set { mana = value; } }
        public int Money { get { return money; } set { money = value; } }

        //methods
        public void Initialize(string[] abilNames)
        {
            if (!initialized && charRace != null && charClass != null)
            {
                abilities = new Ability[abilNames.Length];

                for (int i = 0; i < abilNames.Length; i++)
                {
                    sbyte baseVal = (sbyte)(10 + charRace[abilNames[i]] + charClass[abilNames[i]]);

                    if (baseVal < 1)
                        baseVal = 1;
                    else if (baseVal > 20)
                        baseVal = 20;

                    abilities[i] = new Ability(abilNames[i], baseVal);
                }
            }
        }
    }
}
