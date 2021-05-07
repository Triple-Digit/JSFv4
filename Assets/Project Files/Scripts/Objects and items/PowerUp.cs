using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerUp : MonoBehaviour
{
    public bool m_invicibility, m_doubleJump, m_multiShot;
    public float m_powerUpLength = 8f;

    private void Awake()
    {
        AkSoundEngine.PostEvent("Spawn", this.gameObject);
    }

    public void DestroyObject()
    {
        AkSoundEngine.PostEvent("Pick_Up", this.gameObject);
        Destroy(this.gameObject);
    }    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            EnablePower(other.gameObject);
        }
        DestroyObject();
    }

    void EnablePower(GameObject player)
    {
        if (m_invicibility)
        {
            player.GetComponent<AgentManager>().Invincible(m_powerUpLength);
        }
        if (m_doubleJump)
        {
            player.GetComponent<PlayerController>().ExtraJump(m_powerUpLength);
        }
        if (m_multiShot)
        {
            player.GetComponent<PlayerController>().MultiShot(m_powerUpLength);
        }
    }
}
