using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Response
{
    public enum InMemoryDatabaseLobbyResponses
    {
        Success,
        AlreadyMember,
        InvalidLobby,
        AlreadyFull,
        NotAMember,
        CreateLobbyFailed,
        RemoveLobbyFailed
    }
    public enum InMemoryDatabaseGameResponses
    {
        Success,
        InvalidGame,
        CreateGameFailed,
        RemoveGameFailed
    }
    public enum GameServiceResponses
    {
        ShipCantBePlacedHere,
        ForbiddenAction,
        Success,
        InvalidGame,
        InvalidMember,
        InvalidCorner,
        InvalidEdge,
        InvalidField,
        ActorIsNotActivePlayer,
        GameCanStart,
        GameCantStart,
        InitialRound,
        NotInitialRound,
        CornerAlreadyTaken,
        EdgeAlreadyTaken,
        InitialRoadNotPlacedCorrectly,
        CantPlaceCornerAtTheMoment,
        CantPlaceEdgeAtTheMoment,
        CantPlaceVillageNextToOtherVillage,
        CantUpgradeVillage,
        NotEnoughResourcesForRoad,
        NotEnoughResourcesForVillage,
        NotEnoughResourcesForUpgrade,
        CantPlaceCornerWithoutPath,
        CantPlaceCornerToSea,
        CantPlaceEdgeToSea,
        SomeHowTheRobberIsMissing,
        GameOver,
        GameInProgress,
		TradeListFull,
        NotEnoughResourcesToCreateTrade,
        NotEnoughResourcesToAcceptTrade,
        BadResourceCountForTradingWithBank,
        NotEnoughResourceThrown,
        InvalidResourcesHaveBeenThrown,
        SuccessWithSevenRoll
	}
}
