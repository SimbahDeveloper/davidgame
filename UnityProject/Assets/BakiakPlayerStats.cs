using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

namespace Bakiak
{

    public class BakiakPlayerStats : MonoBehaviour
    {
        public string UID = "nono";
        public bool imReady = false;
        public string name = "please wait..";
        public bool master = false;
        GameObject hu;

        private void Start()
        {
            hu = this.gameObject;
            if (UID != "nono")
            {
                MyDebug.init.Log(GameManager._instance.myRoom);
                MyDebug.init.Log(UID);

#if !UNITY_EDITOR
            FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(GameManager._instance.myRoom).Child(UID).Child("Bakiak").ValueChanged += ChildChange;
            //    if (task.IsCompleted)
            //    {
            //        DataSnapshot snapshot = task.Result;
            //        if (snapshot.ChildrenCount > 0)
            //        {
            //            var x = (double)snapshot.Child("x").Value;
            //            var y = (double)snapshot.Child("y").Value;
            //            var z = (double)snapshot.Child("z").Value;
            //            var w = new Vector3((float)x, (float)y, (float)z);
            //            hu.transform.position = w;
            //        }
            //    }
            //});
            // FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(GameManager._instance.myRoom).Child(UID).Child("Bakiak").ValueChanged += ChildChange; 
#endif
            }
        }
        bool hay = true;
        Vector3 kadir;
        void Update()
        {
#if !UNITY_EDITOR
            FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(GameManager._instance.myRoom).Child(UID).Child("Bakiak").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    var x = (double)snapshot.Child("x").Value;
                    var y = (double)snapshot.Child("y").Value;
                    var z = (double)snapshot.Child("z").Value;

                    var w = new Vector3((float)x, (float)y, (float)z);
                    hu.transform.position = w;
                    if (hay)
                    {
                        hay = false;
                        kadir = w;
                    }

                    if (kadir.x != hu.transform.position.x)
                    {
                        kadir = w;
                        if (master)
                        {
                            BakiakCamera.init.IkutinDong(new Vector3(BakiakPlayerControler.speed * Time.deltaTime, 0f, 0f));
                        }
                    }
                }
            });
#endif
        }

        void ChildChange(object sender, ValueChangedEventArgs args)
        {

            if (args.DatabaseError != null)
            {

            }
        }
    }
}
