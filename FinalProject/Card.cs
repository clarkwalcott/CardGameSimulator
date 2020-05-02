using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Card
    {
        private static Dictionary<int, string> Suits = new Dictionary<int, string>()
        {
            {0, "Spades"},
            {1, "Hearts"},
            {2, "Diamonds"},
            {3, "Clubs"}
        };

        private static Dictionary<int, string> Values = new Dictionary<int, string>()
        {
            {2, "Two"}, {3, "Three" }, {4, "Four" }, {5, "Five" }, {6, "Six" }, {7, "Seven" }, {8, "Eight" }, {9, "Nine" }, {10, "Ten" }, {11, "Jack" }, {12, "Queen"}, {13, "King" }, {14, "Ace" }
        };

        public Card(int value, int suit)
        {
            this.Value = value;
            this.Suit = suit;
        }

        public int Value { get; set; }

        public int Suit { get; set; }

        public void displayCard()
        {
            string val = null;
            string st = null;
            Values.TryGetValue(this.Value, out val);
            Suits.TryGetValue(this.Suit, out st);
            if((val == null) || (st == null))
            {
                return;
            }
            Console.WriteLine(val + " of " + st);
        }

    }
}
