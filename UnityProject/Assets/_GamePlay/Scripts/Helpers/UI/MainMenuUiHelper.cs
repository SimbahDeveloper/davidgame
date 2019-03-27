using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUiHelper : MonoBehaviour
{
    public GameObject blockMe;
    public GameObject Manage;


    Animator _manageAnimator;

    void InitGo()
    {
        _manageAnimator = Manage.GetComponent<Animator>();
    }

    bool cekMan = false;
    public void BackManage()
    {
        _manageAnimator.Play("OnOut");
        cekMan = true;
    }


    private void Update()
    {
        if (cekMan)
        {
            blockMe.SetActive(false);
            if (_manageAnimator.GetCurrentAnimatorStateInfo(0).IsName("OnOut") &&
                _manageAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Manage.SetActive(false);

                cekMan = false;
            }
        }
    }
    private void Start()
    {
        InitGo();
    }
}
