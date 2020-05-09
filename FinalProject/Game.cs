using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FinalProject
{
    abstract class Game
    {
        protected Canvas playingField;
        protected int numPlayers;

        protected Deck deck;
        protected Queue<Card> dealersHand = new Queue<Card>();
        protected Queue<Card> playersHand = new Queue<Card>();

        public GameType Type { get; }

        public enum GameType
        {
            ERS
        }

        public Game(GameType game, int numPlayers, Canvas playingField)
        {
            this.Type = game;
            this.numPlayers = numPlayers;
            this.playingField = playingField;

            deck = new Deck();
        }

        public abstract void setupPlayingField();

        // Deals the cards out between the players
        public abstract void Deal();

        public abstract void GameStart();
    }
}
