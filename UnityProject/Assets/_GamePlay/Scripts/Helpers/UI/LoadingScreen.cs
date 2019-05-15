using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject login;
    public static LoadingScreen init;

    private bool isLoading;
    private bool waitLogin = false;
    private bool itsLogin = false;
    private bool goLogin = false;
    bool isLoged;
    public Scrollbar scrollbar;


    float addField = 0.01f;
    float maxCheckConnection = 0.4f;

    private void Awake()
    {
        isLoading = true;
    }
    private void Start()
    {
        if (init == null)
        {
            init = this;
        }
    }
    public void Reset()
    {
        scrollbar.size = 0;
        isLoading = true;
    }
    private void Update()
    {
        //GameObject.FindWithTag("DebugText").GetComponent<Text>().text += " " + GooglePlayGames.PlayGamesPlatform.Instance.IsAuthenticated() + " ";
        if (!CheckInternetConnection())
        {
            isLoading = false;
            SplashMang.init.ShowLostConnetion();
        }

        if (isLoading && !waitLogin)
        {
            scrollbar.size += addField * Time.deltaTime;
            if (SplashMang.init.IsLogin())
            {
                addField = 0.1f;
                itsLogin = true;
                
            }
            else if(!itsLogin)
            {
                login.SetActive(true);
                login.GetComponent<Animator>().SetBool("Open", true);
                isLoading = false;
                waitLogin = true;

            }
            else
            {
                SetKomplitLogin();
            }

        }

        if (scrollbar.size >= maxCheckConnection && !itsLogin) { isLoading = false; SplashMang.init.ShowLostConnetion(); }
    }

    private void LateUpdate()
    {

        if (scrollbar.size >= 1)
        {

            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    public void SetKomplitLogin()
    {
        addField = 0.1f;
        itsLogin = true;
        login.SetActive(false);
        waitLogin = false;
        isLoading = true;
    }

    public bool CheckInternetConnection()
    {
        bool g;
        g = Application.internetReachability != NetworkReachability.NotReachable;
        return g;
    }
}