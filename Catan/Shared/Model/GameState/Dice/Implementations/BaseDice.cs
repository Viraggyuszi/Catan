using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState.Dice.Implementations
{
	public class BaseDice : IDice
	{

		private int _value;
		private readonly int[] values = { 1, 2, 3, 4, 5, 6 };
		public object Value => _value;

		public Type DiceType { get => typeof(int); }

		public object RollDice()
		{
			int rIndex = new Random().Next(values.Length);
			_value = values[rIndex]; 
			return Value;
		}
	}
}
