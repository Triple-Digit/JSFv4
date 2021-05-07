using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rocket : MonoBehaviour
{
    [SerializeField] float m_maxRange, rotationSpeed;
    [SerializeField] RotateMode m_rotationMode;
    int m_playerThatShot;
    bool m_rotated;
    bool m_direction; //false = move right, true = move left

    private void Awake()
    {
        m_direction = GetComponent<Bullet>().m_moveDirection;
    }

    public void SetPlayerToIgnore(int index)
    {
        m_playerThatShot = index;
    }

    private void Update()
    {
        RotateTowardsClosestPlayer();
    }

    void RotateTowardsClosestPlayer()
    {
        foreach(AgentManager player in GameManager.instance.m_activePlayers)
        {
            Vector3 target = player.transform.position;
            float distanceToPlayer = Vector3.Distance(transform.position, target);
            if (distanceToPlayer <= m_maxRange && player.m_playerID != m_playerThatShot)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target, rotationSpeed * Time.deltaTime, 0f));
                
            }
            
        }
    }


}
