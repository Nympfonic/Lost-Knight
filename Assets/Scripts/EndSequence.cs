using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; //NEVER FORGET THIS.

public class EndSequence : MonoBehaviour
{

    private bool TerminateProjectIsDone = false;

    public GameObject TerminateProject;
    private TextMeshProUGUI TerminateProjectText;

    public GameObject Credits;


    public GameObject MusicBox;
    public AudioClip MusicClip;

    public GameObject FXBox;
    public AudioClip Beep;
    public AudioClip BoomBoom;

    // Start is called before the first frame update
    void Start()
    {
        TerminateProjectText = TerminateProject.GetComponent<TextMeshProUGUI>();
        
        StartCoroutine(Main());
        
        
    }

    IEnumerator Main()
    {
        // Wait 2s 
        yield return new WaitForSeconds(2);

        // Load "Terminate Project." w/ updated count down.
        TerminateProject.SetActive(true);

        // Queue TerminateScreen Animation.
        FXBox.GetComponent<AudioSource>().clip = Beep;
        FXBox.GetComponent<AudioSource>().Play();

        TerminateProjectText.text = "TERMINATE PROJECT" + System.Environment.NewLine + System.Environment.NewLine + "SELF DESTRUCT IN" + System.Environment.NewLine + "5";


        yield return new WaitForSeconds(1);

        FXBox.GetComponent<AudioSource>().clip = Beep;
        FXBox.GetComponent<AudioSource>().Play();
        TerminateProjectText.text = "TERMINATE PROJECT" + System.Environment.NewLine + System.Environment.NewLine + "SELF DESTRUCT IN" + System.Environment.NewLine + "4";
        yield return new WaitForSeconds(1);

        FXBox.GetComponent<AudioSource>().clip = Beep;
        FXBox.GetComponent<AudioSource>().Play();
        TerminateProjectText.text = "TERMINATE PROJECT" + System.Environment.NewLine + System.Environment.NewLine + "SELF DESTRUCT IN" + System.Environment.NewLine + "3";
        yield return new WaitForSeconds(1);

        FXBox.GetComponent<AudioSource>().clip = Beep;
        FXBox.GetComponent<AudioSource>().Play();
        TerminateProjectText.text = "TERMINATE PROJECT" + System.Environment.NewLine + System.Environment.NewLine + "SELF DESTRUCT IN" + System.Environment.NewLine + "2";
        yield return new WaitForSeconds(1);

        FXBox.GetComponent<AudioSource>().clip = Beep;
        FXBox.GetComponent<AudioSource>().Play();
        TerminateProjectText.text = "TERMINATE PROJECT" + System.Environment.NewLine + System.Environment.NewLine + "SELF DESTRUCT IN" + System.Environment.NewLine + "1";
        yield return new WaitForSeconds(1);

        FXBox.GetComponent<AudioSource>().clip = Beep;
        FXBox.GetComponent<AudioSource>().Play();
        TerminateProjectText.text = "TERMINATE PROJECT" + System.Environment.NewLine + System.Environment.NewLine + "SELF DESTRUCT IN" + System.Environment.NewLine + "0";

        // Queue Explosion FX
        FXBox.GetComponent<AudioSource>().clip = BoomBoom;
        FXBox.GetComponent<AudioSource>().Play();

        

        // Wait
        yield return new WaitForSeconds(0.25f);

        // Delete SelfDestruct
        Destroy(TerminateProject);

        // Wait
        yield return new WaitForSeconds(5);

        // Load Title Screen + Credits and Roll
        Credits.gameObject.SetActive(true);

        // Play Music Track
        MusicBox.GetComponent<AudioSource>().clip = MusicClip;
        MusicBox.GetComponent<AudioSource>().Play();

        // Wait 
        yield return new WaitForSeconds(10);

        // Trigger Credits Animation.
        Credits.GetComponent<Animator>().SetTrigger("Scroll");

        TerminateProjectIsDone = true;

        yield return new WaitForSeconds(25); // Animation Length

        // Extra 5s.
        // Lower Music and Transition to Main Menu.

        while (MusicBox.GetComponent<AudioSource>().volume > 0)
        {
            MusicBox.GetComponent<AudioSource>().volume -= 0.01f;
            yield return new WaitForSeconds(0.1f);
        }
        
        // Load Main Menu.
        SceneManager.LoadScene(0);


    }

    // Update is called once per frame
    void Update()
    {
        // Check if any key is pressed to skip Credits and load main menu.
        if (TerminateProjectIsDone && Input.anyKeyDown)
        {
            SceneManager.LoadScene(0);
        }
    }
    
}
