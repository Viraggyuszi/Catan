using Catan.Shared.Model.GameState;
using Catan.Shared.Model.GameState.Inventory;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.ThrowResourcesAction
{
    public interface IThrowResourcesAction
    {
        public GameServiceResponses Execute(Game game, AbstractInventory thrownResources, string name);
    }
}
