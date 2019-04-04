using UnityEngine;

public class TouchMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(1);
            if(touch.phase == TouchPhase.Began)
            {
                Debug.Log("Hai");
            }
        }
    }
}
