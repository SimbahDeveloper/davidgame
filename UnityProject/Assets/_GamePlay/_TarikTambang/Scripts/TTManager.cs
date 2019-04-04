using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;

class CharNeed{
    string uid;
    string name;
    string image_url;
    string power;
}

public class TTManager : MonoBehaviour
{
    public float PowerMin = 1f;

    public CapsulScript MyPlayer;
    public CapsulScript EnemyPlayer;
    public GameObject UiPusher;

    public PlayerModel enemyModel = new PlayerModel();
    public PlayerModel myModel = new PlayerModel();

    public static string roomDate = "roomnull";
    public static string enemyUid = "nulle";

    float power = 0f;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;
    bool GameReady = false;

    private void Start()
    {
        GetDatas(roomDate);
    }

    private void Update()
    {
        if (isFirebaseInitialized && GameReady)
        {
            Ready();
           // GetComponent<MyDebug>().Log("ready");
            UiPusher.SetActive(true);
        }
        else
        {
            //GetComponent<MyDebug>().Log("preparibng");
        }
    }

    void Ready()
    {
        EnemyUpdateDateGet(enemyUid);
    }

    public void GetDatas(string dat)
    {
        GetComponent<MyDebug>().Log("preparibng");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                GetComponent<MyDebug>().Log("Oke " + dependencyStatus);
                InitializeFirebase(dat);
            }
            else
            {
                GetComponent<MyDebug>().Log(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    protected virtual void InitializeFirebase(string dat)
    {
        GetComponent<MyDebug>().Log("Initializ  " + PengembangSebelah.ModelFirebase.DatabaseUrl);
        FirebaseApp app = FirebaseApp.DefaultInstance;
        app.SetEditorDatabaseUrl(PengembangSebelah.ModelFirebase.DatabaseUrl);
        if (app.Options.DatabaseUrl != null)
            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        StartGetDat(dat);
        isFirebaseInitialized = true;
    }
    protected void StartGetDat(string dat)
    {
        GetComponent<MyDebug>().Log("Set Data Route  " + dat);
        FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(dat).ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
              if (e2.DatabaseError != null)
              {
                  Debug.LogError(e2.DatabaseError.Message);
                GetComponent<MyDebug>().Log(e2.DatabaseError.Message);
                return;
              }
              if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
              {
                GameReady = true;
                GetComponent<MyDebug>().Log("Get Snapshp cjhild " + e2.Snapshot.ChildrenCount);
                int y = 0;
                foreach (var childSnapshot in e2.Snapshot.Children)
                  {
                    GetComponent<MyDebug>().Log((string)childSnapshot.Child("uid").Value);
                    if ((string)childSnapshot.Child("uid").Value == GameManager.MYUID) {
                        PlayerModel v = new PlayerModel();
                        v.uid = (string)childSnapshot.Child("uid").Value;
                        v.image_url = (string)childSnapshot.Child("image_url").Value;
                        v.name = (string)childSnapshot.Child("name").Value;
                        if (e2.Snapshot.Child("power").Value == null)
                        {
                            v.power = 0f;
                        }
                        else
                        {
                            v.power = (float)e2.Snapshot.Child("power").Value;
                        }
                        v.model_char = (string)childSnapshot.Child("model_char").Value;
                        GetComponent<MyDebug>().Log(v.power+" "+ childSnapshot.Key);
                        myModel = (PlayerModel)v;
                    }
                    else if ((string)childSnapshot.Child("uid").Value == enemyUid)
                      {
                          PlayerModel v = new PlayerModel();
                          v.uid = (string) childSnapshot.Child("uid").Value;
                          v.image_url = (string)childSnapshot.Child("image_url").Value;
                          v.name = (string)childSnapshot.Child("name").Value;
                        if (e2.Snapshot.Child("power").Value == null)
                        {
                            v.power = 0f;
                        }
                        else
                        {
                            v.power = (float)e2.Snapshot.Child("power").Value;
                        }
                        v.model_char = (string)childSnapshot.Child("model_char").Value;
                          enemyModel =(PlayerModel) v;
                          }
                  }
              }
          };
    }

    protected void EnemyUpdateDateGet(string uid)
    {
        FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(roomDate).Child(uid).ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
            PlayerModel v = new PlayerModel();
            v.uid = (string)e2.Snapshot.Child("uid").Value;
            v.image_url = (string)e2.Snapshot.Child("image_url").Value;
            v.name = (string)e2.Snapshot.Child("name").Value;
            if(e2.Snapshot.Child("power").Value == null)
            {
                v.power = 0f;
            }
            else
            {
                v.power = (float)e2.Snapshot.Child("power").Value;
            }
            v.model_char = (string)e2.Snapshot.Child("model_char").Value;
            enemyModel = (PlayerModel)v;

            //Update enemy realtime
            EnemyPlayer.UpdateModel(enemyModel);
            GetComponent<MyDebug>().Log("Enemy Update " + v.power);
        };
    }

    bool CanPush = true;
    public void UpClickPush()
    {
        if (CanPush)
        {
            if (myModel == null) return;
            CanPush = false;
            power += myModel.power + (PowerMin * Time.deltaTime);
            PlayerUpdatePower(power);
        }
    }

    void PlayerUpdatePower(float val)
    { 
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(roomDate).Child(GameManager.MYUID);
        reference.Child("power").SetValueAsync(val);
        MyPlayer.UpdateModel(myModel);
        GetComponent<MyDebug>().Log("Update"+val);
        CanPush = true;
    }


}
