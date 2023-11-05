using Catan.Shared.Model.GameState.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState
{
    public class TradeOffer
    {
        public int Id { get; set; } 
        public Player Owner { get; set; }
        public AbstractInventory OwnerOffer { get; set; }
        public AbstractInventory TargetOffer { get; set; }
        public bool ToPlayers { get; set; } = true;
        public TradeOffer(Player owner, AbstractInventory ownerOffer, AbstractInventory targetOffer)
        {
            Owner = owner;
            OwnerOffer = ownerOffer;
            TargetOffer = targetOffer;
        }
    }
}
