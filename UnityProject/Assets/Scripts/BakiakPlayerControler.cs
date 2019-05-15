using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Unity.Editor;
using Firebase.Database;
namespace Bakiak
{
    public class BakiakPlayerControler : MonoBehaviour
    {
        public static float speed = 100f;
        public float timeWakeUp = 5f;
        float kuy;
        public bool isPlaying = true;
        
        [SerializeField]
        Vector3 p1Zone;
        Vector3 p2Zone;
        Vector3 playerPos;
        Vector3 _playerPos;
        bool falling;

        bool left ;
        bool firstime;
        // Start is called before the first frame update
        void Start()
        {
            playerPos = gameObject.transform.position;
            _playerPos = playerPos;
            p1Zone = new Vector3(Screen.width * 0.5f,Screen.height,0f);
            p2Zone =new Vector3(Screen.width, Screen.height, 0f);
            firstime = true;
            falling = false;
        }

        // Update is called once per frame
        void Update()
        {
            InputManagerMe();
            Stuned();
        }
        void Stuned()
        {
            if (falling)
            {
                kuy -= Time.deltaTime;
                if (kuy <= 0)
                {
                    GameObject.Find("Debug").GetComponent<UnityEngine.UI.Text>().text += "Ayo Kejar Musuh";
                    //kuy = timeWakeUp;
                    falling = false;
                }
            }
        }
        Vector3 oldPos;
        private void LateUpdate()
        {
            if (oldPos != _playerPos)
            {
                oldPos = _playerPos;
                DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game")
                .Child("rooms")
                    .Child(GameManager._instance.myRoom)
                .Child(GetComponent<BakiakPlayerStats>()
                    .UID).Child("Bakiak");
                reference.Child("x").SetValueAsync((double)_playerPos.x);
                reference.Child("y").SetValueAsync((double)_playerPos.y);
                reference.Child("z").SetValueAsync((double)_playerPos.z);
            }
        }
        Vector2 firstPressPos, secondPressPos, currentSwipe;
        void InputManagerMe()
        {
        
        if (Input.touchCount > 0)
            {
                if (isPlaying && !falling)
                {
                    Touch t = Input.GetTouch(0);
                    if (t.phase == TouchPhase.Began)
                    {
                        //save began touch 2d point
                        firstPressPos = new Vector2(t.position.x, t.position.y);
                    }

                    if (t.phase == TouchPhase.Ended)
                    {
                        //save ended touch 2d point
                        secondPressPos = new Vector2(t.position.x, t.position.y);

                        //create vector from the two points
                        currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                        //normalize the 2d vector
                        currentSwipe.Normalize();

                        //swipe up
                        if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f )
                        {
                            DODO(t);
                        }

                    }
                }
            }
        }

        void DODO(Touch touch)
        {

            var g = Screen.width * 0.5;
            var ss = speed * Time.deltaTime;
            if (firstime)
            {
                firstime = false;
                if (touch.position.x <= g)
                {
                    left = false;
                    _playerPos += new Vector3(ss, 0f, 0f);
                    BakiakCamera.init.IkutinDong(new Vector3(ss, 0f, 0f));
                }
                else if (touch.position.x >= g)
                {
                    left = true;
                    Debug.Log("Right");
                    _playerPos += new Vector3(ss, 0f, 0f);
                    BakiakCamera.init.IkutinDong(new Vector3(ss, 0f, 0f));
                }

            }
            else
            {
                if (touch.position.x <= g)
                {
                    if (left)
                    {
                        left = false;
                        _playerPos += new Vector3(ss, 0f, 0f);
                        BakiakCamera.init.IkutinDong(new Vector3(ss, 0f, 0f));
                    }
                    else
                    {
                        kuy = timeWakeUp;
                        GameObject.Find("Debug").GetComponent<UnityEngine.UI.Text>().text += "Falling " + timeWakeUp + "s";
                        falling = true;
                    }
                }
                else if (touch.position.x >= g)
                {
                    if (!left)
                    {
                        left = true;
                        Debug.Log("Right");
                        _playerPos += new Vector3(ss, 0f, 0f);
                        BakiakCamera.init.IkutinDong(new Vector3(ss, 0f, 0f));
                    }
                    else
                    {
                        kuy = timeWakeUp;
                        GameObject.Find("Debug").GetComponent<UnityEngine.UI.Text>().text += "Falling " + timeWakeUp + "s";
                        falling = true;
                    }
                }
            }
        }
    }
}
