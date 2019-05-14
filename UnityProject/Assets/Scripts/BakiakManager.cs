using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bakiak
{
    public class BakiakManager : MonoBehaviour
    {
        public GameObject ZonaGame;
        public static BakiakManager init;
        public GameObject[] myPlayer;

        private void Awake()
        {
            if (init == null)
            {
                init = this;
            }
            //ZonaGame.SetActive(false);
        }

         void PlayGameBakiak(BakiakCamera.PLAYERZONEBAKIAK p)
        {
            BakiakCamera.init.SetCameraTo(p);
            SetMyPlayer(p);

        }

        void SetMyPlayer(BakiakCamera.PLAYERZONEBAKIAK p)
        {
            switch (p)
            {
                case BakiakCamera.PLAYERZONEBAKIAK.PLAYER1:
                    if (myPlayer[0] == null) break;
                    myPlayer[0].AddComponent<BakiakPlayerControler>();
                    break;
                case BakiakCamera.PLAYERZONEBAKIAK.PLAYER2:
                    if (myPlayer[1] == null) break;
                    myPlayer[1].AddComponent<BakiakPlayerControler>();
                    break;
                case BakiakCamera.PLAYERZONEBAKIAK.PLAYER3:
                    if (myPlayer[2] == null) break;
                    myPlayer[2].AddComponent<BakiakPlayerControler>();
                    break;
                case BakiakCamera.PLAYERZONEBAKIAK.PLAYER4:
                    if (myPlayer[3] == null) break;
                    myPlayer[3].AddComponent<BakiakPlayerControler>();
                    break;
            }
        }

        void Start()
        {
            PlayGameBakiak(BakiakCamera.PLAYERZONEBAKIAK.PLAYER1);
        }
    }
}
