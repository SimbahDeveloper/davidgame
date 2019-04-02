namespace PengembangSebelah.TarikTambang
{
    using UnityEngine;
    using Firebase.Database;
    public class DatabaseHandler
    {
        string child = "Game/Play/";
        public void ReadEnemyPower(string key, string namegame)
        {
            child += namegame;
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance.GetReference(child).ValueChanged += HandleValueChanged;
        }

        void HandleValueChanged(object sender, ValueChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            Debug.Log(args);
        }

        public void WriteMyChar(double val, string namegame, string uid)
        {
            child += namegame+"/"+uid;
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child(child).SetValueAsync(val);
            Debug.Log("UPDATE");
        }
    }
}

