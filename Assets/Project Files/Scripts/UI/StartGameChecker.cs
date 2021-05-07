using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartGameChecker : MonoBehaviour
{
    public string m_LevelToLoad;
    [SerializeField] int m_playersReady;
    [SerializeField] TMP_Text m_countDown, m_countdownShadow;
    [SerializeField] float m_timeToStart;
    float m_timer;
    bool m_playSound, m_soundAlreadyPlaying;

    void Update()
    {
        CountDown();
    }

    void CountDown()
    {
        if (m_playersReady > 1 && m_playersReady == GameManager.instance.m_activePlayers.Count)
        {
            m_countDown.gameObject.SetActive(true);
            m_countdownShadow.gameObject.SetActive(true);
            m_timer -= Time.deltaTime;
            m_countDown.text = Mathf.CeilToInt(m_timer).ToString();
            //Sound();
            if (m_timer <= 0f)
            {
                GameManager.instance.StartFirstRound();
            }
        }
        else
        {            
            m_countDown.gameObject.SetActive(false);
            m_countdownShadow.gameObject.SetActive(false);
            m_timer = m_timeToStart;
        }
    }

    void Sound()
    {
        if (!m_soundAlreadyPlaying)
        {
            m_soundAlreadyPlaying = true;
            AkSoundEngine.PostEvent("Match_Start", this.gameObject);
        }
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            m_playersReady++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            m_playersReady--;
        }
    }
}
