using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeCardPokerSim.Entities
{
	public static class GameRule
	{
		public static bool IsPlay(Player player, bool isDealer)
		{
			var play = false;
			var playHand = player.Hand;

			playHand = playHand.OrderByDescending(item => item.Rank).ToArray();
			

			return play;
		}

		public static bool IsHighCard(Card[] hand)
		{
			if (hand[0].Rank >= Ranks.QUEEN)
			{
				return true;
			}
			return false;
		}

		public static bool IsPair(Card[] hand)
		{
			if (hand[0].Rank == hand[1].Rank || hand[0].Rank == hand[2].Rank)
				return true;
			if (hand[1].Rank == hand[2].Rank)
				return true;
			return false;
		}


	}
}
