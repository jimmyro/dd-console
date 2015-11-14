using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDconsole
{
    public class Ability
    {
        private string name, abbrev;
        private sbyte baseValue;

        public Ability(string myName, sbyte myBaseValue)
        {
            name = myName;
            abbrev = name.Substring(0, 3).ToUpper();
        }

        public string Name { get { return name; } set { name = value; } }
        public string Abbrev { get { return abbrev; } set { abbrev = value; } }
        public sbyte BaseValue { get { return baseValue; } set { baseValue = value; } }

        /*public int Value //add buffs to base value
        {
            get
            {
                int temp = baseValue;

                for (int i = 0; i < modifiers.Count; ++i)
                {
                    temp += modifiers[i].Value;
                }

                if (temp < 1)
                    return 1;
                else if (temp > 20)
                    return 20;
                else
                    return temp;
            }
        }*/
    }
}
