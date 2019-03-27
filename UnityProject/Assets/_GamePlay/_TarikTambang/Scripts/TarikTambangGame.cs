using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class TarikTambangGame : MonoBehaviour
{
    [Header("System Variabel")]
    #region systemvar
    public float powerCal = 0.01f;
    #endregion

    [Header ("Pembatu Variabel")]
    #region variabel
    public GameObject MyPlayer;
    bool EndGame = false;

    //private
    PlayerModel player;
    GameObject EnemyPlayer;
    #endregion variabel


    //uiScript sama dengan TarikTambangUI.cs
    #region scripts
    TarikTambangUI _uiScript;
    PlayerScript _playerScript;
    PlayerScript _enemyScript;
    #endregion

    private void Awake()
    {
        _uiScript = gameObject.GetComponent<TarikTambangUI>();
        _playerScript = MyPlayer.GetComponent<PlayerScript>();
        EnemyPlayer = _playerScript.myEnemie;
        _enemyScript = EnemyPlayer.GetComponent<PlayerScript>();
#if UNITY_EDITOR
        player = new PlayerModel();
        player.name = "Debug";
        player.power = 1f;
#endif

        _playerScript.myName += player.name;
    }

    void InitGame()
    {
        #region Editor

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Space))
        {
            _playerScript.myPower += player.power;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _playerScript.myPower -= player.power;
        }
#endif
        #endregion

        float _wino = calWino(_playerScript.myPower, _enemyScript.myPower);
        Debug.Log(_wino);
        _uiScript.UpdateSlider(_wino);
    }

    void Update()
    {
        if (!EndGame)
        {
            InitGame();
        }


        string _stat = _uiScript.GetStatusGame();
        if (_stat == Constants.Win)
        {
            //Menang
            EndGame = true;
            Debug.Log("------------MENANG-----------");
        }
        else if(_stat == Constants.Lose)
        {
            EndGame = true;
            //Kalah

            Debug.Log("------------KALAH-----------");
        }
        else if (_stat == Constants.Draw)
        {
            EndGame = true;
            //Draw
        }
        else
        {
            //Ongame
        }
    }

    #region penghitunganmanja
    float calWino(float x , float y)
    {
        float z = (x - y) * powerCal * Time.deltaTime;

        return z;
    }
    #endregion penghitunganmanja
}
