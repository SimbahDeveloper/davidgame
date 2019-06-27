using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    public String[] GameType;

    public int MyScore;

    [SerializeField]
    public List<String> UserPlay;
    [SerializeField]
    public static string MYUID ;
    public static string namePlay;
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
if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                playerModel.name = (string)snapshot.Child("name").Value;
                playerModel.uid = (string)snapshot.Child("uid").Value;
                MYUID = playerModel.uid;
                namePlay = playerModel.name;
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
        MYUID = "nulls";
        UserPlay = new List<string>();
        myRoom = "none";
        MyScore = 0;
    }

    void ResetScore()
    {
        MyScore = 0;
    }
    void AddScore(int much)
    {
        MyScore += much;
    }
    bool Hayok = false;
    bool Manuke = true;
    void MakeGame()
    {
        OnRoom = false;
        push = true;
        Hayok = true;
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(myRoom).Child("Play");
        if (CanMakeGame)
        {
            CanMakeGame = false;
            for (int i = 0; i < GameType.Length; i++)
            {
                string temp = GameType[i];
                int randomIndex = UnityEngine.Random.Range(i, GameType.Length);
                GameType[i] = GameType[randomIndex];
                GameType[randomIndex] = temp;
            }

            for (int i = 0; i < UserPlay.Count; i++)
            {
                string temp = UserPlay[i];
                int randomIndex = UnityEngine.Random.Range(i, UserPlay.Count);
                UserPlay[i] = UserPlay[randomIndex];
                UserPlay[randomIndex] = temp;
            }
            for (int i = 0; i < UserPlay.Count; i++)
            {
                reference.Child("Player").Child("Player" + (i + 1)).SetValueAsync(UserPlay[i]);
            }
            for (int i = 0; i < GameType.Length; i++)
            {
                if (i == GameType.Length - 1)
                {
                    listGame += GameType[i];
                }
                else
                {
                    listGame += GameType[i]+listRaw;
                }
                reference.Child("Game" + i + 1).SetValueAsync(GameType[i]);
            }
            reference.Child("STATUS").SetValueAsync("READY");
        }
    }

    public static char listRaw = '#';
    string listGame = "";

    public void CheckRooms()
    {
        PesanTO("Check Room");
        #region android chekroom
#if PLATFORM_ANDROID
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").GetValueAsync().ContinueWith(task => {
if (task.IsCompleted)
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
                            RoomCanJoint.Add(item.Key);
                        }

                    }

                    if(RoomCanJoint.Count > 0)
                    {
                        JointRoom(RoomCanJoint[0]);
                    }
                    else
                    {
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
    public string myRoom;
    public void CreateRoom()
    {
#if UNITY_ANDROID
        #region createRoom
        addedUser = true;
        if (playerModel.uid == null) { CancelFindRoom();return; }
        //var date = DateTime.Now.TimeOfDay;
        int ff = UnityEngine.Random.Range(0, 999);
        myRoom = playerModel.uid + ff;
        
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(myRoom).Child(playerModel.uid);
        reference.Child("uid").SetValueAsync(playerModel.uid);
        reference.Child("name").SetValueAsync(playerModel.name);
        reference.Child("roomMaster").SetValueAsync(true);
        RoomMaster = true;
        OnRoom = true;
        push = true;
        #endregion
#endif

    }
    public void JointRoom(string ro)
    {
#if PLATFORM_ANDROID
        #region joint
        myRoom = ro;
        addedUser = true;
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(ro).Child(playerModel.uid);
        reference.Child("uid").SetValueAsync(playerModel.uid);
        reference.Child("name").SetValueAsync(playerModel.name);
        reference.Child("roomMaster").SetValueAsync(false);
        OnRoom = true;
        push = true;
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
        #region roomddd
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
        #endregion roomddd
#endif
    }

    // Start is called before the first frame update
    void Start()
    {

        //var _room = new Room();
        //var _dplayer  = new List<PlayerModel>();

        //var _player = new PlayerModel();
        //_player.name = "dasd";
        //_player.uid = "dadsaasd";

        //_dplayer.Add(_player);

        //_room.player.Add(_player);
        //var d = JsonUtility.ToJson(_dplayer);
    }
    bool CanMakeGame = false;
    bool addedUser = true;
    bool push = true;
    void Update()
    {
#if PLATFORM_ANDROID
            if (OnRoom)
            {
            #region creatroom
            OnRoom = false;
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(myRoom).GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        OnRoom = true;
                    }
                    else if (task.IsCompleted)
                    {
                        CanMakeGame = false;
                        DataSnapshot snapshot = task.Result;
                        if (snapshot.ChildrenCount == muchPlayerCanPlayTogetherInServer)
                        {
                            List<string> _UserPlay = new List<string>();

                            foreach (var item in snapshot.Children)
                            {
                                _UserPlay.Add(item.Key);
                                if (item.Key == playerModel.uid)
                                {
                                    CanMakeGame = true;
                                }
                            }
                            if (addedUser)
                            {
                                addedUser = false;
                                for (int i = 0; i < muchPlayerCanPlayTogetherInServer; i++)
                                {
                                    UserPlay.Add(_UserPlay[i]);
                                }
                            }
                            MakeGame();
                        }
                        OnRoom = true;
                    }
                });
                #endregion creatroom
            }

            if (Hayok)
            {
            Hayok = false;
            FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(myRoom).Child("Play").GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        DataSnapshot sn = task.Result;
                        if (sn.Child("STATUS").Value.ToString() == "READY")
                        {
                            if (push)
                            {
                                Hayok = false;
                                PlayGame();
                            }
                        }
                        else
                        {
                            Hayok = true;
                        }
                    }
                });
            }
#endif
    }

    void PlayGame()
    {
        AudioHelper.init.StopMusic();
        REMBULAN.Room = myRoom;
        SceneManager.LoadScene("DesaTambangBakiak",LoadSceneMode.Single);
    }
}
