using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMng : MonoBehaviour {
    public string resourcesPath = null;

    private static AudioMng instance = null;
    private Object[] audioArr = null;
    private Queue<AudioClip> audioQueue = new Queue<AudioClip>();
    private Queue<Vector3> vecQueue = new Queue<Vector3>();
    private Queue<float> volumeQueue = new Queue<float>();

    private void Start () {
        audioArr = Resources.LoadAll(resourcesPath);
	}
    private void Update()
    {
        if (audioQueue.Count > 0)
        {
            StartSound();
        }
    }

    public static AudioMng GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType(typeof(AudioMng)) as AudioMng;
            if (!instance)
                Debug.LogError("AudioMng instance is Null,, Cant Find GameObject what have AuudioMng");
        }
        return instance;
    }

    public void PlaySound(string audioName , Vector3 audioPos , float volume)
    {
        for (int i = 0; i < audioArr.Length; ++i)
        {
            if (audioArr[i].name == audioName)
            {
                AudioClip clip = audioArr[i] as AudioClip;
                if (!audioQueue.Contains(clip))
                {
                    audioQueue.Enqueue(clip);
                    vecQueue.Enqueue(audioPos);
                    volumeQueue.Enqueue(volume);
                }

                return;
            }
        }

        Debug.Log("Cant Find Audio Name : " + audioName);
    }

    public void StartSound()
    {
        AudioSource.PlayClipAtPoint(audioQueue.Dequeue(), vecQueue.Dequeue() , volumeQueue.Dequeue() );
    }
}
