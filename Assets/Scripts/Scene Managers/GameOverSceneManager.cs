using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
{
    //Cache
    GameObject GameOverCanvas;

    //Ads purposes
    public static int GameOverCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();

        //Leaderboard score updating
        GPGSManager.AddScoreToLeaderboard(gameOverManager.FinalScore);

        //UI stuff
        GameOverCanvas = GameObject.Find("Game Over Canvas");
        GameOverCanvas.transform.Find("Game Over Score").GetComponent<Text>().text = gameOverManager.FinalScore.ToString();
        GameOverCanvas.transform.Find("Try Again Button").GetComponent<Button>().
            onClick.AddListener(delegate { SceneManager.LoadScene("Main Game Scene"); });
        GameOverCanvas.transform.Find("Home Button").GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene("Main Menu"); });

        //Destroying GameOverManager,It will be created when the game restarts.
        Destroy(gameOverManager.gameObject);

        //Ad stuff
        GameOverCount++;
        if (GameOverCount >= 3)
            FindObjectOfType<AdsManager>().ShowVideoAd();
    }
}
