using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Image progressbar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync ()
    {
        StartCoroutine(DataHandler.GetRarities());
        StartCoroutine(DataHandler.GetItems());
        StartCoroutine(DataHandler.GetLocationSpots());
        StartCoroutine(DataHandler.GetPlayerItems());
        StartCoroutine(DataHandler.GetTags());
        StartCoroutine(DataHandler.GetItemTags());

        AsyncOperation game = SceneManager.LoadSceneAsync(2);

        while(game.progress < 0)
        {
            progressbar.fillAmount = game.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
