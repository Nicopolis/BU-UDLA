using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;
using System;

public class NumberPickup : MonoBehaviour
{
    private Action<GameObject> m_removeAction;

    public TextMeshProUGUI m_numberText;

    public int m_numberValue;
    void Start()
    {
        m_numberText.text = m_numberValue.ToString();
    }

    public void UpdateText()
    {
        m_numberText.text = m_numberValue.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<SnakeController>().AddBodyToList(m_numberValue);
             m_removeAction(gameObject);
        }
    }

    public void Init(Action<GameObject> removeAction)
    {
        m_removeAction = removeAction;
    }
}
