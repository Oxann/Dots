using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoostSpawner : MonoBehaviour , IEnemyFeature
{
    //Cache
    static GameObject HealthBoost = null;
    Transform HealthBoostSpawn;

    void Start()
    {
        if(!HealthBoost) HealthBoost = Resources.Load<GameObject>("Prefabs/Health Boost");
        HealthBoostSpawn = transform.GetChild(0).transform;
    }

    public void Activate()
    {
        GameObject NewHealthBoost = Instantiate(HealthBoost, transform.position, Quaternion.identity);
        NewHealthBoost.transform.localScale = HealthBoostSpawn.transform.lossyScale;
        NewHealthBoost.GetComponent<HealthBoost>().Health = (int)(GetComponent<EnemyDotManager>().SpawnHealth / 3.0f);
    }
}
