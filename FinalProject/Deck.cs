using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Deck
    {
        private Queue<Card> cards;
        Random random = new Random();
        private int TIMES_TO_SHUFFLE = 15;

        public Deck()
        {
            this.cards = buildDeck();
            shuffle();
            displayDeck();
        }

        public Queue<Card> Cards
        {
            get
            {
                return cards;
            }
        }

        private Queue<Card> buildDeck()
        {
            Queue<Card> deck = new Queue<Card>();
            for (int st = 0; st < 4; st++)
            {
                for (int val = 2; val < 15; val++)
                {
                    Card card = new Card(val, st);
                    deck.Enqueue(card);
                }
            }

            return deck;
        }

        public void displayDeck()
        {
            int count = 0;
            foreach (Card card in Cards)
            {
                count++;
                Console.Write(count.ToString() + " ");
                card.displayCard();
            }
        }

        public void shuffle()
        {
            List<Card> list = Cards.ToList<Card>();
            for (int i = 0; i < TIMES_TO_SHUFFLE; i++)
            {
                list.Shuffle<Card>(random);    
            }
            Cards.Clear();
            foreach(Card card in list)
            {
                Cards.Enqueue(card);
            }
        }
    }
}
