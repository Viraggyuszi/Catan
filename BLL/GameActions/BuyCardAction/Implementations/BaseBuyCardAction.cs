using Catan.Shared.Model.GameMap;
using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.BuyCardAction.Implementations
{
	public class BaseBuyCardAction : IBuyCardAction
	{
		public GameServiceResponses Execute(Game game, string name)
		{
			if (game.ActivePlayer.Name != name)
			{
				return GameServiceResponses.ActorIsNotActivePlayer;
			}
			if (game.ResolveResourceCount || game.RobberNeedsMove)
			{
				return GameServiceResponses.CantBuyCardsAtTheMoment;
			}
			var inventory = game.ActivePlayer.Inventory;
			if (!inventory.HasEnoughForCard())
			{
				return GameServiceResponses.NotEnoughResourcesForCard;
			}
			inventory.PayForCard();
			if (game.Cards.IsNullOrEmpty())
			{
				return GameServiceResponses.OutOfCards;
			}
			var number = new Random().Next(0, game.Cards.Count);
			var card = game.Cards[number];
			game.Cards.RemoveAt(number);
			game.ActivePlayer.CardInventory.AddCard(card);
			return GameServiceResponses.Success;
		}
	}
}
