using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState.Dice
{
	public interface IDice
	{
		public DiceValue Value { get; }
		public DiceValue RollDice();
	}
}
