using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    
    public AudioClip audioClip;
    public AudioClip audioClip2;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Managers.Sound.Play(audioClip, Define.Sound.Bgm);
    }
}
