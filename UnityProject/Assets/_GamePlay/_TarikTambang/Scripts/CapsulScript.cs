using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsulScript : MonoBehaviour
{


    public bool IsEnemy = false;

    PlayerModel Model;
    bool IsModelReady = false;
    TTManager tT;

    public void SetModel(PlayerModel model, TTManager tT)
    {
        this.tT = tT;
        this.Model = model;
        IsModelReady = true;
   }

    public PlayerModel GetModel()
    {
        return Model;
    }

    public void UpdateModel(PlayerModel model)
    {
        this.Model = model;
    }
}
