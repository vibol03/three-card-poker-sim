using System.Linq;

namespace ThreeCardPokerSim.Entities
{
	public static class GameMechanics
	{
		public static bool IsPlay(Player player, bool isDealer)
		{
			var playHand = player.Hand;

			playHand = playHand.OrderByDescending(item => item.Rank).ToArray();

			return IsHighCard(playHand, isDealer) ||
					IsPair(playHand) ||
					IsFlush(playHand) ||
					IsStraight(playHand) ||
					IsThreeOfAKind(playHand) ||
					IsStraightFlush(playHand) ||
					IsMiniRoyalFlush(playHand);
		}

		public static PlayTypes PlayType(Player player, bool isDealer)
		{
			var playHand = player.Hand;

			playHand = playHand.OrderByDescending(item => item.Rank).ToArray();

			if (IsMiniRoyalFlush(playHand)) return PlayTypes.MINI_ROYAL_FLUSH;
			if (IsStraightFlush(playHand)) return PlayTypes.STRAIGHT_FLUSH;
			if (IsThreeOfAKind(playHand)) return PlayTypes.THREE_OF_A_KIND;
			if (IsStraight(playHand)) return PlayTypes.STRAIGHT;
			if (IsFlush(playHand)) return PlayTypes.FLUSH;
			if (IsPair(playHand)) return PlayTypes.ONE_PAIR;
			if (IsHighCard(playHand, isDealer)) return PlayTypes.HIGH_CARD;

			return 0;
		}

		public static bool IsBeatDealer(Player aDealer, Player aPlayer)
		{
			var aDealerPlayType = PlayType(aDealer, true);
			var aPlayerPlayType = PlayType(aPlayer, false);

			if (aPlayerPlayType > aDealerPlayType) return true;

			aPlayer.Hand = aPlayer.Hand.OrderByDescending(item => item.Rank).ToArray();
			aDealer.Hand = aDealer.Hand.OrderByDescending(item => item.Rank).ToArray();
			// if both dealer and player play
			if (aDealerPlayType != 0 && aPlayerPlayType != 0)
			{
				// check if their playtype are the same, if so, check which hand has bigger card
				if (aPlayerPlayType == aDealerPlayType)
				{
					switch (aPlayerPlayType)
					{
						case PlayTypes.HIGH_CARD:
						case PlayTypes.THREE_OF_A_KIND:
						case PlayTypes.STRAIGHT:
						case PlayTypes.STRAIGHT_FLUSH:
						case PlayTypes.FLUSH:
							return aPlayer.Hand[0].Rank > aDealer.Hand[0].Rank;
						case PlayTypes.ONE_PAIR:
							var dealerPairRank = (aDealer.Hand[0].Rank == aDealer.Hand[1].Rank) ? aDealer.Hand[0].Rank : aPlayer.Hand[2].Rank;
							var playerPairRank = (aPlayer.Hand[0].Rank == aPlayer.Hand[1].Rank) ? aPlayer.Hand[0].Rank : aPlayer.Hand[2].Rank;
							return playerPairRank > dealerPairRank;
						default:
							return false;
					}
				}
			}

			return false;
		}

		public static bool IsHighCard(Card[] hand, bool isDealer)
		{
			hand = hand.OrderByDescending(item => item.Rank).ToArray();
			if (isDealer)
			{
				return (hand[0].Rank >= Ranks.QUEEN);
			}
			return (hand[0].Rank >= Ranks.QUEEN && hand[1].Rank >= Ranks.SIX);

		}

		public static bool IsPair(Card[] hand)
		{
			return (hand[0].Rank == hand[1].Rank || hand[0].Rank == hand[2].Rank || hand[1].Rank == hand[2].Rank);
		}

		public static bool IsFlush(Card[] hand)
		{
			return (hand[0].Suit == hand[1].Suit && hand[1].Suit == hand[2].Suit);
		}

		public static bool IsStraight(Card[] hand)
		{
			hand = hand.OrderByDescending(item => item.Rank).ToArray();
			return (hand[0].Rank - 1 == hand[1].Rank && hand[1].Rank - 1 == hand[2].Rank) ||
					(hand[0].Rank == Ranks.ACE && hand[1].Rank == Ranks.THREE && hand[2].Rank == Ranks.TWO);
		}

		public static bool IsThreeOfAKind(Card[] hand)
		{
			return (hand[0].Rank == hand[1].Rank && hand[1].Rank == hand[2].Rank);
		}

		public static bool IsStraightFlush(Card[] hand)
		{
			return IsStraight(hand) && IsFlush(hand);
		}

		public static bool IsMiniRoyalFlush(Card[] hand)
		{
			return (hand[0].Rank == Ranks.ACE && hand[1].Rank == Ranks.KING && hand[2].Rank == Ranks.QUEEN) && IsFlush(hand);
		}
	}
}
