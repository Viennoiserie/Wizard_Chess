using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject offlineSetup;
    [SerializeField] private TMP_InputField whiteName;
    [SerializeField] private TMP_InputField blackName;

    private bool offlineActive;

    public void Awake()
    {
        offlineActive = false;
    }

    // Offline 
    public void loadOffline()
    {
        if (!offlineActive)
        {
            offlineSetup.SetActive(true);
            offlineActive = true;
        }
        else
        {
            offlineSetup.SetActive(false);
            offlineActive = false;
        }
    }

    public void playOffline()
    {
        PlayerPrefs.SetString("WhiteName", whiteName.text);
        PlayerPrefs.SetString("BlackName", blackName.text);

        SceneManager.LoadScene("Offline");
    }
}
