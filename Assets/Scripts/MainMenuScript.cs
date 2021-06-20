using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Animator transitionAnim;

    public Slider MasterV;
    public Slider MusicV;
    public Slider FXV;

    private void Start()
    {
        MasterV.value = PlayerPrefs.GetFloat("Master_Volume", 0);
        MusicV.value = PlayerPrefs.GetFloat("Music_Volume", 0);
        FXV.value = PlayerPrefs.GetFloat("FX_Volume", 0);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerController.playerHealth = PlayerController.numOfHearts = 3;
        PlayerController.numOfConsecutiveKills = 0;
        PlayerController.numOfHealthPotions = 0;
    }

    public void Restart()
    {
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        //asyncOperation.allowSceneActivation = false;
        //transitionAnim.SetTrigger("FadeOut");

        //if (transitionAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        //{
        //    asyncOperation.allowSceneActivation = true;
        //}

        SceneManager.LoadScene(1);
        PlayerController.playerHealth = PlayerController.numOfHearts = 3;
        PlayerController.numOfConsecutiveKills = 0;
        PlayerController.numOfHealthPotions = 0;
    }

    public void BackToMainMenu()
    {
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0);
        //asyncOperation.allowSceneActivation = false;
        //transitionAnim.SetTrigger("FadeOut");

        //if (transitionAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        //{
        //    asyncOperation.allowSceneActivation = true;
        //}

        SceneManager.LoadScene(0);

        PlayerController.playerHealth = PlayerController.numOfHearts = 3;
        PlayerController.numOfConsecutiveKills = 0;
        PlayerController.numOfHealthPotions = 0;

        gameObject.GetComponent<PauseMenuScript>().Resume();
    }

    public void QuitGame ()
    {
        //Debug.Log("Quit");
        Application.Quit();
    }

    public void SetVolume_Master(float volume)
    {
        audioMixer.SetFloat("volumeMaster", volume);
    }

    public void SetVolume_Music(float volume)
    {
        audioMixer.SetFloat("volumeMusic", volume);
    }

    public void SetVolume_FX(float volume)
    {
        audioMixer.SetFloat("volumeFX", volume);
    }

    public void IncreaseDeathCount()
    {
        DeathCount.deaths++;
    }

    public void SaveSettings()
    {
        float Master;
        audioMixer.GetFloat("volumeMaster", out Master);

        float Music;
        audioMixer.GetFloat("volumeMusic", out Music);

        float FX;
        audioMixer.GetFloat("volumeFX", out FX);

        PlayerPrefs.SetFloat("Master_Volume", Master);
        PlayerPrefs.SetFloat("Music_Volume", Music);
        PlayerPrefs.SetFloat("FX_Volume", FX);

        PlayerPrefs.Save();

    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(6);
    }
}
