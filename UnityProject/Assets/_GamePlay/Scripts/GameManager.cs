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
    bool OnRoom = false;

    public PlayerModel GetPlayer()
    {
        return playerModel;
    }

    public static GameManager _instance = null;
    GameObject panelConect;
    IEnumerator GetData()
    {
#if PLATFORM_ANDROID
        #region getUser
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
            }
        });
        #endregion getUser
#endif
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
        #region android chekroom
#if PLATFORM_ANDROID
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
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
                    List<String> RoomCanJoint = new List<String>();
                    int max = muchPlayerCanPlayTogetherInServer;
                    foreach (var item in snapshot.Children)
                    {
                        if (item.ChildrenCount < max)
                        {
                            GameObject.FindWithTag("DebugText").GetComponent<Text>().text += " " + item.Key;
                            RoomCanJoint.Add(item.Key);
                        }

                    }

                    if(RoomCanJoint.Count > 0)
                    {
                        GameObject.FindWithTag("DebugText").GetComponent<Text>().text += "Joint "+ RoomCanJoint[0];
                        JointRoom(RoomCanJoint[0]);
                    }
                    else
                    {
                        GameObject.FindWithTag("DebugText").GetComponent<Text>().text += "Create";
                        CreateRoom();
                    }
                }
                else
                {
                    CreateRoom();
                }


            }
        });
#endif
        #endregion android chekroom
    }

    bool RoomMaster = false;
    public string myRoom = "none";
    public void CreateRoom()
    {
#if UNITY_ANDROID
        #region createRoom
        OnRoom = true;
        if (playerModel.uid == null) { CancelFindRoom();return; }
        //var date = DateTime.Now.TimeOfDay;
        int ff = UnityEngine.Random.Range(0, 999);
        myRoom = playerModel.uid + ff;
        
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(myRoom).Child(playerModel.uid);
        reference.Child("uid").SetValueAsync(playerModel.uid);
        reference.Child("name").SetValueAsync(playerModel.name);
        reference.Child("roomMaster").SetValueAsync(true);
        RoomMaster = true;
        #endregion
#endif

    }
    public void JointRoom(string ro)
    {
#if PLATFORM_ANDROID
        #region joint
        OnRoom = true;
        myRoom = ro;
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(ro).Child(playerModel.uid);
        reference.Child("uid").SetValueAsync(playerModel.uid);
        reference.Child("name").SetValueAsync(playerModel.name);
        reference.Child("roomMaster").SetValueAsync(false);
        #endregion joint
#endif
    }
    public void PesanTO(string huh)
    {
      //  var fu = panelConect.GetComponentsInChildren<Transform>();
    //    fu[4].GetComponent<Text>().text = huh;
    }

    public void StartGame(GameObject panelConnection)
    {
        panelConect = panelConnection;
        CheckRooms();

    }
    public void CancelFindRoom()
    {
#if PLATFORM_ANDROID
        #region room
        OnRoom = false;
        if (RoomMaster)
        {
            RoomMaster = false;
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(myRoom).Child(playerModel.uid);
            reference.RemoveValueAsync();
        }
        else
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(myRoom).Child(playerModel.uid);
            reference.RemoveValueAsync();
        }
        #endregion room
#endif
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
#if PLATFORM_ANDROID
        if (OnRoom)
        {
            #region creatroom
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(myRoom).GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {

                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.ChildrenCount == muchPlayerCanPlayTogetherInServer)
                    {
                        PlayGame();
                    }
                }
            });
            #endregion creatroom
        }
#endif
    }

    void PlayGame()
    {
        GameObject.FindWithTag("DebugText").GetComponent<Text>().text += "PLAY GAME";
    }
}
