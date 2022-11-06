using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //private void Awake()
    //{
    //    Init();
    //}
    AudioSource[] audioSource = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    public void Init()
    {
        audioSource[(int)Define.Sound.BGM] = Instantiate(new GameObject("BgmSource")).AddComponent<AudioSource>();
        audioSource[(int)Define.Sound.BGM].gameObject.transform.parent = Camera.main.transform;
        audioSource[(int)Define.Sound.Effect] = Instantiate(new GameObject("EffectSource")).AddComponent<AudioSource>();
        audioSource[(int)Define.Sound.Effect].gameObject.transform.parent = Camera.main.transform;
        Play("BGM_02", Define.Sound.BGM);
        audioSource[(int)Define.Sound.BGM].loop = true;
    }


    public void Clear(Define.Sound type)
    {
        if (type == Define.Sound.BGM)
        {
            audioSource[(int)Define.Sound.BGM].clip = null;
            audioSource[(int)Define.Sound.BGM].Stop();
        }
        else if (type == Define.Sound.Effect)
        {
            audioSource[(int)Define.Sound.Effect].clip = null;
            audioSource[(int)Define.Sound.Effect].Stop();
        }

        audioClips.Clear();
    }
    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.BGM) // BGM 배경음악 재생
        {
            AudioSource tempSource = audioSource[(int)Define.Sound.BGM];
            if (tempSource.isPlaying)
                tempSource.Stop();

            tempSource.pitch = pitch;
            tempSource.clip = audioClip;
            tempSource.Play();
        }
        else // Effect 효과음 재생
        {
            AudioSource tempSource = audioSource[(int)Define.Sound.Effect];
            tempSource.pitch = pitch;
            tempSource.PlayOneShot(audioClip);
        }
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }
    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (!path.Contains("Sounds/"))
            path = $"Sounds/{path}"; 

        AudioClip audioClip = null;

        if (type == Define.Sound.BGM) // BGM 배경음악 클립 붙이기
        {
            audioClip = Resources.Load<AudioClip>(path);
        }
        else // Effect 효과음 클립 붙이기
        {
            if (!audioClips.TryGetValue(path, out audioClip))
            {
                audioClip = Resources.Load<AudioClip>(path);
                audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

    public void Clear()
    {
        
    }
}


