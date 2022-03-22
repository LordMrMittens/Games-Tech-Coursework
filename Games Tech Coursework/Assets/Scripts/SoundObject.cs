using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField]AudioSource audioSource;
    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
