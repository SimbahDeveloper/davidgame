using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    public static UIGame init;

    public Text ScoreUI;
    public Text NameUI;
    public GameObject Stunes;

    void Start()
    {
        if (init == null)
        {
            init = this;
        }
        Stunes.SetActive(false);
    }

    private void LateUpdate()
    {
        ScoreUI.text = "Score : " + GameManager._instance.MyScore;
        NameUI.text = GameManager._instance.GetPlayer().name;
    }

    public void Stuned(float tot)
    {
        Stunes.SetActive(true);
        Stunes.GetComponent<Scrollbar>().size += tot*0.2f;
    }

    public void StunedDone()
    {
        Stunes.SetActive(false);
        Stunes.GetComponent<Scrollbar>().size = 0;

    }

}
