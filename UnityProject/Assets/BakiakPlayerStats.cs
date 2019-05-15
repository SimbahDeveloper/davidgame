using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class BakiakPlayerStats : MonoBehaviour
{
    public string UID = "nono";
    public bool imReady = false;
    public string name = "please wait..";
    GameObject hu;

    private void Start()
    {
        hu = this.gameObject;
    }

    void Update()
    {
        if (UID != "nono")
        {
            FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(GameManager._instance.myRoom).Child(UID).Child("Bakiak").GetValueAsync().ContinueWith(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.ChildrenCount > 0)
                    {
                        var x = (double)snapshot.Child("x").Value;
                        var y = (double)snapshot.Child("y").Value;
                        var z = (double)snapshot.Child("z").Value;
                        var w = new Vector3((float)x, (float)y, (float)z);
                        hu.transform.position = w;
                    }
                }
            });
           // FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(GameManager._instance.myRoom).Child(UID).Child("Bakiak").ValueChanged += ChildChange; 
        }
    }

    void ChildChange(object sender, ValueChangedEventArgs args)
    {
        
        if (args.DatabaseError != null)
        {
            GameObject.Find("Debug").GetComponent<UnityEngine.UI.Text>().text += "New Data "+UID;
            DataSnapshot snapshot = args.Snapshot;
            float x =(float) snapshot.Child("x").Value;
            float y = (float)snapshot.Child("y").Value;
            float z = (float)snapshot.Child("z").Value;
            GameObject.Find("Debug").GetComponent<UnityEngine.UI.Text>().text += "New Data " + UID +x +" "+ y+" "+z;
            //Bakiak.BakiakCamera.init.IkutinDong(new Vector3(Bakiak.BakiakPlayerControler.speed*Time.deltaTime, 0f, 0f));
        }
    }
}
