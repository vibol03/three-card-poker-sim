using System;
using ThreeCardPokerSim.Entities;

namespace TheSim
{
	public class Program
	{
		public const int MAXPLAYER = 7;
		public const int CURRENTPLAYER = 5;

		static Player aDealer;
		static Player[] aPlayers;
		static Deck aDeck;
		public static int Main(string[] agrs)
		{
            testProcedure();
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
			SixCardBonusPayouts aSixCardBonusType;
			for (int i = 0; i < aPlayers.Length; i++)
			{
				aDealerPlayType = GameMechanics.PlayType(aDealer, true);
				aPlayerPlayType = GameMechanics.PlayType(aPlayers[i], false);
				aSixCardBonusType = GameMechanics.SixCardBonusType(aDealer, aPlayers[i]);
				aPlayers[i].AnteBetPayout = GameMechanics.CalculateAnteBetPayout(aDealer, aPlayers[i], aDealerPlayType, aPlayerPlayType);
				aPlayers[i].PairPlusBetPayout = GameMechanics.CalculatePairPlusBetPayout(aPlayers[i], aPlayerPlayType);
				aPlayers[i].SixCardBonusBetPayout = GameMechanics.CalculateSixCardBonusBetPayout(aPlayers[i], aSixCardBonusType);
				aPlayers[i].TotalMoney += (aPlayers[i].AnteBetPayout + aPlayers[i].PairPlusBetPayout + aPlayers[i].SixCardBonusBetPayout);
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
				aPlayers[i].PairPlusBet = 10000;
				aPlayers[i].SixCardBonusBet = 10000;
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
				Console.WriteLine("Six Card Payout: " + aPlayers[i].SixCardBonusBetPayout);
				Console.WriteLine();

			}
		}

		public static void testProcedure()
		{
			Player aTestDealer = new Player(true);
			Player aTestPlayer = new Player();

			aTestDealer.Hand[0] = new Card()
			{
				Rank = Ranks.ACE,
				Suit = Suits.HEART
			};
			aTestDealer.Hand[1] = new Card()
			{
				Rank = Ranks.KING,
				Suit = Suits.HEART
			};
			aTestDealer.Hand[2] = new Card()
			{
				Rank = Ranks.QUEEN,
				Suit = Suits.HEART
			};

			aTestPlayer.Hand[1] = new Card()
			{
				Rank = Ranks.JACK,
				Suit = Suits.HEART
			};
			aTestPlayer.Hand[2] = new Card()
			{
				Rank = Ranks.TEN,
				Suit = Suits.HEART
            };
			aTestPlayer.Hand[0] = new Card()
			{
				Rank = Ranks.NINE,
				Suit = Suits.HEART
            };

			Console.WriteLine(GameMechanics.SixCardBonusType(aTestDealer, aTestPlayer));
			Console.ReadLine();
		}

	}
}
