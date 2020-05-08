using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    //Cache
    AdsManager adsManager = null;
    [SerializeField] GameObject RefuelCanvas = null;
    [SerializeField] GameObject GameCanvas = null;
    GameObject EndNode;
    GameObject CenterNode;
    EnemySpawner enemySpawner;

    bool isFirstTry = true;
    public int FinalScore { get; private set; } = 0;

    private void Start()
    {
        //Will be destroyed in Game Over Scene
        DontDestroyOnLoad(gameObject);

        adsManager = FindObjectOfType<AdsManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        EndNode = GameObject.Find("End Node");
        CenterNode = GameObject.Find("Center Node");
        RefuelCanvas.transform.Find("Yes Button").GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(StartSecondTry()); });
        RefuelCanvas.transform.Find("No Button").GetComponent<Button>().onClick.AddListener(delegate { LoadGameOverScene(); });
    }

    public IEnumerator GameOver()
    {
        GameCanvas.SetActive(false);
        if(isFirstTry && adsManager.IsSecondChanceAdReady())
        {
            enemySpawner.AliveEnemyCount = 0;
            enemySpawner.enabled = false;
            EnemyDotManager[] enemies = FindObjectsOfType<EnemyDotManager>();
            foreach (EnemyDotManager enemy in enemies)
            {
                enemy.enabled = false;
            }

            EndNode.SetActive(false);
            CenterNode.GetComponent<Animator>().enabled = true;

            yield return new WaitForSeconds(1.8f);
            RefuelCanvas.SetActive(true);
        }
        else
            LoadGameOverScene();
    }

    private IEnumerator StartSecondTry()
    {
        adsManager.ShowSecondChanceAd();
        yield return new WaitUntil(adsManager.IsAddClosed);

        isFirstTry = false;
        PlayerHealth playerHealth = CenterNode.GetComponent<PlayerHealth>();
        playerHealth.Health = playerHealth.MaxHealth;
        Animator playerAnimator = playerHealth.GetComponent<Animator>();
        playerAnimator.SetBool("IsSecondTryActive", true);
        RefuelCanvas.SetActive(false);
        //Waiting for player animation
        yield return new WaitForSeconds(2.0f);

        EndNode.transform.position = Vector3.zero;
        EndNode.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
        EndNode.SetActive(true);
        GameCanvas.SetActive(true);
        //Little delay before respawning enemies
        yield return new WaitForSeconds(0.2f);

        playerAnimator.enabled = false;
        enemySpawner.enabled = true;
    }

    public void LoadGameOverScene()
    {
        FinalScore = EndNode.GetComponent<PlayerAttack>().Score;
        SceneManager.LoadScene("Game Over Scene");
    }
}
