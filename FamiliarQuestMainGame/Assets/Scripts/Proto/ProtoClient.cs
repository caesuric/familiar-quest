using Grpc.Core;
using System;
using System.Collections.Generic;

public class ProtoClient {

    private static readonly Channel channel = null;
    private static Login.LoginClient loginClient = null;
    private static MatchMaker.MatchMakerClient matchMakerClient = null;
    private static CharacterSync.CharacterSyncClient characterSyncClient = null;
    private static string userToken = null;
    public static string gameId = null;
    public static List<NetworkCharacterSyncer> syncedCharacters = new List<NetworkCharacterSyncer>();
    public static NetworkCharacterSyncer localPlayer = null;
    public static List<CharacterWorldState> newSyncedCharacters = new List<CharacterWorldState>();
    public static bool newSyncedCharactersLocked = false;
    public static bool isHost = false;

    static ProtoClient() {
        channel = new Channel("localhost:50051", ChannelCredentials.Insecure);
        loginClient = new Login.LoginClient(channel);
        matchMakerClient = new MatchMaker.MatchMakerClient(channel);
        characterSyncClient = new CharacterSync.CharacterSyncClient(channel);
    }

    public static void Login() {
        var reply = loginClient.GetUserToken(new UserTokenRequest { Name = "placeholder" });
        userToken = reply.Token;
    }

    public static GameList GetActiveGames() {
        return matchMakerClient.GetActiveGames(new GetActiveGamesRequest());
    }

    public static GameDescription CreateGame(string name) {
        var reply = matchMakerClient.CreateGame(new GameCreationRequest { GameName = name, Token = userToken });
        gameId = reply.GameDescription.GameId;
        isHost = true;
        return reply.GameDescription;
    }

    public static async void UpdateCharacterWorldStates() {
        var data = new CharacterWorldStates { GameId = gameId };
        TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
        double timestamp = t.TotalSeconds;
        data.UpdateTime = timestamp;
        foreach (var entry in syncedCharacters) data.CharacterData.Add(entry.worldState);
        var result = await characterSyncClient.UpdateCharacterWorldStatesAsync(data);
    }

    public static async void UpdatePersonalWorldState(CharacterWorldState worldState) {
        worldState.IsPlayer = true;
        worldState.IsVisible = true;
        var request = new CharacterWorldStateUpdate() { GameId = gameId, WorldState = worldState };
        var result = await characterSyncClient.UpdatePersonalWorldStateAsync(request);
    }

    public static async void GetCharacterWorldStates() {
        var states = await characterSyncClient.GetCharacterWorldStatesAsync(new CharacterWorldStateRequest() { GameId = gameId });
        foreach (var data in states.CharacterData) FindOrUpdateWorldState(data, NetworkCharacterSyncer.currentUpdateTimestamp, states.UpdateTime);
        NetworkCharacterSyncer.currentUpdateTimestamp = states.UpdateTime;
        RemoveOldCharacters(states);
    }

    public static async void GetPlayerUpdates() {
        var states = await characterSyncClient.GetPlayerWorldStatesAsync(new CharacterWorldStateRequest() { GameId = gameId });
        foreach (var data in states.CharacterData) FindOrUpdateWorldState(data, NetworkCharacterSyncer.currentUpdateTimestamp, states.UpdateTime);
        NetworkCharacterSyncer.currentUpdateTimestamp = states.UpdateTime;
        RemoveOldPlayers(states);
    }

    private static void FindOrUpdateWorldState(CharacterWorldState data, double oldTime, double newTime) {
        foreach (var syncer in syncedCharacters) {
            if (syncer.worldState.Id == data.Id) {
                syncer.worldState = data;
                return;
            }
        }
        AddNewCharacter(data);
    }

    private static void AddNewCharacter(CharacterWorldState data) {
        if (!newSyncedCharactersLocked) newSyncedCharacters.Add(data);
    }

    private static void RemoveOldCharacters(CharacterWorldStates states) {
        // STUB: TODO
    }

    private static void RemoveOldPlayers(CharacterWorldStates states) {
        // STUB: TODO
    }
}
