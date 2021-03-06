using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArenaManager : MonoBehaviour
{
    [Header("Main Data")]
    public List<Transform> m_spawnpoints = new List<Transform>();
    [SerializeField] bool m_roundOver;
    public bool m_start, m_suddenDeath, m_stopTimer;

    [Header("UI Data")]
    [SerializeField] TMP_Text m_playerWinText, m_countDownTimer, m_timer;
    float m_totalMatchTime = 30f;
    float m_matchtime;

    [Header("Deathmatch data")]
    [SerializeField] GameObject m_bullets;
    float m_spawnTime = 0.05f;
    Vector2 m_screenBounds;

    [Header("Power Up Data")]
    [SerializeField] GameObject[] m_powerUps;
    float m_powerUpSpawnRate = 15f;
    float m_spawnPowerUpTimer;

    bool m_stingerPlayed;

    private void Start()
    {
        SetUp();
    }

    void Update()
    {
        TrackMatch();
        CountDown();
        SpawnPowerUp();
        PlayStinger();
    }

    void PlayStinger()
    {
        if(!m_stingerPlayed && m_suddenDeath)
        {
            m_stingerPlayed = true;
            AkSoundEngine.PostEvent("Bullet_rain", this.gameObject);
        }
    }

    void SetUp()
    {
        GameManager.instance.ActivatePlayers();

        foreach (AgentManager player in GameManager.instance.m_activePlayers)
        {
            int randomPoint = Random.Range(0, m_spawnpoints.Count);
            player.transform.position = m_spawnpoints[randomPoint].position;
            if (GameManager.instance.m_activePlayers.Count <= m_spawnpoints.Count)
            {
                m_spawnpoints.RemoveAt(randomPoint);
            }
        }
        m_matchtime = 3f;
        AkSoundEngine.PostEvent("Match_Start", this.gameObject);
    }

    void TrackMatch()
    {
        if (GameManager.instance.CheckActivePlayers() == 1 && !m_roundOver)
        {
            m_suddenDeath = false;
            GameManager.instance.m_canFight = false;
            Camera.main.GetComponent<CameraControl>().m_suddenDeath = false;
            m_roundOver = true;
            StartCoroutine(EndRoundCo());
        }
    }

    IEnumerator EndRoundCo()
    {
        m_playerWinText.gameObject.SetActive(true);
        m_playerWinText.text = "Player " + (1 + GameManager.instance.m_lastPlayerNumber) + " wins!";             
        GameManager.instance.AddRoundWin();        
        yield return new WaitForSeconds(3f);
        GameManager.instance.GoToNextArena();
    }

    void CountDown()
    {
        if (m_stopTimer) return;
        m_matchtime -= Time.deltaTime;
        if (!m_start)
        {
            m_countDownTimer.gameObject.SetActive(true);
            m_timer.gameObject.SetActive(false);
            
            if(m_matchtime <= 0)
            {
                
                m_start = true;
                GameManager.instance.m_canFight = true;
                foreach (AgentManager player in GameManager.instance.m_activePlayers)
                {
                    player.TriggerControl();                                        
                }
                m_countDownTimer.gameObject.SetActive(false);
                m_matchtime = m_totalMatchTime;
            }
            m_countDownTimer.text = Mathf.CeilToInt(m_matchtime).ToString();
        }
        else
        {
            
            m_timer.gameObject.SetActive(true);
            
            if(m_matchtime <= 0)
            {
                m_matchtime = 0; 
                m_playerWinText.text = "Sudden Death";                
                m_suddenDeath = true;
                m_stopTimer = true;
                StartCoroutine(ActivateSuddenDeath());                
                Camera.main.GetComponent<CameraControl>().SuddenDeathMode();
            }
            m_timer.text = Mathf.CeilToInt(m_matchtime).ToString();
        }        
    }

    IEnumerator ActivateSuddenDeath()
    {        
        while(m_suddenDeath)
        {
            yield return new WaitForSeconds(m_spawnTime);
            SuddenDeath();
        }        
    }

    void SuddenDeath()
    {        
        m_screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        GameObject a = Instantiate(m_bullets) as GameObject;
        a.transform.position = new Vector2(Random.Range(-m_screenBounds.x, m_screenBounds.x), m_screenBounds.y * 2);
        if(!Camera.main.GetComponent<CameraControl>().m_shaking)
        {
            Camera.main.GetComponent<CameraControl>().m_shaking = true;
        }
        
    }

    void SpawnPowerUp()
    {
        if (!GameManager.instance.m_canFight) return;
        if(m_spawnPowerUpTimer <= 0)
        {
            int randomPoint = Random.Range(0, m_spawnpoints.Count);
            Instantiate(m_powerUps[Random.Range(0, m_powerUps.Length)], m_spawnpoints[randomPoint].position, m_spawnpoints[randomPoint].rotation);
            m_spawnPowerUpTimer = m_powerUpSpawnRate * Random.Range(0.75f, 1.25f);
            AkSoundEngine.PostEvent("Spawn", this.gameObject);
        }
        else
        {
            m_spawnPowerUpTimer -= Time.deltaTime;
        }
    }

}
