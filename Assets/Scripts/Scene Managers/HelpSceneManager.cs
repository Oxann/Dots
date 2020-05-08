using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpSceneManager : MonoBehaviour
{
    //Cache
    LineRenderer lineRenderer;
    Transform CenterNode;
    Transform EndNode;

    private void Start()
    {
        GameObject.Find("Back Button").GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene("Main Menu"); });
        CenterNode = GameObject.Find("Center Node").transform;
        EndNode = GameObject.Find("End Node").transform;
        lineRenderer = EndNode.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, CenterNode.position);
        lineRenderer.SetPosition(1, EndNode.position);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene("Help Scene");
    }
}
