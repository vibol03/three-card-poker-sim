namespace ThreeCardPokerSim.Entities
{
	public class Player
	{
		public Player()
		{
			Hand = new Card[3];
		}

		public Player(bool isDealer)
		{
			Hand = new Card[3];
			IsDealer = isDealer;
		}

		public long TotalMoney { get; set; }
		public long AnteBet { get; set; }
		public long PairBonusBet { get; set; }
		public long SixCardBonusBet { get; set; }
		public Card[] Hand { get; set; }
		public bool IsDealer { get; set; }
		public bool beatDealer { get; set; }
	}
}
