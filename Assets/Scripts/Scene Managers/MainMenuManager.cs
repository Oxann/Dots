using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    //Cache
    GameObject[] CircularDots;
    [SerializeField] Button ScoreboardButton;

    //Scene Conditions
    bool isGameInitiated = false;

    //CircularDots Config
    private float Speed = 25.0f;
    public float ToCenterSpeed { get; private set; } = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        CircularDots = GameObject.FindGameObjectsWithTag("Circular Dot");
        GameObject.Find("Help Button").GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene("Help Scene"); });
        GameObject.Find("Play Button").GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(InitiateGame()); });
        ScoreboardButton.onClick.AddListener(GPGSManager.ShowLeaderboard);
    }

    private void Update()
    {
        if(isGameInitiated)
        {
            Speed += 300.0f * Time.deltaTime;
        }
        foreach(GameObject cd in CircularDots)
        {
            if(isGameInitiated)
            {
                cd.transform.position = Vector3.MoveTowards(cd.transform.position, Vector3.zero, ToCenterSpeed * Time.deltaTime);
            }
            cd.transform.RotateAround(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), Speed * Time.deltaTime);
        }
    }

    IEnumerator InitiateGame()
    {
        GameObject.Find("UI").SetActive(false);
        isGameInitiated = true;
        yield return new WaitUntil(IsGameStarting);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Main Game Scene");
    }

    private bool IsGameStarting()
    {
        return CircularDots[0].transform.position == Vector3.zero;
    }
}
