using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRandomFeatureActivator : MonoBehaviour,IEnemyFeature
{
    //Cache
    static PlayerAttack playerAttack = null;
    
    //Type Rates
    //                  ScoreMultiplier_2x_Rate          =  1.00f  // %50
    private const float ScoreMultiplier_5x_Rate          =  0.15f; // %10
    private const float ScoreMultiplier_10x_Rate         =  0.05f; // %5
    private const float PlayerFire_Rate                  =  0.40f; // %17.5
    private const float UnlimitedFuel                    =  0.65f; // %17.5

    private void Start()
    {
        if(!playerAttack) playerAttack = FindObjectOfType<PlayerAttack>();
    }

    public void Activate()
    {
        float TypeRNG = Random.value;
        if (TypeRNG <= ScoreMultiplier_10x_Rate)
            playerAttack.ActivateRandomFeature(PlayerAttack.RandomFeature.ScoreMultiplier_10x);
        else if (TypeRNG <= ScoreMultiplier_5x_Rate)
            playerAttack.ActivateRandomFeature(PlayerAttack.RandomFeature.ScoreMultiplier_5x);
        else if (TypeRNG <= PlayerFire_Rate)
            playerAttack.ActivateRandomFeature(PlayerAttack.RandomFeature.Fire);
        else if (TypeRNG <= UnlimitedFuel)
            playerAttack.ActivateRandomFeature(PlayerAttack.RandomFeature.UnlimitedFuel);
        else
            playerAttack.ActivateRandomFeature(PlayerAttack.RandomFeature.ScoreMultiplier_2x);
    }
}



