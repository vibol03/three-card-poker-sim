using System;
using ThreeCardPokerSim.Entities;

namespace TheSim
{
	public class Program
	{
		public const int MAXPLAYER = 7;
		public const int CURRENTPLAYER = 3;

		static Player aDealer;
		static Player[] aPlayers;
		static Deck aDeck;
		public static int Main(string[] agrs)
		{
			initializeGame();

			while(true)
			{
				DealCards();
				CheckAgainstDealer();
				printDealerAndPlayersHands();
				GameRule.isPlay(aDealer);
				Console.ReadLine();
				Console.Clear();
			}
			return 0;
		}

		public static void CheckAgainstDealer()
		{
			//if (_dealerPlay())
			//{

			//}
		}

		public static void initializeGame()
		{
			aDealer = new Player(true);
			aPlayers = new Player[CURRENTPLAYER];
			for (int i = 0; i < aPlayers.Length; i++)
			{
				aPlayers[i] = new Player();
				aPlayers[i].TotalMoney = 1000000;
			}
			aDeck = new Deck();
		}

		public static void DealCards()
		{
			aDeck = new Deck();
			int deckDealingIndex = 0;

			for (int i = 0; i < 3; i++)
			{
				aDealer.Hand[i] = aDeck.Cards[deckDealingIndex++];
				for (int j = 0; j < aPlayers.Length; j++)
				{
					aPlayers[j].Hand[i] = aDeck.Cards[deckDealingIndex++];
				}
			}
		}

		public static void printDealerAndPlayersHands()
		{
			Console.WriteLine("Dealer's Hand:");
			foreach (Card card in aDealer.Hand)
			{
				Console.WriteLine(card.Rank + " " + card.Suit);
			}
			Console.WriteLine();

			for (int i = 0; i < aPlayers.Length; i++)
			{
				Console.WriteLine("Player" + (i + 1) + "'s " + "Hand:");
				foreach (Card card in aPlayers[i].Hand)
				{
					Console.WriteLine(card.Rank + " " + card.Suit);
				}
				Console.WriteLine();
			}
		}

	}
}
