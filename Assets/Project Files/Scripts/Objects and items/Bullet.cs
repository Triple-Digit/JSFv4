using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool m_moveDirection = false; //false = move right, true = move left
    public float m_moveSpeed;
    public float m_destroyTime;
    public GameObject m_impactFx;

    Vector3 currentDirection = Vector3.zero;
    public LayerMask playerLayer;
    float detectionRadius = 0.1f;
    float detectionRateChangePerSec = 2f;
    float detectionRadiusMax = 5;

    private void Awake()
    {
        StartCoroutine("DestroyByTime");
        AkSoundEngine.PostEvent("Fire_Shoot_Player", this.gameObject);
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(m_destroyTime);
        DestroyObject();
    }


    /*public void ChangeDirection()
    {
        m_moveDirection = true;
    }*/

    public void ChangeDirection(Vector3 initialDirection, Collider2D ogPlayer)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ogPlayer, true);
        currentDirection = initialDirection;
    }


    public void DestroyObject()
    {
        
        Destroy(this.gameObject);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, currentDirection, Color.green);

        /*if (!m_moveDirection)
        {
            transform.Translate(Vector3.right * m_moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * m_moveSpeed * Time.deltaTime);
        }*/

        Collider2D playerCollision = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if(playerCollision != null)
        {
            Vector3 proposedDirection = (playerCollision.transform.position - transform.position).normalized;
            if(Vector3.Dot(currentDirection, proposedDirection) > 0.5f)
            {
                currentDirection = Vector3.Lerp(currentDirection, proposedDirection, Time.deltaTime * 10).normalized;
            }
        }

        Debug.DrawRay(transform.position, transform.up * detectionRadius, Color.green);
        Debug.DrawRay(transform.position, -transform.up * detectionRadius, Color.green);
        Debug.DrawRay(transform.position, transform.right * detectionRadius, Color.green);
        Debug.DrawRay(transform.position, -transform.right * detectionRadius, Color.green);

        transform.Translate(currentDirection * m_moveSpeed * Time.deltaTime);

        detectionRadius += detectionRateChangePerSec * Time.deltaTime;
        if (detectionRadius > detectionRadiusMax) detectionRadius = detectionRadiusMax;
    }

    private void OnDrawGizmosSelected()
    {
        if (transform.position == null) return;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AkSoundEngine.PostEvent("Hit_fire_Shoot_Player", this.gameObject);
        GameObject fx = Instantiate(m_impactFx, transform.position, Quaternion.identity);
        Destroy(fx, 5);
        DestroyObject();
    }

}
