using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] Button loginButton;

    [SerializeField] TMP_Text errorMessages;

    WWWForm form;

    public void OnLoginButtonClicked()
    {
        loginButton.interactable = false;
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        form = new WWWForm();

        form.AddField("username", username.text);
        form.AddField("password", password.text);

        WWW w = new WWW(DataHandler.url + "login.php", form);
        yield return w;

        if (w.error != null)
        {
            errorMessages.text = "404 not found!";
            Debug.Log("<color=red>" + w.text + "</color>"); // Error
        }
        else
        {
            if (w.isDone)
            {
                if (w.text.Contains("error"))
                {
                    errorMessages.text = "invalid username or password!";
                    Debug.Log("<color=red>" + w.text + "</color>"); // Error
                }
                else
                {
                    DataHandler.player = JsonUtility.FromJson<PlayerData>(w.text);
                    SceneManager.LoadScene(1); // Open loading Screen
                }
            }
        }
        loginButton.interactable = true;

        w.Dispose();
    }
}
