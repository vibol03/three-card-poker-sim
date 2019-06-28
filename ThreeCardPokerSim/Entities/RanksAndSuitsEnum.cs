namespace ThreeCardPokerSim.Entities
{
	public enum Suits { SPADE, CLUB, DIAMOND, HEART };
	public enum Ranks { TWO = 2, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE };
	public enum BetTypes { ANTE, PAIR_PLUS, SIX_CARD_BONUS }
	public enum PlayTypes { HIGH_CARD = 1, ONE_PAIR, FLUSH, STRAIGHT, THREE_OF_A_KIND, STRAIGHT_FLUSH, MINI_ROYAL_FLUSH, FULL_HOUSE, ROYAL_FLUSH }
	public enum AntePayouts { STRAIGHT = 1, THREE_OF_A_KIND = 4, STRAIGHT_FLUSH }
	public enum PairPlusPayouts { ONE_PAIR = 1, FLUSH = 4, STRAIGHT, THREE_OF_A_KIND = 30, STRAIGHT_FLUSH = 40 }
	public enum SixCardBonusPayouts { THREE_OF_A_KIND = 5, STRAIGHT = 10, FLUSH = 20, FULL_HOUSE = 25, FOUR_OF_A_KIND = 50, STRAIGHT_FLUSH = 200, ROYAL_FLUSH = 1000 }

}
