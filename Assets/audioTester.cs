using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioTester : MonoBehaviour
{
    [SerializeField] string m_event;
    [SerializeField] bool change;

    void Update()
    {
        if(change)
        {
            change = false;
            PlaySound();
        }
    }

    void PlaySound()
    {
        AkSoundEngine.PostEvent(m_event, this.gameObject);
    }
}
