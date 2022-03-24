using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUIScript : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private PlaceOnPlane placeScript;
    [SerializeField] private GameObject clearLevelContainer;
    [SerializeField] private int levelNumber;

    private AsyncOperation sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Disable placement";
        
    }

    public void ToggleRaycast()
    {
        if (placeScript.enabled)
        {
            placeScript.enabled = false;
            text.text = "Enable placement";
        }
        else
        {
            placeScript.enabled = true;
            text.text = "Disable placement";
        }
    }

    public void DisableRaycast()
    {
        if (placeScript.enabled)
            ToggleRaycast();
    }

    public void ClearLevel()
    {
        if (sceneLoader != null) return;
        sceneLoader = SceneManager.LoadSceneAsync("Main Menu");
        sceneLoader.allowSceneActivation = false;
        int lv = PlayerPrefs.GetInt("LevelProgress");
        if (lv < levelNumber) PlayerPrefs.SetInt("LevelProgress", levelNumber);

        StartCoroutine(WaitBeforeVictoryScreen());
    }

    IEnumerator WaitBeforeVictoryScreen()
    {
        yield return new WaitForSeconds(3);

        clearLevelContainer.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        sceneLoader.allowSceneActivation = true;
    }
}
