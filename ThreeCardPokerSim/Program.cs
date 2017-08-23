using System;
using ThreeCardPokerSim.Entities;

namespace TheSim
{
	public class Program
	{
		public const int MAXPLAYER = 7;
		public const int CURRENTPLAYER = 6;

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
				//GameRule.isPlay(aDealer);
				Console.ReadLine();
				Console.Clear();
			}
			return 0;
		}

		public static void CheckAgainstDealerAnd()
		{
			var aDealerPlayType = GameMechanics.PlayType(aDealer, true);
			if (aDealerPlayType != 0) //dealer play
			{
				for(int i = 0; i < aPlayers.Length; i++)
				{
					var aPlayerPlayType = GameMechanics.PlayType(aPlayers[i], false);
					if (aPlayerPlayType != 0) //player play too
					{
						if (aPlayerPlayType <= aDealerPlayType)
						{
							//continue here, how to determine if beat dealer
						}
					}
				}
			}
		}

		public static void initializeGame()
		{
			aDealer = new Player(true);
			aPlayers = new Player[CURRENTPLAYER];
			for (int i = 0; i < aPlayers.Length; i++)
			{
				aPlayers[i] = new Player();
				aPlayers[i].TotalMoney = 1000000;
				aPlayers[i].AnteBet = 3000;
				aPlayers[i].PairBonusBet = 1000;
				aPlayers[i].SixCardBonusBet = 1000;
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
				Console.Write(card.Rank + " " + card.Suit + ", ");
			}
			Console.WriteLine("Play with: " + GameMechanics.PlayType(aDealer, true));
			Console.WriteLine();

			for (int i = 0; i < aPlayers.Length; i++)
			{
				Console.WriteLine("Player" + (i + 1) + "'s " + "total money: " + aPlayers[i].TotalMoney);
				foreach (Card card in aPlayers[i].Hand)
				{
					Console.Write(card.Rank + " " + card.Suit + ", ");
				}
				Console.WriteLine("Play with: " + GameMechanics.PlayType(aPlayers[i], false));
				Console.WriteLine();
			}
		}

	}
}
