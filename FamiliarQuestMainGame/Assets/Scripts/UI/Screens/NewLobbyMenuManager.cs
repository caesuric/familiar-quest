﻿using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewLobbyMenuManager : MonoBehaviour {
    GameList gameList = null;
    public GameObject buttonContainer;
    public GameObject gameButtonPrefab;
    public GameObject newGamePanel;
    public GameObject selectGamePanel;
    public GameObject playerPrefab;
    public InputField nameInput;

    void Start() {
        try {
            ProtoClient.Login();
            UpdateGameList();
        }
        catch (Exception e) {
            LoadInitialScene();
        }
    }

    private void UpdateGameList() {
        gameList = ProtoClient.GetActiveGames();
        foreach (Transform child in buttonContainer.transform) Destroy(child);
        foreach (var game in gameList.Games) AddGameButton(game);
    }

    private void AddGameButton(GameDescription game) {
        var go = Instantiate(gameButtonPrefab, buttonContainer.transform);
        var handler = go.GetComponent<LobbyGameButtonHandler>();
        handler.Initialize(game, this);
    }

    public void SelectGame(GameDescription game) {
        ProtoClient.gameId = game.GameId;
        LoadInitialScene();
    }

    private void LoadInitialScene() {
        WorldGenerator.FetchWorlds();
        if (!WorldGenerator.worlds.Contains(CharacterSelectScreen.selectedCharacterName)) WorldGenerator.CreateWorld(CharacterSelectScreen.selectedCharacterName);
        if (LobbyManager.singleton == null) LobbyManager.singleton = new LobbyManager();
        LobbyManager.singleton.worldByteArray = File.ReadAllBytes(Application.persistentDataPath + "/worlds/" + CharacterSelectScreen.selectedCharacterName + ".world");
        var ao = SceneManager.LoadSceneAsync("Starting Area");
        LoadingProgressBar.StartLoad(ao, 0);
        var player = Instantiate(playerPrefab);
        player.GetComponent<PlayerCharacter>().isMe = true;
    }

    public void ClickCreateNewGameButton() {
        newGamePanel.SetActive(true);
        selectGamePanel.SetActive(false);
    }

    public void CreateNewGame() {
        var game = ProtoClient.CreateGame(nameInput.text);
        SelectGame(game);
    }

    public void CancelCreateNewGame() {
        newGamePanel.SetActive(false);
        selectGamePanel.SetActive(true);
    }

    public void CancelStartGame() {
        SceneManager.LoadScene("Character Selection");
    }
}
