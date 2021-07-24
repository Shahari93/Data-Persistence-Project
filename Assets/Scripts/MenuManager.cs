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
    [SerializeField] Text highScoreText;
    public int highScore;
    public string playerName;
    public string highScoreName;

    private void Awake()
    {
        if (menuManager != null)
        {
            Destroy(gameObject);
            return;
        }
        menuManager = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }


    public void LoadNextScene(int sceneIndex)
    {
        playerName = playerNameIF.text;
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        SaveData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }


    private void OnApplicationQuit()
    {
        SaveData();
    }

    [Serializable]
    public class SavePlayerData
    {
        public int highScore;
        public string highScorePlayer;
    }

    public void SaveData()
    {
        SavePlayerData savePlayerData = new SavePlayerData();
        savePlayerData.highScore = highScore;
        savePlayerData.highScorePlayer = highScoreName;

        string json = JsonUtility.ToJson(savePlayerData);
        File.WriteAllText(Application.persistentDataPath + "/saveData.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavePlayerData savePlayerData = JsonUtility.FromJson<SavePlayerData>(json);
            highScore = savePlayerData.highScore;
            highScoreName = savePlayerData.highScorePlayer;
        }
    }
}
