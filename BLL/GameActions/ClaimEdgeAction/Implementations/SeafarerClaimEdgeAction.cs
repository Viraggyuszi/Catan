using Catan.Shared.Model;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.ClaimEdgeAction.Implementations
{
	public class SeafarerClaimEdgeAction : IClaimEdgeAction
	{
		public GameServiceResponses Execute(Game game, int edgeId, string name)
		{
			throw new NotImplementedException();  //TODO tengeri utazó út/hajó foglalása
		}
	}
}
