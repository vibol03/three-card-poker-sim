using System;
using ThreeCardPokerSim.Entities;

namespace TheSim
{
	public class Program
	{
		public const int MAXPLAYER = 7;
		public const int CURRENTPLAYER = 7;

		static Player aDealer;
		static Player[] aPlayers;
		static Deck aDeck;
		public static int Main(string[] agrs)
		{
			initializeGame();
			Console.SetWindowSize(100, 49);

			while (true)
			{
				DealCards();
				CheckAgainstDealer();
				CalculatePayout();
				printDealerAndPlayersHands();
				Console.ReadLine();
				Console.Clear();
			}
			return 0;
		}

		public static void CheckAgainstDealer()
		{
			for (int i = 0; i < aPlayers.Length; i++)
			{
				aPlayers[i].AnteMatchBet = (GameMechanics.PlayType(aPlayers[i], false) > 0) ? aPlayers[i].AnteBet : 0;
				aPlayers[i].beatDealer = GameMechanics.IsBeatDealer(aDealer, aPlayers[i]);
			}
		}

		public static void CalculatePayout() 
		{
			PlayTypes aDealerPlayType;
			PlayTypes aPlayerPlayType;
			for (int i = 0; i < aPlayers.Length; i++)
			{
				aDealerPlayType = GameMechanics.PlayType(aDealer, true);
				aPlayerPlayType = GameMechanics.PlayType(aPlayers[i], false);
				aPlayers[i].AnteBetPayout = GameMechanics.CalculateAnteBetPayout(aDealer, aPlayers[i], aDealerPlayType, aPlayerPlayType);
				aPlayers[i].PairPlusBetPayout = GameMechanics.CalculatePairPlusBetPayout(aPlayers[i], aPlayerPlayType);
				//aPlayers[i].SixCardBonusBetPayout = GameMechanics.CalculateSixCardBonusBetPayout(aDealer, aPlayers[i]);
				aPlayers[i].TotalMoney += (aPlayers[i].AnteBetPayout + aPlayers[i].PairPlusBetPayout);
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
				aPlayers[i].PairPlusBet = 3000;
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
				Console.WriteLine("Beat dealer: " + aPlayers[i].beatDealer);
				Console.WriteLine("Ante payout: " + aPlayers[i].AnteBetPayout);
				Console.WriteLine("Pair Plus Payout: " + aPlayers[i].PairPlusBetPayout);
				Console.WriteLine();

			}
		}

	}
}
