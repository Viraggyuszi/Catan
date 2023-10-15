using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState.Dice
{
	public interface IDice
	{
		public Type DiceType { get; }
		public object Value { get; }
		public object RollDice();
	}
}
