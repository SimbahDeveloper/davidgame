using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine;
using Firebase;

public class BakiakCommunication : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public static BakiakCommunication init = null;
    [SerializeField]
    bool isPlay = false;
    string path;
    

    public void setRoomPathParent(string path)
    {
        this.path = path;
    }

    private void OnEnable()
    {
        isPlay = true;
    }

    private void OnDisable()
    {
        isPlay = false;
    }

    private void OnDestroy()
    {
        isPlay = false;
    }

    private void Awake()
    {
        if(init = null)
        {
            init = this;
        }
        ConnectionToRoom(path);
    }

    void ConnectionToRoom(string p)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("rooms").Child(p);
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
