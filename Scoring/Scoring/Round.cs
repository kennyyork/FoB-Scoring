using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scoring
{
    public class Round
    {
        public enum Types
        {
            Preliminary,
            Wildcard,
            Semifinals,
            Finals
        }

        public int Number { get; private set; }
        public Types Type { get; private set; }

        public Round(int number, Types type, Team red, Team green, Team blue, Team yellow)
        {
            Number = number;
            Type = type;
        }
    }
}
