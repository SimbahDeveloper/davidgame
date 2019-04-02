using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarikTambang
{
    public float power;
    public string uid;
}

public enum ServerPlayerModel
{
    model1,model2,model3,model4,model5,model6
}

public class PlayerScript : MonoBehaviour
{
    string refers = "Game/Play/TarikTambang";
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;

    public ServerPlayerModel model;
    public GameObject myEnemie;
    public float myPower { get; set; }
    public string myName { get; set; }
    TextMesh _nameMesh;

    public ServerPlayerModel GetModelPlayer()
    {
        return model;
    }
    bool isMainChar = false;
    string enemy = "";
    string mykey = "erere";
    public void SetMyChar(string enemy, string mykey)
    {
        isMainChar = true;
        this.enemy = enemy;
        this.mykey = mykey;

    }
    public bool IsMyChar()
    {
        return isMainChar;
    }
    // Start is called before the first frame update
    void Start()
    {
        FirebaseOpenCon();
        _nameMesh = gameObject.GetComponentsInChildren<Transform>()[1].GetComponent<TextMesh>();
        _nameMesh.text = myName;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMyChar())
        {

        }
        AddPower();
        // Debug.Log(myPower);
    }

    protected virtual void FirebaseOpenCon()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        // NOTE: You'll need to replace this url with your Firebase App's database
        // path in order for the database connection to work correctly in editor.
        app.SetEditorDatabaseUrl("https://freedomgame-d0095.firebaseio.com/");
        if (app.Options.DatabaseUrl != null)
            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        if (isMainChar)
        {
            StartListener(mykey);
        }
        else
        {

        }
        isFirebaseInitialized = true;
    }

    protected void StartListener(string key)
    {

    }

    public void AddPower(TarikTambang tarik)
    {
        string json = JsonUtility.ToJson(tarik);
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(refers);
        reference.Child(tarik.uid).SetRawJsonValueAsync(json);
    }
    public void AddPower()
    {
        TarikTambang tarik = new TarikTambang();
        tarik.power = myPower;
        tarik.uid = "25ewtrf";
        string json = JsonUtility.ToJson(tarik);
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(refers);
        reference.Child(tarik.uid).SetRawJsonValueAsync(json);
    }

}
