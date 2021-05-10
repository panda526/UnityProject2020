using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AudioData
{
    public string name { get { return clip.name; } }
    public string id;
    public AudioClip clip;
}
[CreateAssetMenu(fileName = "SoundDB", menuName = "DB/SoundDB", order = 3)]
public class SoundDB : ScriptableObject
{
    public List<AudioData> audioDatas = new List<AudioData>();
    public AudioClip SearchAudio(string id)
    {
        AudioClip clip = null;
        foreach (AudioData data in audioDatas)
        {
            if (data.id == id || data.name == id)
            {
                clip = data.clip;
                break;
            }
        }
        return clip;
    }
}
    
