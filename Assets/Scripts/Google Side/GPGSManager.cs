using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;

public class GPGSManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var GPGSmanagers = FindObjectsOfType<GPGSManager>();
        if (GPGSmanagers.Length > 1)
            Destroy(gameObject);
        else
        {
            StartCoroutine(TryAuthenticate());
            DontDestroyOnLoad(gameObject);
        }
    }

    private IEnumerator TryAuthenticate()
    {
        while (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool value) => {});
            yield return new WaitForSeconds(15.0f);
        }
    }

    static public void ShowLeaderboard()
    {
        if(PlayGamesPlatform.Instance.IsAuthenticated())
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_high_score);
    }
    
    static public void AddScoreToLeaderboard(int Score)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            Social.ReportScore(Score, GPGSIds.leaderboard_high_score, null);
    }
}
