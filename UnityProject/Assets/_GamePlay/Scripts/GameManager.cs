using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    PlayerModel playerModel;

    public NetworkCon network;
    public static GameManager _instance = null;
    GameObject panelConect;
    private void Awake()
    {
        playerModel = new PlayerModel();
        playerModel.name = "sancaya";
        playerModel.power = 0.01f;

        if (_instance != null)
        {
            Destroy(_instance);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void CreateRoom()
    {
        PesanTO("create room");
        network.CreateRoom(playerModel.name + "_room");
    }

    public void PesanTO(string huh)
    {
        var fu = panelConect.GetComponentsInChildren<Transform>();
        fu[4].GetComponent<Text>().text = huh;
    }

    public void StartGame(GameObject panelConnection)
    {
        panelConect = panelConnection;
        network.CheckRoom();
    }
    public void CancelFindRoom(GameObject panelConnection)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
