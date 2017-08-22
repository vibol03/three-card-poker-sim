namespace ThreeCardPokerSim.Entities
{
	public class Card
	{
		public Card()
		{

		}
		public Card(Ranks rank, Suits suit)
		{
			Rank = rank;
			Suit = suit;
		}
		public Ranks Rank { get; set; }
		public Suits Suit { get; set; }

	}
}
