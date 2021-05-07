using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextShadow : MonoBehaviour
{
    Vector2 m_offset = new Vector2(-0.14f, -0.14f);
    [SerializeField] TMP_Text m_mainText, m_shadowText;
    [SerializeField] Color m_shadowColor;
    Transform m_casterTransform, m_shadowTransform;

    void Start()
    {
        m_casterTransform = m_mainText.transform;
        m_shadowTransform = transform;
        m_shadowText = gameObject.GetComponent<TMP_Text>();
    }

    
    void Update()
    {        
        m_shadowTransform.position = new Vector2(m_casterTransform.position.x + m_offset.x, m_casterTransform.position.y + m_offset.y);
        m_shadowText.text = m_mainText.text.ToString();
    }
}
