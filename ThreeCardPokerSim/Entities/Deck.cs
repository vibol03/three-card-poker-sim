using System;
using System.Linq;

namespace ThreeCardPokerSim.Entities
{
	public class Deck
	{
		public Card[] Cards;
		public Deck()
		{
			Cards = NewDeckAndShuffle();
		}

		public Card[] NewDeckAndShuffle()
		{
			var cards = new Card[52];

			var index = 0;
			foreach (Ranks rank in Enum.GetValues(typeof(Ranks)))
			{
				foreach (Suits suit in Enum.GetValues(typeof(Suits)))
				{
					cards[index++] = new Card(rank, suit);
				}
			}

			return ShuffleDeck(cards);
		}

		public static Card[] ShuffleDeck(Card[] aDeck)
		{
			Random rnd = new Random();
			var aShuffledDeck = aDeck.OrderBy(x => rnd.Next()).ToArray();

			return aShuffledDeck;
		}
	}
}
