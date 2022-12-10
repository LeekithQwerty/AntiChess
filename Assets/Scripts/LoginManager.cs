using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Proyecto26;
using FullSerializer;
using System.Threading;
public class LoginManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayConfirmationText;
    [SerializeField] GameObject usernameGameObject;
    [SerializeField] GameObject googleGameObject;
    public string username;
    public TMP_InputField usernameIF;
    public string password;
    public TMP_InputField passwordIF;

    public TMP_InputField googleCode;

    private const string dataBaseURL = "https://antichess-97b76-default-rtdb.asia-southeast1.firebasedatabase.app/users";
    private string tokenId;
    private string localId;

    private string receiveLocalId;
    bool checkIfLocalIdExists = false;

    public static fsSerializer serializer = new fsSerializer();

    public void setUsername()
    {
        username = usernameIF.text;
    }
    public void setPassword()
    {
        password = passwordIF.text;
    }

    public string getUsername()
    {
        return username;
    }
    public string getPassword()
    {
        return password;
    }

    public string getLocalId()
    {
        return localId;
    }
    public void onCLickGetGoogleCode()
    {
        GoogleAuthHandler.GetAuthCode();
    }

    public void OnClickGoogleLogin()
    {
        Debug.Log(googleCode.text);
        //setPassword();
        GoogleAuthHandler.ExchangeAuthCodeWithIdToken(googleCode.text, idToken =>
        {
            FirebaseManager.SignInWithToken(idToken, "google.com");
        });

        displayConfirmationText.text = "Connecting to server...";
        StartCoroutine(LoadingCheck());
        
    }
    IEnumerator LoadingCheck()
    {
        yield return new WaitForSeconds(4);
        CheckIfLocalIdExists();
        yield return new WaitForSeconds(4);
        bool check = true;
        if (checkIfLocalIdExists == true)
        {
            GetUsernameFromDatabase();
            StartCoroutine(hold());
        }
        else
        {
            googleGameObject.SetActive(false);
            usernameGameObject.SetActive(true);
            displayConfirmationText.text = "Enter your username";
            
        }
        displayConfirmationText.text = "loading...";
    }

    public void OnClickUsername()
    {
        setUsername();
        PostToDatabase();
        StartCoroutine(hold());

    }

    IEnumerator  hold()
    {
        yield return new WaitForSeconds(3);
        Credentials.FindObjectOfType<Credentials>().setUsername(username);
        yield return new WaitForSeconds(3);
        displayConfirmationText.text = "Hello " + username;
        yield return new WaitForSeconds(3);
        SceneLoader.FindObjectOfType<SceneLoader>().LoadNextScene();
    }
    private void GetUsernameFromDatabase()
    {
        Debug.Log(dataBaseURL + " / " + localId + ".json ? auth = " + tokenId);
        RestClient.Get<User>(dataBaseURL + "/" + localId + ".json?auth=" + tokenId).Then(response =>
        {

            username = response.username;
            Debug.Log(username);
        
        }).Catch(error =>
        {
            Debug.Log(error);
        }); 
    }
    public void onSubmit()
    {
        
        PostToDatabase();
    }

    public void SetLocalID(string localIds ) // set the local id got after google sign in 
    {
        localId = localIds;
        
    }
    public void SetTokenID(string tokenId) // set the local id got after google sign in 
    {
        this.tokenId = tokenId;
    }
    private void PostToDatabase()
    {
        User user = new User();
        RestClient.Put(dataBaseURL + "/" + localId + ".json?auth=" + tokenId, user);
    }

    private void RetrieveFromDatabase() //  gets the user of that localId
    {
        RestClient.Get<User>(dataBaseURL + "/" + localId + ".json?auth=" + tokenId).Then(response =>
        {
           // mainUser = response;
            //UpdateScore();
        });
    }

    private void CheckLocalID()   // Check 
    {
        RestClient.Get(dataBaseURL + ".json?auth=" + tokenId).Then(response =>
        {
            var username = getUsername();

            fsData userData = fsJsonParser.Parse(response.Text);
            Dictionary<string, User> users = null;
            serializer.TryDeserialize(userData, ref users);

            foreach (var user in users.Values)
            {
                if (user.username == username)
                {
                    receiveLocalId = user.localId;
                    RetrieveFromDatabase();
                    break;
                }
            }
        }).Catch(error =>
        {
            Debug.Log(error);
        });
    }

    private void  CheckIfLocalIdExists()    // check if there is an account already in the datrabase
    {
       
        RestClient.Get(dataBaseURL + ".json?auth=" + tokenId).Then(response =>
        {
            
            fsData userData = fsJsonParser.Parse(response.Text);
            Dictionary<string, User> users = null;
            serializer.TryDeserialize(userData, ref users);

            foreach (var user in users.Values)
            {
                Debug.Log("The local id is" + user.localId);
                if (user.localId == localId)
                {
                    Debug.Log("IN IF");
                    checkIfLocalIdExists = true;
                    username=user.username;
                    Debug.Log(checkIfLocalIdExists);
                    
                }
            }
        }).Catch(error =>
        {
            Debug.Log(error);
        });
    }
 
}
