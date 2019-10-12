using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Menu containers for settings UI
    public GameObject optionsMenu;
    public GameObject settingsMenu;
    public GameObject audioMenu;
    public GameObject controlsMenu;

    // UI Sliders to hold settings values for Sound and Music volume
    public Slider musicSlider;
    public Slider soundSlider;

    // Controls settings, each unique control needs it's own Text field which is assigned from the button text field
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public Text left, right, jump, attack, interact;
    private GameObject currentKey;
    private Color32 normal = new Color32(255, 255, 255, 255);
    private Color32 selected = new Color32(255, 185, 81, 255);

    public void Start()
    {
        optionsMenu.SetActive(false);
        audioMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);

        // Sound and Music value gets stored in PlayerPrefs to be used to set volume on audio objects throughout project
        soundSlider.value = PlayerPrefs.GetFloat("Sound", 0.5f);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 0.5f);

        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "LeftArrow")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "RightArrow")));
        keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space")));
        keys.Add("Attack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Attack", "Z")));
        keys.Add("Interact", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "X")));

        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        jump.text = keys["Jump"].ToString();
        attack.text = keys["Attack"].ToString();
        interact.text = keys["Interact"].ToString();
    }

    public void PlayButtonClicked()
    {
        // Set the scene to load, it must also be part of the Build order in Build Settings
        SceneManager.LoadScene("Game");
    }

    public void OptionsButtonClicked()
    {
        settingsMenu.SetActive(true);
        optionsMenu.SetActive(true);
    }

    public void AudioButtonClicked()
    {
        settingsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }

    public void ControlsButtonClicked()
    {
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void SaveAudioButton()
    {
        PlayerPrefs.SetFloat("Sound", soundSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.Save();
        CancelButtonClicked("Audio");
    }

    public void SaveControlsButton()
    {
        foreach (var key in keys)
        {
            Debug.Log(key.Key + "    " + key.Value.ToString());
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
        CancelButtonClicked("Controls");
    }

    public void CancelButtonClicked(string menu)
    {
        switch (menu)
        {
            case "Settings":
                optionsMenu.SetActive(false);
                break;
            case "Audio":
                audioMenu.SetActive(false);
                break;
            case "Controls":
                controlsMenu.SetActive(false);
                currentKey = null;
                break;
            default:
                Debug.Log("No menu switch specified");
                break;
        }
        settingsMenu.SetActive(true);
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }

    void OnGUI()
    {
        // If in Controls menu and a button is pressed it is set to currentKey by ChangeKey()
        // The button GameObject needs to be named after the PlayerPrefs data it is for, for example the Jump key is bound to "Jump" so the button must be named Jump
        // The text object of the button is changed to match the key that was pressed
        if (currentKey != null)
        {
            Event press = Event.current;
            if (press.isKey)
            {
                Debug.Log(keys[currentKey.name] + "    " + press.keyCode);
                keys[currentKey.name] = press.keyCode;
                Debug.Log(keys[currentKey.name] + "    " + press.keyCode);
                currentKey.transform.GetChild(0).GetComponent<Text>().text = press.keyCode.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        if (currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal;
        }
        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;
    }
}
