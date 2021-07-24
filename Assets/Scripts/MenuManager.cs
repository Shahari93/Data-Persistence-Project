using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManager;
    public InputField playerNameIF;
    public string playerName;


    private void Awake()
    {
        if (menuManager != null)
        {
            Destroy(gameObject);
            return;
        }
        menuManager = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextScene(int sceneIndex)
    {
        if (playerNameIF.text != null)
        {
            playerName = playerNameIF.text;
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        SaveData();
    }

    [Serializable]
    public class SaveHighScoreData
    {
        public int highScore;
    }

    public void SaveData()
    {
        SaveHighScoreData saveHighScore = new SaveHighScoreData();
        saveHighScore.highScore = MainManager.highScorePoints;

        string json = JsonUtility.ToJson(saveHighScore);
        File.WriteAllText(Application.persistentDataPath + "/saveDtat.json", json);
    }
}
