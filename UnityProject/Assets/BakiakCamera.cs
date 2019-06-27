using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bakiak
{
    public class BakiakCamera : MonoBehaviour
    {
        public float sppedFollow = 10f;
        public static BakiakCamera init;

        Vector3 postiNew;

        public enum PLAYERZONEBAKIAK
        {
            PLAYER1, PLAYER2, PLAYER3, PLAYER4
        }

        private void LateUpdate()
        {
            //            Debug.Log(postiNew + "  " + gameObject.transform.position);
            //MyDebug.init.Log(postiNew + "  " + gameObject.transform.position);
            if (postiNew.x >= gameObject.transform.position.x)
            {
                var f = BakiakPlayerControler.speed/sppedFollow * Time.deltaTime;

                gameObject.transform.position += new Vector3(f, 0f, 0f);
            }
        }
        public void StopFollow()
        {
            hay = false;
        }
        bool hay = true;
        public void IkutinDong(Vector3 vector)
        {
            if (hay)
            {
                postiNew += vector;
            }
//            Debug.Log(postiNew);
        }

        public Vector3 z1 = new Vector3(-82.2f, 14f, -3.2f);
        public Vector3 z2 = new Vector3(-82.2f, 14f, -8.9f);
        public Vector3 z3 = new Vector3(-82.2f, 14f, -20.4f);
        public Vector3 z4 = new Vector3(-82.2f, 14f, -31.6f);

        public void SetCameraTo(PLAYERZONEBAKIAK p)
        {
            switch (p)
            {
                case PLAYERZONEBAKIAK.PLAYER1:
                    gameObject.transform.position = z1;
                    postiNew = z1;
                    break;
                case PLAYERZONEBAKIAK.PLAYER2:
                    gameObject.transform.position = z2;
                    postiNew = z2;
                    break;
                case PLAYERZONEBAKIAK.PLAYER3:
                    gameObject.transform.position = z3;
                    postiNew = z3;
                    break;
                case PLAYERZONEBAKIAK.PLAYER4:
                    gameObject.transform.position = z4;
                    postiNew = z4;
                    break;
            }
        }

        private void Awake()
        {
            if (init == null)
            {
                init = this;
            }
        }
    }
}
