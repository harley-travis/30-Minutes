using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    //public AudioSource pause;
    //public AudioSource unPause;
    //public AudioSource selectButton;
    //public AudioSource gameStart;
    //public AudioSource gameEnd;
    //public AudioSource money;
    //public AudioSource extraTime;
    //public AudioSource doorBell;

    public AudioSource mainSong;
    private bool pause;
    public AudioSource _as;
    public AudioClip[] audioClipArray;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("music", 1) == 0)
        {
            mainSong.Pause();
        }

        if (PlayerPrefs.GetFloat("volume", 1) < 1)
        {
            mainSong.volume = PlayerPrefs.GetFloat("volume");
        }

        // play round start noise here
        
    }

    public void PauseMusic(bool pause)
    {

        if (pause = false)
        {
            mainSong.UnPause();
        }
        
        if (pause = true)
        {
            mainSong.Pause();
        }
    }

    public void PlayMainSong(bool pause)
    {

        if (pause = true)
        {
            mainSong.Play();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string soundName)
    {
        if (soundName == "Victory")
        {
            mainSong.Pause();
        }

        GameObject soundGo = GameObject.Find(soundName);
        AudioSource sound = soundGo.GetComponent<AudioSource>();
        sound.Play();
    }

    public void PlayerSpeak(string soundName)
    {
        
        // I FOUND OUT THAT THE OTHER GAMEOBJECTS ARE ON THE OLD SCRIPT IM NOT USING. TESTING NOTIPS 
        // TRYING TO LEARN HOW TO DEBUG LOG SO I CAN SEE WHERE THE HOLDUP IS
        // the array = 0 right now. that's why i am getting the out of bounds array message

        GameObject soundGo = GameObject.Find(soundName);
        Debug.LogWarning("Gameobject: "+ soundGo);

        AudioSource sound = soundGo.GetComponent<AudioSource>();
        Debug.LogWarning("audiosource: " + sound);

        Debug.LogWarning("array: " + audioClipArray[1]);
        Debug.LogWarning("array length: " + audioClipArray.Length); 
        //sound.PlayOneShot(audioClipArray[Random.Range(0, audioClipArray.Length - 1)]);

    }


    public void PlayCharacterVoice()
    {

        // I would like to use one function to call all the audio. 
        // however, each group of audio has different amounts
        // how can we dynamically update that to serve one function?s

    }


}


