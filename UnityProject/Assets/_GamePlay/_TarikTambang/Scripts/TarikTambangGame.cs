using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using PengembangSebelah;

public class TarikTambangGame : MonoBehaviour
{
    [Header("System Variabel")]
    #region systemvar
    public float powerCal = 0.01f;
    #endregion

    [Header ("Pembatu Variabel")]
    #region variabel
    public GameObject PlayersModel;
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

    public ServerPlayerModel model;
    string enemy_key="tes123";
    string my_key;
    public void SetEnemyKey(string s)
    {
        this.enemy_key = s;
    }

    public void SetModel(ServerPlayerModel model)
    {
        this.model = model;
    }

    private void Awake()
    {
        var uhut = PlayersModel.GetComponentsInChildren<PlayerScript>();
        foreach (var item in uhut)
        {
            if (item.GetModelPlayer() == model)
            {
               _playerScript = item;
               _playerScript.SetMyChar(enemy_key,"fghj");
                EnemyPlayer = _playerScript.myEnemie;
                _enemyScript = EnemyPlayer.GetComponent<PlayerScript>();
#if UNITY_EDITOR
                player = new PlayerModel();
                player.name = "Debug";
                player.power = 1f;
#endif
                _playerScript.myName += player.name;
            }
        }
        _uiScript = gameObject.GetComponent<TarikTambangUI>();
    }

    void InitGame()
    {
        #region Editor

#if UNITY_EDITOR
        
        if (Input.GetKey(KeyCode.Space))
        {
            _playerScript.myPower += player.power;
            UpdatePower(my_key);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _playerScript.myPower -= player.power;
        }
        //#elif UNITY_ANDROID
        var touchs = InputHelper.GetTouches();
#endif
        #endregion

        if (touchs.Count > 0)
        {
            Touch t = touchs[0];
            if (t.phase != TouchPhase.Began) return;
            _playerScript.myPower += player.power;
            Debug.Log("Touch");
        }

        float _wino = calWino(_playerScript.myPower, _enemyScript.myPower);
        _uiScript.UpdateSlider(_wino);
    }

    void UpdatePower(string key)
    {

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
