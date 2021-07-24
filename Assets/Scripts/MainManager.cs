using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text highScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    private static int highScorePoints;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        //if (m_Points > highScorePoints)
        //{
        //    highScorePoints = m_Points;
        //}
        highScoreText.text = "High Score: " + MenuManager.menuManager.playerName + " " + highScorePoints;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (m_Points > highScorePoints)
        {
            highScorePoints = m_Points;
            highScoreText.text = "High Score: " + MenuManager.menuManager.playerName + " " + m_Points;
        }

        m_GameOver = true;
        GameOverText.SetActive(true);
    }


    [Serializable]
    public class SaveHighScoreData
    {
        MenuManager menuManagerSave = new MenuManager();
        int highScore;

        public void SaveData()
        {
            highScore = highScorePoints;
            string json = JsonUtility.ToJson(menuManagerSave);
        }
    }
}
