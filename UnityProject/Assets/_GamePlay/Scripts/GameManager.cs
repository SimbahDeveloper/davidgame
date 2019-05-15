using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;


class Room{
    public List <PlayerModel> player = new List<PlayerModel>();
}

class Rooms
{
    public List<Room> rooms = new List<Room>();
}

public class GameManager : MonoBehaviour
{
    public int muchPlayerCanPlayTogetherInServer = 2;
    public static string MYUID = "nulls";
    PlayerModel playerModel;

    public PlayerModel GetPlayer()
    {
        return playerModel;
    }

    public static GameManager _instance = null;
    GameObject panelConect;
    IEnumerator GetData()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        yield return new WaitForEndOfFrame();
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("game/users").Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                GameObject.FindWithTag("DebugText").GetComponent<Text>().text += " ERRROR DB ";
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                playerModel.name = (string)snapshot.Child("name").Value;
                playerModel.uid = (string)snapshot.Child("uid").Value;
                // Do something with snapshot...
            }
        });
    }
    private void Awake()
    {
        playerModel = new PlayerModel();
        StartCoroutine(GetData());

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
    public void CheckRooms()
    {
        PesanTO("Check Room");
        GameObject.FindWithTag("DebugText").GetComponent<Text>().text += " CHECK ROOM ";
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        GameObject.FindWithTag("DebugText").GetComponent<Text>().text += " PATH ROOM ";
        FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                GameObject.FindWithTag("DebugText").GetComponent<Text>().text += " ERRROR DB ";
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {

                    //GameObject.FindWithTag("DebugText").GetComponent<Text>().text += " " + snapshot.Value;
                    foreach (var item in snapshot.Children)
                    {
                        GameObject.FindWithTag("DebugText").GetComponent<Text>().text += " " + item.Key;
                    }

                    //if (snapshot.ChildrenCount == 0)
                    //{
                    //    CreateRoom(0);
                    //}
                    //var _js = snapshot.GetRawJsonValue();
                    //var gg = JsonUtility.FromJson<Rooms>(_js);
                    //GameObject.FindWithTag("DebugText").GetComponent<Text>().text += " JAJAJ "+gg.rooms.Count;
                }
                else
                {
                    CreateRoom();
                }


            }
        });
    }

    bool RoomMaster = false;
    public string myRoom = "none";
    public void CreateRoom()
    {
        if (playerModel.uid == null) { CancelFindRoom();return; }
        //var date = DateTime.Now.TimeOfDay;
        int ff = UnityEngine.Random.Range(0, 999);
        myRoom = playerModel.uid + ff;
        
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(myRoom).Child(playerModel.uid);
        reference.Child("uid").SetValueAsync(playerModel.uid);
        reference.Child("name").SetValueAsync(playerModel.name);
        reference.Child("roomMaster").SetValueAsync(true);
        RoomMaster = true;

    }
    public void JointRoom()
    {
        PesanTO("create room");
    }
    public void PesanTO(string huh)
    {
        var fu = panelConect.GetComponentsInChildren<Transform>();
        fu[4].GetComponent<Text>().text = huh;
    }

    public void StartGame(GameObject panelConnection)
    {
        panelConect = panelConnection;
        CheckRooms();

    }
    public void CancelFindRoom()
    {
        if (RoomMaster)
        {
            RoomMaster = false;
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(myRoom).Child(playerModel.uid);
            reference.RemoveValueAsync();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("--------dassssssssssss--------");
        //var _room = new Room();
        //var _dplayer  = new List<PlayerModel>();

        //var _player = new PlayerModel();
        //_player.name = "dasd";
        //_player.uid = "dadsaasd";

        //_dplayer.Add(_player);

        //_room.player.Add(_player);
        //var d = JsonUtility.ToJson(_dplayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
