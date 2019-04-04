using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject login;


    private bool isLoading;
    private bool waitLogin = false;
    bool isLoged;
    [SerializeField]
    public Scrollbar scrollbar;

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    float addField = 0.01f;
    float maxCheckConnection = 0.4f;
    private void Awake()
    {
        isLoading = true;
    }
    public void Reset()
    {
        scrollbar.size = 0;
        isLoading = true;
    }
    private void Update()
    {
        if (isLoading && !waitLogin)
        {
            scrollbar.size += addField * Time.deltaTime;
            if (GetComponent<SplashMang>().IsLogin())
            {
                addField = 0.1f;
            }
            else
            {
                isLoading = false;
                login.SetActive(true);
                waitLogin = true;
                login.GetComponent<Animator>().SetBool("Open",true);
            }

        }

        if (!CheckInternetConnection())
        {
            isLoading = false;
            GetComponent<SplashMang>().ShowLostConnetion();
        }

        if (scrollbar.size > maxCheckConnection) { isLoading = false; GetComponent<SplashMang>().ShowLostConnetion(); }
    }

    private void LateUpdate()
    {
        if (scrollbar.size >= 1)
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    GetComponent<MyDebug>().Log("Oke " + dependencyStatus);
                    FirebaseApp app = FirebaseApp.DefaultInstance;
                    app.SetEditorDatabaseUrl(PengembangSebelah.ModelFirebase.DatabaseUrl);
                    if (app.Options.DatabaseUrl != null)
                        app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
                    var user = GetComponent<SplashMang>().GetUSer();

                    //Add To databse
                    DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("game").Child("users").Child(user.UserId).Child(GameManager.MYUID);
                    reference.Child("uid").SetValueAsync(user.UserId);
                    reference.Child("name").SetValueAsync(user.DisplayName);
                    reference.Child("image_url").SetValueAsync(user.PhotoUrl);

                    SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);

                }
                else
                {
                    GetComponent<MyDebug>().Log(
                      "Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }
    }

    public void SetKomplitLogin()
    {
        waitLogin = false;
    }

    public bool CheckInternetConnection()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }
}