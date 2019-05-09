using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine;
using Firebase;

public class SplashMang : MonoBehaviour
{
    static string FIRSTIMPRESS = "dasdasdas";
    public int FirstImpression()
    {
        return PlayerPrefs.GetInt(FIRSTIMPRESS,0);
    }

    bool fromLogin = false;

    public GameObject lostKoneksi;
    public GameObject login;
    public GameObject check;
    public GameObject loginBtn;

    public static SplashMang init;

    bool imAgree = false;

    DependencyStatus dependencyStatus;

    private void Awake()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false)
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
#endif
    }

    void Start()
    {
        if (init == null)
        {
            init = this;
        }

        dependencyStatus = DependencyStatus.UnavailableOther;
        lostKoneksi.SetActive(false);
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
#if UNITY_ANDROID
            if (success)
            {
                var authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                if (FirstImpression() == 1) return;
                IntFirebase(authCode);
                PlayerPrefs.SetInt(FIRSTIMPRESS, 1);

            }
#else
            LoadingScreen.init.SetKomplitLogin();

#endif
        });

    }

   
    void IntFirebase(string key)
    {
        fromLogin = true;
        FirebaseApp app = FirebaseApp.DefaultInstance;
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(key);

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
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(taskd =>
                {
                    dependencyStatus = taskd.Result;
                    if (dependencyStatus == DependencyStatus.Available)
                    {
                        ;
                        if (app.Options.DatabaseUrl != null)
                            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
                        var user = newUser;

                        //Add To databse
                        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("users").Child(user.UserId);
                   //     GameManager
                        reference.Child("uid").SetValueAsync(user.UserId);
                        reference.Child("name").SetValueAsync(user.DisplayName);
                        reference.Child("image_url").SetValueAsync(user.PhotoUrl);

                        LoadingScreen.init.SetKomplitLogin();

                    }
                    else
                    {
                        LoadingScreen.init.Reset();
                    }

                });
          
        });
    }

    public bool IsLogin()
    {
        if (FirstImpression() == 0)
        {
            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                if (!fromLogin)
                {
                    fromLogin = true;
                    var authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                    IntFirebase(authCode);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            LoginOnPlayGoogle();
            return true;
        }


    }
}
