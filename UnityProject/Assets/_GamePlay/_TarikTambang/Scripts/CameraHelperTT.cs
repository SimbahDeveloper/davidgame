using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelperTT : MonoBehaviour
{
    public bool IsFocusCam = false;
    public bool turnOfAnimateor = false;
    public GameObject playerUtama;

    float posxCam;
    // Start is called before the first frame update
    void Start()
    {
        posxCam = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        CamUse();
    }

    void CamUse()
    {
        if (IsFocusCam)
        {
           // if (playerUtama == null) return;
            float posisiplayer = playerUtama.transform.position.z;
            float posx = posxCam + posisiplayer;
            gameObject.transform.position = new Vector3(transform.position.x,transform.position.y,posx);
        }

        if (turnOfAnimateor)
        {
            TurnOfAnimator();
            turnOfAnimateor = false;
        }
    }
    public void TurnOfAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }

}
