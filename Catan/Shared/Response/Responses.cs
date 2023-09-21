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
        Success,
        InvalidGame,
        InvalidMember,
        InvalidCorner,
        InvalidEdge,
        InvalidField,
        GameCanStart,
        GameCantStart,
        InitialRound,
        NotInitialRound,
        CornerAlreadyTaken,
        EdgeAlreadyTaken,
        InitialRoadNotPlacedCorrectly,
        CantPlaceCornerAtTheMoment,
        CantPlaceEdgeAtTheMoment,
        NotEnoughResourcesForRoad,
        NotEnoughResourcesForVillage,
        NotEnoughResourcesForUpgrade,
        CantPlaceCornerWithoutPath,
        SomeHowTheRobberIsMissing,
        GameOver
    }
}
