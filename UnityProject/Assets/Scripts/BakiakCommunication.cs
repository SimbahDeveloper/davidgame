using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine;
using Firebase;

namespace Bakiak
{
    public class BakiakCommunication : MonoBehaviour
    {
        public GameObject[] playerObj;

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
            if (init = null)
            {
                init = this;
            }


        }

        void ConnectionToRoom(string p)
        {
            for (int i = 0; i < GameManager._instance.UserPlay.Count; i++)
            {
                playerObj[i].AddComponent<BakiakPlayerStats>();
                playerObj[i].GetComponent<BakiakPlayerStats>().UID = GameManager._instance.UserPlay[i];
                if (GameManager._instance.UserPlay[i] == GameManager._instance.GetPlayer().uid)
                {
                    playerObj[i].GetComponent<BakiakPlayerStats>().master = true;
                    if (i == 0)
                    {
                        Bakiak.BakiakManager.init.PlayGameBakiak(Bakiak.BakiakCamera.PLAYERZONEBAKIAK.PLAYER1);
                    }
                    else if (i == 1)
                    {
                        Bakiak.BakiakManager.init.PlayGameBakiak(Bakiak.BakiakCamera.PLAYERZONEBAKIAK.PLAYER2);
                    }
                    else if (i == 2)
                    {
                        Bakiak.BakiakManager.init.PlayGameBakiak(Bakiak.BakiakCamera.PLAYERZONEBAKIAK.PLAYER3);
                    }
                    else if (i == 3)
                    {
                        Bakiak.BakiakManager.init.PlayGameBakiak(Bakiak.BakiakCamera.PLAYERZONEBAKIAK.PLAYER4);
                    }
                }
            }
        }

        void Start()
        {
#if UNITY_EDITOR
            Bakiak.BakiakManager.init.PlayGameBakiak(Bakiak.BakiakCamera.PLAYERZONEBAKIAK.PLAYER1);
            playerObj[0].AddComponent<BakiakPlayerStats>();
#else

        ConnectionToRoom(GameManager._instance.GetPlayer().uid);
#endif
        }
        void Update()
        {

        }
    }
}
