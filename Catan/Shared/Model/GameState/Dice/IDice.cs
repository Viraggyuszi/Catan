using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState.Dice
{
	public interface IDice<T>
	{
		public T Value { get; }
		public T RollDice();
	}
}
