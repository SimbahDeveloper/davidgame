using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
[RequireComponent(typeof(SwipeManager))]
public class MainMenuUiHelper : MonoBehaviour
{
    public GameObject blockMe;
    public GameObject Manage;
    public GameObject quitPanel;
    public GameObject leaderboard;
    public GameObject pesanPanel;
    public GameObject connection;

    [Header("prefabs me")]
    public GameObject listLeaderboard;

    Animator _manageAnimator;
    Transform listing;

    public float paddleSpeed = 1;
    public Vector3 playerPos;
    private Vector2 touchOrigin = -Vector2.one;

    bool kelu = false;
    void HandleSwipe(SwipeAction swipeAction)
    {
        //Debug.LogFormat("HandleSwipe: {0}", swipeAction);
        if (swipeAction.direction == SwipeDirection.Up || swipeAction.direction == SwipeDirection.UpRight)
        {
            Debug.Log("up");
        }
        else if (swipeAction.direction == SwipeDirection.Right || swipeAction.direction == SwipeDirection.DownRight)
        {
            if (leaderboard.activeInHierarchy&&!kelu)
            {
             //   kelu = true;
               // leaderboard.GetComponent<Animator>().Play("LederIn");
            }
            else
            {
                //kelu = true;
                //leaderboard.SetActive(true);
            }

            Debug.Log("right");
        }
        else if (swipeAction.direction == SwipeDirection.Down || swipeAction.direction == SwipeDirection.DownLeft)
        {
            Debug.Log("down");
        }
        else if (swipeAction.direction == SwipeDirection.Left || swipeAction.direction == SwipeDirection.UpLeft)
        {
            // move left
            if (kelu)
            {
                //kelu = false;
                //leaderboard.GetComponent<Animator>().Play("LeaderOut");
            }

            Debug.Log("left");
        }
    }


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

    public GameObject GetConnectionPanel()
    {
        return connection;
    }

    public void onPlayGameClik()
    {
        escapeKill = true;
        blockMe.SetActive(true);
        connection.SetActive(true);
    }

    public void OnlinePlay()
    {
        GameObject.Find("Scripts").GetComponent<GameManager>().StartGame(connection);
    }

    public void CancelFindRoom()
    {
        blockMe.SetActive(false);
        connection.GetComponent<Animator>().Play("OnOut");
        GameManager._instance.CancelFindRoom();
        cekcon = true;
    }

    IEnumerator Send (string pesan)
    {
        escapeKill = true;
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
        connection.SetActive(false);
        leaderboard.SetActive(false);
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
    bool cekcon = false;
    bool escapeKill = false;
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
                escapeKill = false;
            }
        }
        if (cekcon)
        {
            blockMe.SetActive(false);
            if (connection.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("OnOut") &&
                connection.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                connection.SetActive(false);
                cekcon = false;
                escapeKill = false;
            }
        }

        #region backscape
        if (Input.GetKeyDown(KeyCode.Escape)&&!escapeKill)
        {
            if (quitPanel.activeInHierarchy)
            {
                playOnClick();
                HideQuite();
            }
            else
            {
                playOnClick();
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
        SwipeManager swipeManager = GetComponent<SwipeManager>();
        swipeManager.onSwipe += HandleSwipe;

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
