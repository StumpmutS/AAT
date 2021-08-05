using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private int battleSceneBuildIndex;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ToBattleScene()
    {
        SceneManager.LoadScene(battleSceneBuildIndex);
    }
}
