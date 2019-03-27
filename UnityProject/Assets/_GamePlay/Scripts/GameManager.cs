﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
