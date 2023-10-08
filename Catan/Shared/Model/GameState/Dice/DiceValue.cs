using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState.Dice
{
	public class DiceValue
	{
		public static readonly DiceValue NULLDICE = new DiceValue();
		public static readonly DiceValue D6_1 = new DiceValue();
		public static readonly DiceValue D6_2 = new DiceValue();
		public static readonly DiceValue D6_3 = new DiceValue();
		public static readonly DiceValue D6_4 = new DiceValue();
		public static readonly DiceValue D6_5 = new DiceValue();
		public static readonly DiceValue D6_6 = new DiceValue();
		private DiceValue() { }
		public static DiceValue DiceValueFromInt(int value)
		{
			switch (value)
			{
				case 1: return D6_1;
				case 2: return D6_2;
				case 3: return D6_3;
				case 4: return D6_4;
				case 5: return D6_5;
				case 6: return D6_6;
				default: return NULLDICE;
			}
		}
		public static int IntFromDiceValue(DiceValue value)
		{
			if (value == D6_1) return 1;
			if (value == D6_2) return 2;
			if (value == D6_3) return 3;
			if (value == D6_4) return 4;
			if (value == D6_5) return 5;
			if (value == D6_6) return 6;
			if (value == NULLDICE) return -1;
			return -10000;
		}
	}
}
