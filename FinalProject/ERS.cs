using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{

    /// <summary>
    /// The game Egyptian Rat Screw or ERS for short.
    /// </summary>
    /*
     * https://bicyclecards.com/how-to-play/egyptian-rat-screw/
     * 
     * Goal: Get all cards
     * Rules:
     *      1. All cards are dealt face-down in a clockwise circle starting with the player immediately to the dealer's left. Players may not look at their cards.
     *      2. Starting with the player left of the dealer, a card is taken from the top of their pile and placed face-up in the middle of the table.
     *      3. If the card played is a number card (2-10), the next player puts down a card. This continues until a face card or an ace is played.
     *      4. When a face card or ace is played, the next person in the sequence must play another face card or an ace in order for play to continue.
     *      5. If they do not do so within their allotted chance, the person who played the last face card or ace wins the pile. The winner begins the next round of play.
     *      6. The only thing that overrides the face card or an ace is the slap rule. The first person to slap the pile of cards when the slap rule is in effect wins the pile.
     *      Slap Rules:
     *          Double - When two cards of equal value are laid down consecutively e.g. 5, 5 or Q, Q
     *          Sandwich - When two cards of equivalent value are laid down consecutively, but with one card of different value between them. e.g. 5, 7, 5 or Q, 8, Q
     */
    class ERS : Game
    {
        private Queue<Card> playingField;
        private int cardsLeft = -1;
        private int turnResult = -1;                // 1 if stack is won, 0 if lost, -1 otherwise       

        public ERS():base(GameType.ERS)
        {
            playingField = new Queue<Card>();
            Deal();
            Console.WriteLine("Starting Game...");
            GameStart();
        }

        // Deals the cards out between the players
        // \return the player's hand as a List<Card>
        public override void Deal()
        {
            //Console.WriteLine("Initial Cards: " + deck.Cards.Count());
            int numCards = deck.Cards.Count();
            for (int i = 0; i < numCards; i++)
            {
                Card card = deck.Cards.Dequeue();
                if ((i % 2) == 0)
                {
                    playersHand.Enqueue(card);
                }
                else
                {
                    dealersHand.Enqueue(card);
                }
            }
            //Console.WriteLine("Player's Hand Size: " + playersHand.Count());
            //Console.WriteLine("Dealer's Hand Size: " + dealersHand.Count());
        }

        public override void GameStart()
        {
            bool end = false;
            int round = 0;
            string winner = "";
            while(!end)
            {
                Console.WriteLine("Round #: " + round);
                Turn("Player", playersHand);     // Player's turn
                Turn("Dealer", dealersHand);     // Dealer's turn
                round++;
                
                if(playersHand.Count() == 52 || dealersHand.Count() < 1)
                {
                    winner = "Player";
                    end = true;
                }
                if(dealersHand.Count() == 52 || playersHand.Count() < 1)
                {
                    winner = "Dealer";
                    end = true;
                }
            }
            Console.WriteLine(winner + " Wins!");
        }

        private void Turn(string name, Queue<Card> hand)
        {
            Console.WriteLine(name + "'s Turn");
            if (turnResult == 0)                                // If the previous player lost, the current player won
            {
                Console.WriteLine(name + " wins this stack!");
                TakePile(hand);
                turnResult = -1;
            }
            else
            {
                turnResult = PlayCard(hand);
                if (turnResult == 1)                                // If player won
                {
                    Console.WriteLine(name + " wins this stack!");
                    TakePile(hand);
                    turnResult = -1;
                }
            }
        }

        // Returns 1 if stack is won, 0 if lost, -1 otherwise
        private int PlayCard(Queue<Card> hand)
        {
            Card card = null;
            int value = 0, suit = 0;
            Dictionary<int, int> faceCards = new Dictionary<int, int>() 
            { 
                { 11, 1 }, { 12, 2 }, { 13, 3 }, { 14, 4 } 
            };

            if (hand.Count() > 0)
            {
                //Console.WriteLine("Cards left: " + hand.Count());
                card = hand.Dequeue();
                value = card.Value;
                playingField.Enqueue(card);
                Console.Write("Played: ");
                card.displayCard();
            }
            try
            {
                if (checkSlap(playingField.Reverse<Card>().Take<Card>(3)))
                {
                    Console.WriteLine("Slapped!");
                    return 1;
                }
                else
                {
                    if (value > 10)                     // If the card just played is a face card
                    {
                        cardsLeft = faceCards[value];
                    }
                    else
                    {
                        cardsLeft -= 1;                 // The card played was not a face card so the player has one less card to play another face card
                        if (cardsLeft > -2)
                        {
                            if (cardsLeft > 0)
                            {
                                return PlayCard(hand);
                            }
                            else
                            {
                                Console.WriteLine("Didn't Play a Face Card in Allotted Plays.");
                                return 0;
                            }
                        }
                        
                    }
                    
                }
            }
            catch
            {
            }
            return -1;

        }

        private void TakePile(Queue<Card> hand)
        {
            while (playingField.Count() > 0)
            {
                hand.Enqueue(playingField.Dequeue());
            }
        }

        private bool checkSlap(IEnumerable<Card> cards)
        {
            List<Card> card = cards.ToList();
            if(card.Count() < 2) { return false; }
            if(card.Count() == 2) { return card[0] == card[1]; }
            for (int i = 0; i < card.Count() - 2; i+=2)
            {
                // Checks for sandwiches
                if (card[i] == card[i + 2])
                {
                    return true;
                }
                // Checks for doubles
                if (card[i] == card[i + 1] || card[i + 1] == card[i + 2])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
