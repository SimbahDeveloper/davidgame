using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject initial;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {

        var f = initial.GetComponentsInChildren<PlayerScript>();
        foreach (var item in f)
        {
            if (item.IsMyChar())
            {
                player = item.gameObject;
            }
        }
    }

    private void LateUpdate()
    {
        CameraHelper helper = new CameraHelper(player);
    }
}
