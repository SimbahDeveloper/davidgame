using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class REMBULAN : MonoBehaviour
{
    public GameObject MainCam;
    public static string Room = "aa";
    public string playerId = "kunir";

    public string G1="", G2="", G3="";
    public string p1 = "", p2 = "", p3 = "", p4 = "";

    public static REMBULAN init;
    public bool kuy = false;
    int a = 0;
    public GameObject[] bangbang;

    // Start is called before the first frame update
    void Start()
    {
        if(init == null)
        {
            init = this;
        }

        MyDebug.init.Log(Room);
        MyDebug.init.Log(p1 + " " + p2 + " " + p3 + " " + p4);
//        MyDebug.init.Log(GameManager._instance.MYUID);
        kuy = true;
#if UNITY_EDITOR
        One();
#endif

#if !UNITY_EDITOR

        FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(Room).Child("Play").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                p1 = snapshot.Child("Player/Player1").Value.ToString();
                p2 = snapshot.Child("Player/Player2").Value.ToString();
                p3 = snapshot.Child("Player/Player3").Value.ToString();
                p4 = snapshot.Child("Player/Player4").Value.ToString();
                One();

            }
        });
#endif
    }

    void One()
    {
        if (kuy)
        {
            kuy = false;
            if (a == 0)
            {
                PlayGame("Bakiak");
                a++;
            }
        }
    }

    void PlayGame(string val)
    {
        MainCam.SetActive(false);
        MyDebug.init.Log(val);
        MyDebug.init.Log(p1+" m "+p2);
        bangbang[0].SetActive(true);
    }
}
