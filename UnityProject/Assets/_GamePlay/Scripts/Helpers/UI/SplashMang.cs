using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using UnityEngine;

public class SplashMang : MonoBehaviour
{

    public GameObject lostKoneksi;
    public GameObject login;
    public GameObject check;
    public GameObject loginBtn;

    bool imAgree = false;

    private void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false)
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }

    void Start()
    {
        lostKoneksi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ImAgre()
    {
        if (imAgree)
        {
            imAgree = false;
            loginBtn.SetActive(false);
            check.SetActive(false);
        }
        else
        {
            imAgree = true;
            loginBtn.SetActive(true);
            check.SetActive(true);
        }
    }

    public void ShowLostConnetion()
    {
        lostKoneksi.SetActive(true);
    }

    public void RetryConnection()
    {
        if (!GetComponent<LoadingScreen>().CheckInternetConnection()) return;
        GetComponent<LoadingScreen>().Reset();
        lostKoneksi.SetActive(false);
    }

    public void LoginOnPlayGoogle()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            // handle success or failure
            if (success)
            {
                var authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                IntFirebase(authCode);
            }
        });
    }

    void IntFirebase(string key)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential =
            Firebase.Auth.PlayGamesAuthProvider.GetCredential(key);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;

            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            GetComponent<LoadingScreen>().SetKomplitLogin();
        });
    }

    public Firebase.Auth.FirebaseUser GetUSer()
    {
        return Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
    }

    public bool IsLogin()
    {
        Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;

        if (user != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
