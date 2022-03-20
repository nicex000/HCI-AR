using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject[] levels;

    void Start()
    {
        int levelProgress = PlayerPrefs.GetInt("LevelProgress");
        int lv = -1;
        foreach (var level in levels)
        {
            lv++;
            if (lv == 0) continue;
            if (lv > levelProgress)
            {
                level.GetComponent<Button>().interactable = false;
            }
        }
    }

    // Start is called before the first frame update
    public void SwitchMenu(int menu)
    {
        switch (menu)
        {
            case 1:
                mainMenu.SetActive(false);
                levelSelectMenu.SetActive(true);
                break;
            default:
                mainMenu.SetActive(true);
                levelSelectMenu.SetActive(false);
                break;
        } 
    }

    public void SelectLevel(int level)
    {
        switch (level)
        {
            case 1:
                SceneManager.LoadSceneAsync("Level_1");
                break;
            default: break;
        }

    }


}