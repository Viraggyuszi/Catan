using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState.Dice.Implementations
{
	public class BaseDice : IDice<int>
	{
		private int _value;
		public int Value => _value;

		private int[] values = { 1, 2, 3, 4, 5, 6 };
		public int RollDice()
		{
			int rIndex=new Random().Next(values.Length);
			_value = values[rIndex];
			return values[rIndex];
		}
	}
}
