using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class MainMenuUiHelper : MonoBehaviour
{
    public GameObject blockMe;
    public GameObject Manage;
    public GameObject quitPanel;
    public GameObject leaderboard;
    public GameObject pesanPanel;

    [Header("prefabs me")]
    public GameObject listLeaderboard;

    Animator _manageAnimator;
    Transform listing;
    public void SetLeaderboard(List<LeaderScore> scores)
    {
        List<LeaderScore> score;
        score = scores;
        //score.Sort();
        for (int i = 0; i < score.Count; i++)
        {
            var tran = listLeaderboard.GetComponentsInChildren<Transform>();
            Instantiate(listLeaderboard, listing);
            tran[2].GetComponent<Text>().text = score[i].Nama;
            tran[3].GetComponent<Text>().text = score[i].Score;
        }
    }
    public void SendMessage(string s)
    {
        StartCoroutine(Send(s));
    }

    IEnumerator Send (string pesan)
    {
        blockMe.SetActive(true);
        pesanPanel.SetActive(true);
        var ff = pesanPanel.GetComponentsInChildren<Transform>();
        ff[2].GetComponent<Text>().text = pesan;
        yield return new WaitForSeconds(2);
        pesanPanel.GetComponent<Animator>().Play("OnOut");
        cekmes = true;
    }


    private void Awake()
    {
        Manage.SetActive(false);
        quitPanel.SetActive(false);
        pesanPanel.SetActive(false);
        listing = leaderboard.GetComponentsInChildren<Transform>()[2];

        //Test
        List<LeaderScore> dd = new List<LeaderScore>();
        for (int i = 0; i < 5; i++)
        {
            var leaders = new LeaderScore();
            leaders.Nama = "Nama " + i;
            leaders.Score = "Score " + i;
            dd.Add(leaders);
        }
        SetLeaderboard(dd);
    }

    void InitGo()
    {
        _manageAnimator = Manage.GetComponent<Animator>();
    }

    bool cekMan = false;
    bool cekque = false;
    bool cekmes = false;
    public void BackManage()
    {
        _manageAnimator.Play("OnOut");
        cekMan = true;
    }

    void ShowQuit()
    {
        quitPanel.SetActive(true);
        blockMe.SetActive(true);
    }

    public void HideQuite()
    {
        quitPanel.GetComponent<Animator>().Play("OnOut");
        cekque = true;
    }

    public void ImOut()
    {
        Application.Quit();
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
        if (cekque)
        {
            blockMe.SetActive(false);
            if (quitPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("OnOut") &&
                quitPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                quitPanel.SetActive(false);
                cekque = false;
            }
        }
        if (cekmes)
        {
            blockMe.SetActive(false);
            if (pesanPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("OnOut") &&
                pesanPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                pesanPanel.SetActive(false);
                cekmes = false;
            }
        }

        #region backscape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (quitPanel.activeInHierarchy)
            {
                HideQuite();
            }
            else
            {
                if (!Manage.activeInHierarchy)
                {
                    ShowQuit();
                }
                else
                {
                    BackManage();
                }
            }
        }
        #endregion backscape

    }

    public void playOnClick()
    {
        GameObject.Find("Audio").GetComponent<AudioHelper>().PlayClick();
    }

    AudioHelper _audioMan;
    private void Start()
    {
        #region AudioManagement

        var AudioScrp = GameObject.Find("Audio").GetComponent<AudioHelper>();
        var kacang = Manage.GetComponentsInChildren<Transform>();
        _audioMan = AudioScrp;
        _audioMan.PlayMusic(AudioHelper.MENUMUSIC);

        kacang[5].GetComponent<Slider>().value = AudioScrp.GetVolumeSFX();
        kacang[13].GetComponent<Slider>().value = AudioScrp.GetVolumeMusic();

        #endregion AudioManagement
        InitGo();
    }

    public void SetAudioSFX(float val)
    {
        _audioMan.SetSFXVolume(val);
    }
    public void SetAudioMusic(float val)
    {
        _audioMan.SetMusicVolume(val);
    }
}
