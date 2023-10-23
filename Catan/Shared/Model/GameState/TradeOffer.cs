﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameState
{
    public class TradeOffer
    {
        public Player Owner { get; set; }
        public Inventory OwnerOffer { get; set; }
        public Inventory TargetOffer { get; set; }
        public bool ToPlayers { get; set; } = true;
        public TradeOffer(Player creator, Inventory ownerOffer, Inventory targetOffer)
        {
            Owner = creator;
            OwnerOffer = ownerOffer;
            TargetOffer = targetOffer;
        }

    }
}
