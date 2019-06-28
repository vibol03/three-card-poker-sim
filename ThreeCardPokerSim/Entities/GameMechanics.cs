using System;
using System.Collections.Generic;
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

		public static long CalculateAnteBetPayout(Player aDealer, Player aPlayer, PlayTypes aDealerPlayType, PlayTypes aPlayerPlayType)
		{
			var payoutAmount = 0L;
			
			if (aDealerPlayType > 0 && aPlayerPlayType > 0) // if both the dealer and the player play
			{
				if (aPlayer.beatDealer)
				{
					payoutAmount = aPlayer.AnteBet + aPlayer.AnteMatchBet;
					payoutAmount += CalculateAnteBonus(aPlayer, aPlayerPlayType);
				}
				else
				{
					payoutAmount = -(aPlayer.AnteBet + aPlayer.AnteMatchBet);
				}
			}
			else if (aDealerPlayType == 0 && aPlayerPlayType > 0)
			{
				payoutAmount += aPlayer.AnteBet;
				payoutAmount += CalculateAnteBonus(aPlayer, aPlayerPlayType);
			}
			else if (aPlayerPlayType == 0)
			{
				payoutAmount -= aPlayer.AnteBet;
			}

			return payoutAmount;
		}

		private static long CalculateAnteBonus(Player aPlayer, PlayTypes aPlayType)
		{
			switch (aPlayType) // determine additional ante bonus
			{
				case PlayTypes.STRAIGHT:
					return aPlayer.AnteBet * (long)AntePayouts.STRAIGHT;
				case PlayTypes.THREE_OF_A_KIND:
					return aPlayer.AnteBet * (long)AntePayouts.THREE_OF_A_KIND;
				case PlayTypes.STRAIGHT_FLUSH:
					return aPlayer.AnteBet * (long)AntePayouts.STRAIGHT_FLUSH;
				default:
					return 0;
			}
		}

		public static long CalculatePairPlusBetPayout(Player aPlayer, PlayTypes aPlayerPlayType)
		{
			switch (aPlayerPlayType) // determine additional ante bonus
			{
				case PlayTypes.ONE_PAIR:
					return aPlayer.PairPlusBet * (long)PairPlusPayouts.ONE_PAIR;
				case PlayTypes.FLUSH:
					return aPlayer.PairPlusBet * (long)PairPlusPayouts.FLUSH;
				case PlayTypes.STRAIGHT:
					return aPlayer.PairPlusBet * (long)PairPlusPayouts.STRAIGHT;
				case PlayTypes.THREE_OF_A_KIND:
					return aPlayer.PairPlusBet * (long)PairPlusPayouts.THREE_OF_A_KIND;
				case PlayTypes.STRAIGHT_FLUSH:
					return aPlayer.PairPlusBet * (long)PairPlusPayouts.STRAIGHT_FLUSH;
				default:
					return -aPlayer.PairPlusBet;
			}
		}

		public static long CalculateSixCardBonusBetPayout(Player aPlayer, SixCardBonusPayouts SixCardBonusType)
		{
			switch (SixCardBonusType) // determine additional ante bonus
			{
				case SixCardBonusPayouts.THREE_OF_A_KIND:
					return aPlayer.SixCardBonusBet * (long)SixCardBonusPayouts.THREE_OF_A_KIND;
				case SixCardBonusPayouts.STRAIGHT:
					return aPlayer.SixCardBonusBet * (long)SixCardBonusPayouts.STRAIGHT;
				case SixCardBonusPayouts.FLUSH:
					return aPlayer.SixCardBonusBet * (long)SixCardBonusPayouts.FLUSH;
				case SixCardBonusPayouts.FULL_HOUSE:
					return aPlayer.SixCardBonusBet * (long)SixCardBonusPayouts.FULL_HOUSE;
				case SixCardBonusPayouts.FOUR_OF_A_KIND:
					return aPlayer.SixCardBonusBet * (long)SixCardBonusPayouts.FOUR_OF_A_KIND;
				case SixCardBonusPayouts.STRAIGHT_FLUSH:
					return aPlayer.SixCardBonusBet * (long)SixCardBonusPayouts.STRAIGHT_FLUSH;
				case SixCardBonusPayouts.ROYAL_FLUSH:
					return aPlayer.SixCardBonusBet * (long)SixCardBonusPayouts.ROYAL_FLUSH;
				default:
					return -aPlayer.SixCardBonusBet;
			}
		}

		public static SixCardBonusPayouts SixCardBonusType(Player aDealer, Player aPlayer)
		{
			//merge and sort the player and dealer's cards
			var aSixCardHand = new Card[6];
			aDealer.Hand.CopyTo(aSixCardHand, 0);
			aPlayer.Hand.CopyTo(aSixCardHand, 3);
			aSixCardHand = aSixCardHand.OrderByDescending(item => item.Rank).ToArray();
            //royal flush not working
			if (IsRoyalFlushInSixCardHand(aSixCardHand)) return SixCardBonusPayouts.ROYAL_FLUSH;
			if (IsStraightFlushInSixCardHand(aSixCardHand)) return SixCardBonusPayouts.STRAIGHT_FLUSH;
			if (IsFourOfAKindInSixCardHand(aSixCardHand)) return SixCardBonusPayouts.FOUR_OF_A_KIND;
			if (IsFullHouseInSixCardHand(aSixCardHand)) return SixCardBonusPayouts.FULL_HOUSE;
			if (IsFlushInSixCardHand(aSixCardHand)) return SixCardBonusPayouts.FLUSH;
			if (IsStraightInSixCardHand(aSixCardHand)) return SixCardBonusPayouts.STRAIGHT;
			if (IsThreeOfAKindInSixCardHand(aDealer, aDealer, aSixCardHand)) return SixCardBonusPayouts.THREE_OF_A_KIND;

			return 0;
		}

		public static bool IsThreeOfAKindInSixCardHand(Player aDealer, Player aPlayer, Card[] aSixCardHand)
		{
			if (PlayType(aDealer, true) == PlayTypes.THREE_OF_A_KIND || PlayType(aPlayer, false) == PlayTypes.THREE_OF_A_KIND)
			{
				return false;
			}

			foreach(Ranks rank in Enum.GetValues(typeof(Ranks)))
			{
				var rankCount = 0;
				foreach(Card card in aSixCardHand)
				{
					rankCount += (card.Rank == rank) ? 1 : 0;
				}
				if (rankCount == 3) return true;
			}

			return false;
		}

		public static bool IsStraightInSixCardHand(Card[] aSixCardHand)
		{
			//getting rids of pairs.
			aSixCardHand = aSixCardHand.GroupBy(x => x.Rank).Select(x => x.First()).ToArray();
			if (aSixCardHand.Length < 5) return false; //if there are two pairs or three of a kind, we can be sure that there's no straight

			if (aSixCardHand[0].Rank == Ranks.ACE) //check for special case with the straight beginning with ace
			{
				if (aSixCardHand.Length == 6)
				{
					for (int i = 2; i < aSixCardHand.Length; i++)
					{
						if (i < aSixCardHand.Length - 1 && aSixCardHand[i].Rank - aSixCardHand[i + 1].Rank != 1)
							return false;
					}
				}
			}

			for (int i = 1; i < aSixCardHand.Length - 1; i++)
			{
				if (i < aSixCardHand.Length - 2 && aSixCardHand[i].Rank - aSixCardHand[i + 1].Rank != 1)
					return false;
			} // if we're past this loop, that means the middle cards form a straight, now just need to check the head and the tail

			return	((aSixCardHand[0].Rank - aSixCardHand[1].Rank == 1) ||
					(aSixCardHand[aSixCardHand.Length - 2].Rank - aSixCardHand[aSixCardHand.Length - 1].Rank == 1)
					|| (aSixCardHand[0].Rank == Ranks.ACE && aSixCardHand[aSixCardHand.Length - 1].Rank == Ranks.TWO));
		}

		public static bool IsFlushInSixCardHand(Card[] aSixCardHand)
		{
			foreach (Suits suit in Enum.GetValues(typeof(Suits)))
			{
				var suitCount = 0;
				foreach (Card card in aSixCardHand)
				{
					suitCount += (card.Suit == suit) ? 1 : 0;
					if (suitCount == 5) return true;
				}
			}

			return false;
		}

		public static bool IsFullHouseInSixCardHand(Card[] aSixCardHand)
		{
			var aDeDupedHand = aSixCardHand.GroupBy(x => x.Rank).Select(x => x.First()).ToArray();
			var hasThreeOfAKind = false;
			var hasAPair = false;

			foreach(Card card in aDeDupedHand)
			{
				hasThreeOfAKind = aSixCardHand.Count(item => item.Rank == card.Rank) == 3;
				hasAPair = aSixCardHand.Count(item => item.Rank == card.Rank) == 2;
				if (hasThreeOfAKind && hasAPair) return true;
			}

			return false;
		}

		public static bool IsFourOfAKindInSixCardHand(Card[] aSixCardHand)
		{
			foreach (Ranks rank in Enum.GetValues(typeof(Ranks)))
			{
				var rankCount = 0;
				foreach (Card card in aSixCardHand)
				{
					rankCount += (card.Rank == rank) ? 1 : 0;
				}
				if (rankCount == 4) return true;
			}

			return false;
		}

		public static bool IsStraightFlushInSixCardHand(Card[] aSixCardHand)
		{
			//getting rids of pairs.
			var anArrayOfTwoHands = new List<Card[]>();
			anArrayOfTwoHands.Add(aSixCardHand.GroupBy(x => x.Rank).Select(x => x.First()).ToArray());
			anArrayOfTwoHands.Add(aSixCardHand.GroupBy(x => x.Rank).Select(x => x.Last()).ToArray());

			if (anArrayOfTwoHands[0].Length < 5) return false; //if there are two pairs or three of a kind, we can be sure that there's no straight

			foreach(Card[] hand in anArrayOfTwoHands)
			{
				if (_straightFlushSubFunctionCheck(hand)) return true;
			}
			return false;
		}

		public static bool IsRoyalFlushInSixCardHand(Card[] aSixCardHand)
		{
			var iis = IsStraightFlushInSixCardHand(aSixCardHand);
			return aSixCardHand[0].Rank == Ranks.ACE && (aSixCardHand[4].Rank == Ranks.TEN || aSixCardHand[5].Rank == Ranks.TEN) && iis;
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
							if (aPlayer.Hand[0].Rank > aDealer.Hand[0].Rank)
								return true;
							else if (aPlayer.Hand[0].Rank == aDealer.Hand[0].Rank && aPlayer.Hand[1].Rank > aDealer.Hand[1].Rank)
								return true;
							else if (aPlayer.Hand[1].Rank == aDealer.Hand[1].Rank && aPlayer.Hand[2].Rank > aDealer.Hand[2].Rank)
								return true;
							else
								return false;
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


		private static bool _straightFlushSubFunctionCheck(Card[] aSixCardHand)
		{
			if (aSixCardHand[0].Rank == Ranks.ACE) //check for special case with the straight beginning with ace
			{
				if (aSixCardHand.Length == 6)
				{
					for (int i = 2; i < aSixCardHand.Length; i++)
					{
						// the first condition is to make sure we dont go out of array index
						if (i < aSixCardHand.Length - 1 &&
							(aSixCardHand[i].Rank - aSixCardHand[i + 1].Rank != 1 || aSixCardHand[i].Suit != aSixCardHand[i + 1].Suit))
							return false;
					}
				}
			}

			if (aSixCardHand.Length == 5)
			{
				for (int i = 1; i < aSixCardHand.Length - 1; i++)
				{
					// the first condition is to make sure we dont go out of array index
					if (i < aSixCardHand.Length - 2 &&
						(aSixCardHand[i].Rank - aSixCardHand[i + 1].Rank != 1 || aSixCardHand[i].Suit != aSixCardHand[i + 1].Suit))
						return false;
				} // if we're past this loop, that means the middle cards form a straight, now just need to check the head and the tail

                var headMatch = (aSixCardHand[0].Rank - aSixCardHand[1].Rank == 1 && aSixCardHand[0].Suit == aSixCardHand[1].Suit); //match both rank and suit on the first and second cards;
                var tailMatch = (aSixCardHand[aSixCardHand.Length - 2].Rank - aSixCardHand[aSixCardHand.Length - 1].Rank == 1 && aSixCardHand[aSixCardHand.Length - 2].Suit == aSixCardHand[aSixCardHand.Length - 1].Suit); //match both rank and suit on the last and second to last cards;
                var headTailMatchWhenAceSmall = (aSixCardHand[0].Rank == Ranks.ACE && aSixCardHand[aSixCardHand.Length - 1].Rank == Ranks.TWO && aSixCardHand[0].Suit == aSixCardHand[aSixCardHand.Length - 1].Suit);

                return (headMatch && tailMatch) || headTailMatchWhenAceSmall;
			}
			return false;
		}

	}
}
