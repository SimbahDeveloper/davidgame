using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bakiak
{
    public class BakiakPlayerControler : MonoBehaviour
    {
        public static float speed = 100f;
        public bool isPlaying = true;
        [SerializeField]
        Vector3 p1Zone;
        Vector3 p2Zone;

        bool left = true;

        // Start is called before the first frame update
        void Start()
        {
            p1Zone = new Vector3(Screen.width * 0.5f,Screen.height,0f);
            p2Zone =new Vector3(Screen.width, Screen.height, 0f);
        }

        // Update is called once per frame
        void Update()
        {
            InputManagerMe();
        }

        void InputManagerMe()
        {
#if UNITY_EDITOR
            if (Input.touchCount > 0 && isPlaying)
            {
                var ss = speed * Time.deltaTime;
              //  if (Input.GetTouch(0) = null) return;
                    Touch touch = Input.GetTouch(0);
                    var g = Screen.width * 0.5;
                    if (touch.position.x <= g && left)
                    {
                        left = false;
                        Debug.Log("Left");
                    gameObject.transform.position += new Vector3(ss, 0f, 0f);
                    BakiakCamera.init.IkutinDong(new Vector3(ss, 0f, 0f));

                    }
                    else if(touch.position.x >= g && !left)
                    {
                        left = true;
                        Debug.Log("Right");
                    gameObject.transform.position += new Vector3(ss, 0f, 0f);
                    BakiakCamera.init.IkutinDong(new Vector3(ss, 0f, 0f));
                }
             
            }
#endif
        }
    }
}
