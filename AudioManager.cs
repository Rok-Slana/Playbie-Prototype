using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {/*
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        */
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        //Warn if sound is not avalible
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //float len = s.source.clip.length;
        s.source.Play();
    }


    public IEnumerator PlayTwoStrings(string firstStr, string secondStr)
    {
        Sound s = Array.Find(sounds, sound => sound.name == firstStr);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + firstStr + " not found!");
        }

        float len = s.source.clip.length;
        s.source.Play();
        yield return new WaitForSeconds(len);

        Sound s2 = Array.Find(sounds, sound => sound.name == secondStr);
        if (s2 == null)
        {
            Debug.LogWarning("Sound: " + secondStr + " not found!");
        }

        float len2 = s.source.clip.length;
        s2.source.Play();
        yield return new WaitForSeconds(len2);
    }
    /*
    public IEnumerator PlayMultipleStrings(string[] stringsArr)
    {
        foreach(string str in stringsArr)
        {
            Sound s = Array.Find(sounds, sound => sound.name == str);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + str + " not found!");
            }

            float len = s.source.clip.length;
            s.source.Play();
            yield return new WaitForSeconds(len);
        }        
    }
    */
}
