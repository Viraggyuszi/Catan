﻿using Catan.Shared.Model.GameState;
using Catan.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GameActions.BuildCityAction.Implementations
{
	public class BaseBuildCityAction : IBuildCityAction
	{
		public GameServiceResponses Execute(Game game, int cornerId, string name)
		{
			if (game.ActivePlayer.Name != name)
			{
				return GameServiceResponses.ActorIsNotActivePlayer;
			}
			if (game.HaveToRollDices)
			{
				return GameServiceResponses.RollDicesFirst;
			}
			if (game.ResolveResourceCount || game.RobberNeedsMove)
			{
				return GameServiceResponses.CantPlaceCornerAtTheMoment;
			}
			var corner = game.GameMap.CornerList.FirstOrDefault(corner => corner.Id == cornerId);
			if (corner is null)
			{
				return GameServiceResponses.InvalidCorner;
			}
			if (corner.Player.Name != name || corner.Level != 1)
			{
				return GameServiceResponses.CantUpgradeVillage;
			}
			var inventory = game.ActivePlayer.Inventory;
			if (!inventory.HasEnoughForCityUpgrade())
			{
				return GameServiceResponses.NotEnoughResourcesForUpgrade;
			}
			inventory.PayForCityUpgrade();
			corner.Level = 2;
			game.ActivePlayer.Points += 1;
			return GameServiceResponses.Success;
		}
	}
}
