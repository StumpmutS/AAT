using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SessionJoiner : MonoBehaviour
{
    [SerializeField] private StumpNetworkRunner networkRunner;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(HostSession);
        joinButton.onClick.AddListener(JoinSession);
    }

    private void HostSession()
    {
        networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Host,
            SessionName = input.text,
            Scene = SceneManager.GetActiveScene().buildIndex + 1,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    private void JoinSession()
    {
        networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Client,
            SessionName = input.text,
            Scene = SceneManager.GetActiveScene().buildIndex + 1,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
}
