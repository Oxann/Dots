using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLineRenderer : MonoBehaviour
{
    //Cache
    LineRenderer lineRenderer;
    PlayerAttack playerAttack;

    //Config
    private const float maxLineWidth = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.widthMultiplier = maxLineWidth;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.widthMultiplier = Mathf.Lerp(0.0f,maxLineWidth,(float)playerAttack.Fuel / (float)playerAttack.MaxFuel);
        lineRenderer.SetPosition(1, transform.position);
    }
}
