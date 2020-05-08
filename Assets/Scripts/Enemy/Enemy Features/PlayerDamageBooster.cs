using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageBooster : MonoBehaviour, IEnemyFeature
{
    //Cache
    static PlayerAttack playerAttack = null;

    // Start is called before the first frame update
    void Start()
    {
        if(!playerAttack) playerAttack = FindObjectOfType<PlayerAttack>();
    }

    public void Activate()
    {
        playerAttack.Damage += 0.075f;
    }
}
