using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bakiak
{
    public class BakiakFinish : MonoBehaviour
    {
        List<string> winingList;
        public static BakiakFinish init;
        public Text win;
        public GameObject backMenu,maincam,bakiakcam;
        private void Start()
        {
            if (init == null)
            {
                init = this;
            }
            win.gameObject.SetActive(false);
            backMenu.SetActive(false);
            winingList = new List<string>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.gameObject.GetComponent<BakiakPlayerControler>() != null)
            {
                Debug.Log("ADD");
                winingList.Add("Aku");
                if (collision.transform.gameObject.GetComponent<BakiakPlayerControler>() != null)
                {
                    collision.transform.gameObject.GetComponent<BakiakPlayerControler>().isPlaying = false;
           

                    MyDebug.init.Log("Horrar");
                    string wew="";

                    wew += "#" + winingList.Count;
#if !UNITY_EDITOR
                    wew += " "+GameManager.namePlay;
#endif
                    win.text = wew;
                    win.gameObject.SetActive(true);
                    backMenu.SetActive(true);

                    bakiakcam.SetActive(false);
                    maincam.SetActive(true);
                }


            }
        }
    }
}
