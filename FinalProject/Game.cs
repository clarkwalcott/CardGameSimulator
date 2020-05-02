using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    abstract class Game
    {
        protected Deck deck;
        protected Queue<Card> dealersHand = new Queue<Card>();
        protected Queue<Card> playersHand = new Queue<Card>();

        public GameType Type { get; }

        public enum GameType
        {
            ERS
        }

        public Game(GameType game)
        {
            this.Type = game;

            deck = new Deck();
        }

        // Deals the cards out between the players
        public abstract void Deal();

        public abstract void GameStart();
    }
}
