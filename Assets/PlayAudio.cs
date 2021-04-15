using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource _as;
    public AudioClip[] audioClipArray;

    //    public void Awake()
    //  {
    //    _as = GetComponent<AudioSource>();
    // }

    // Start is called before the first frame update
    public void PlaySound(string soundName)
    {
        _as = GetComponent<AudioSource>();
        _as.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        _as.PlayOneShot(_as.clip);

    }

    public void PlayerSpeak(string soundName)
    {
        GameObject soundGo = GameObject.Find(soundName);
        //AudioSource sound = soundGo.GetComponent<AudioSource>();
        _as = GetComponent<AudioSource>();
        _as.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        _as.PlayOneShot(_as.clip);
    }

    public void DebugSound(string soundName)
    {

        // I FOUND OUT THAT THE OTHER GAMEOBJECTS ARE ON THE OLD SCRIPT IM NOT USING. TESTING NOTIPS 
        // TRYING TO LEARN HOW TO DEBUG LOG SO I CAN SEE WHERE THE HOLDUP IS
        // the array = 0 right now. that's why i am getting the out of bounds array message

        GameObject soundGo = GameObject.Find(soundName);
        Debug.LogWarning("Gameobject: " + soundGo);

        AudioSource sound = soundGo.GetComponent<AudioSource>();
        Debug.LogWarning("audiosource: " + sound);

        Debug.LogWarning("array: " + audioClipArray[1]);
        Debug.LogWarning("array length: " + audioClipArray.Length);
        //sound.PlayOneShot(audioClipArray[Random.Range(0, audioClipArray.Length - 1)]);

    }

}
