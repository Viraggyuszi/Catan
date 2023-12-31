﻿using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.AcceptTradeOfferAction
{
    public interface IAcceptTradeOfferAction
    {
        public GameServiceResponses Execute(Game game, TradeOffer offer, string name);
    }
}
