using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
     *      4. When a face card (Ace included) is played, the next person in the sequence must play another face card (or Ace) in order for play to continue.
     *          a. Allotted chance (plays to get another face card): Jack - 1, Queen - 2, King - 3, Ace - 4
     *      5. If they do not do so within their allotted chance, the person who played the last face card or ace wins the pile. The winner begins the next round of play.
     *      6. The only thing that overrides the face card or an ace is the slap rule. The first person to slap the pile of cards when the slap rule is in effect wins the pile.
     *      Slap Rules:
     *          Double - When two cards of equal value are laid down consecutively e.g. 5, 5 or Q, Q
     *          Sandwich - When two cards of equivalent value are laid down consecutively, but with one card of different value between them. e.g. 5, 7, 5 or Q, 8, Q
     */
    class ERS : Game
    {
        private Queue<Card> pile;
        private int cardsLeft = -1;                 // Cards left for face card play
        private int turnResult = -1;                // 1 if stack is won, 0 if lost, -1 otherwise   
        Random rnd = new Random();


        public ERS(int numPlayers, Canvas playingField):base(GameType.ERS, numPlayers, playingField)
        {
            pile = new Queue<Card>();
            Deal();
            Console.WriteLine("Starting Game...");
            setupPlayingField();
            //GameStart();
        }

        // Seats the players around the playing field
        public override void setupPlayingField()
        {
            Ellipse table = playingField.FindName("table") as Ellipse;      // Would like to refactor later. Will break if table is renamed in XAML

            // Handles design
            playingField.Background = Brushes.Black;
            table.Fill = Brushes.Green;
            List<SolidColorBrush> colors = new List<SolidColorBrush>() { Brushes.Red, Brushes.Blue, Brushes.Yellow, Brushes.Orange, Brushes.Indigo, Brushes.Violet };
            colors.Shuffle<SolidColorBrush>(rnd);

            // Handles setup of player pieces and card pile
            double height = 40.0;
            double width = 40.0;

            double radius = (table.ActualWidth / 2);
            double border = 1;
            double angle = ((2 * Math.PI) / numPlayers);

            // Where the circle starts
            double x_naught = table.Margin.Left + radius - (width / 2) + border;
            double y_naught = table.Margin.Top + radius - (height/2) + border;

            double x = x_naught, y = y_naught;

            for (int i = 0; i < numPlayers; i++)
            {
                Ellipse player = new Ellipse() { Stroke = colors[i], Fill = colors[i], Width = width, Height = height };
                Label playerLabel = new Label() { Width = 2 * width, Height = height };
                ListBox pileListBox = new ListBox() { Width = 4 * width, Height = 4 * height };

                //Console.WriteLine(Math.Sin(i * angle));
                //Console.WriteLine(Math.Cos(i * angle));
                double sin = Math.Sin(i * angle);
                double cos = Math.Cos(i * angle);
                x = x_naught + (sin * radius);
                y = y_naught + (cos * radius);
                //Console.WriteLine(x);
                //Console.WriteLine(y);
                //Console.WriteLine();

                player.Margin = new Thickness(x, y, 0, 0);
                player.Name = "Player" + (i + 1).ToString();

                playerLabel.Margin = new Thickness(x - ((width/2)*sin) - 5, y - (height*cos), 0, 0);
                playerLabel.Content = player.Name;
                playerLabel.Name = "label" + (i + 1).ToString();

                pileListBox.Margin = new Thickness(x_naught - pileListBox.Width/2 + 20, y_naught - pileListBox.Height/2, 0, 0);
                pileListBox.FontSize = 20;
                pileListBox.Background = Brushes.PaleVioletRed;
                pileListBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                pileListBox.VerticalContentAlignment = VerticalAlignment.Center;

                playingField.Children.Add(player);
                playingField.Children.Add(playerLabel);
                playingField.Children.Add(pileListBox);
                playingField.UpdateLayout();                 // If we don't update the window first, the ActualHeight and ActualWidth values of the window will be zero
            }
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
            int value = 0;
            Dictionary<int, int> faceCards = new Dictionary<int, int>() 
            { 
                { 11, 1 }, { 12, 2 }, { 13, 3 }, { 14, 4 } 
            };

            if (hand.Count() > 0)
            {
                //Console.WriteLine("Cards left: " + hand.Count());
                card = hand.Dequeue();
                value = card.Value;
                pile.Enqueue(card);
                Console.Write("Played: ");
                card.displayCard();
            }
            try
            {
                if (checkSlap(pile.Reverse<Card>().Take<Card>(3)))
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
            while (pile.Count() > 0)
            {
                hand.Enqueue(pile.Dequeue());
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
                // Can add more rules to make game more interesting
            }
            return false;
        }
    }
}
