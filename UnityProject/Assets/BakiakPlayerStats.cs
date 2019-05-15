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

    void Update()
    {
        if (UID != "nono")
        {
            FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(GameManager._instance.myRoom).Child(UID).Child("Bakiak").ValueChanged += ChildChange; 
        }
    }

    void ChildChange(object sender, ValueChangedEventArgs args)
    {
        
        if (args.DatabaseError != null)
        {
            GameObject.Find("Debug").GetComponent<UnityEngine.UI.Text>().text += "New Data "+UID;
            DataSnapshot snapshot = args.Snapshot;
            var x = snapshot.Child("x").Value;
            var y = snapshot.Child("y").Value;
            var z = snapshot.Child("z").Value;
        }
    }
}
