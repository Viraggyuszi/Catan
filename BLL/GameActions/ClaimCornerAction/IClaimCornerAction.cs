using Catan.Shared.Model;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.ClaimCornerAction
{
	public interface IClaimCornerAction
	{
		public GameServiceResponses Execute(Game game, int cornerId, string name);
	}
}
