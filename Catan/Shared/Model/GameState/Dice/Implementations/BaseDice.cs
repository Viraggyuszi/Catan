using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState.Dice.Implementations
{
	public class BaseDice : IDice
	{
		private DiceValue _value = DiceValue.NULLDICE;
		public DiceValue Value => _value;

		private int[] values = { 1, 2, 3, 4, 5, 6 };
		public DiceValue RollDice()
		{
			int rIndex=new Random().Next(values.Length);
			_value = DiceValue.DiceValueFromInt(values[rIndex]);
			return _value;
		}
	}
}
