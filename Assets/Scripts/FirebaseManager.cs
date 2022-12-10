using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using FullSerializer;
public static class FirebaseManager 
{
    private const string ApiKey = "AIzaSyAJT8C9ajwuTjwBP5DWrfViKyZt1RqTfdU"; //TODO: Change [API_KEY] to your API_KEY

    /// <summary>
    /// Signs in a user with their Id Token
    /// </summary>
    /// <param name="token"> Id Token </param>
    /// <param name="providerId"> Provider Id </param>
    public static void SignInWithToken(string token, string providerId)
    {
      
        var payLoad =
            $"{{\"postBody\":\"id_token={token}&providerId={providerId}\",\"requestUri\":\"http://localhost\",\"returnIdpCredential\":true,\"returnSecureToken\":true}}";
        RestClient.Post($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={ApiKey}", payLoad).Then(
            response =>
            {
                // You now have the userId (localId) and the idToken of the user!
                var data = StringSerializationAPI.Deserialize(typeof(SignInResponse), response.Text) as SignInResponse;
                LoginManager.FindObjectOfType<LoginManager>().SetLocalID(data.localId);
                LoginManager.FindObjectOfType<LoginManager>().SetTokenID(data.idToken);
            }).Catch(Debug.Log);
    }
}
