using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject myEnemie;
    public float myPower { get; set; }
    public string myName { get; set; }
    TextMesh _nameMesh;
    // Start is called before the first frame update
    void Start()
    {
        _nameMesh = gameObject.GetComponentsInChildren<Transform>()[1].GetComponent<TextMesh>();
        _nameMesh.text = myName;
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(myPower);
    }
}
