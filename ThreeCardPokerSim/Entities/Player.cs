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
		public long AnteMatchBet { get; set; }
		public long AnteBet { get; set; }
		public long PairPlusBet { get; set; }
		public long SixCardBonusBet { get; set; }
		public long AnteBetPayout { get; set; }
		public long PairPlusBetPayout { get; set; }
		public long SixCardBonusBetPayout { get; set; }
		public Card[] Hand { get; set; }
		public bool IsDealer { get; set; }
		public bool beatDealer { get; set; }
	}
}
