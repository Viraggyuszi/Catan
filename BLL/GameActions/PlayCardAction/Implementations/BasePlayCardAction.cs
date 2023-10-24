using Catan.Shared.Model.GameState;
using Catan.Shared.Model.GameState.Inventory;
using Catan.Shared.Response;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.PlayCardAction.Implementations
{
	public class BasePlayCardAction : IPlayCardAction
	{
		public GameServiceResponses Execute(Game game, CardType card, string name)
		{
			if (game.ActivePlayer.Name != name)
			{
				return GameServiceResponses.ActorIsNotActivePlayer;
			}
			if (game.ResolveResourceCount || game.RobberNeedsMove)
			{
				return GameServiceResponses.CantPlayCardsAtTheMoment;
			}
			var inventory = game.ActivePlayer.CardInventory;
			if (!inventory.HasCard(card))
			{
				return GameServiceResponses.DontHaveCard;
			}
			inventory.RemoveCard(card);
			switch (card)
			{
				case CardType.Knight:
					game.RobberNeedsMove = true;
					game.ActivePlayer.KnightForce += 1; //TODO legnagyobb lovagi hatalom
					if (game.ActivePlayer.KnightForce > game.BiggestKnightForce.KnightForce)
					{
						game.BiggestKnightForce.Points -= 2;
						game.BiggestKnightForce = game.ActivePlayer;
						game.BiggestKnightForce.Points += 2;
					}
					break;
				case CardType.RoadBuilder:
					game.FreeRoadBuilding = 2;
					break;
				case CardType.ExtraPoint:
					game.ActivePlayer.Points += 1;
					break;
				default:
					break;
			}
			return GameServiceResponses.Success;
		}
	}
}
