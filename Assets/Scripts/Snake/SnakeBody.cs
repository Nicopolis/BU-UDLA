using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeBody : MonoBehaviour
{
    public TextMeshProUGUI m_numberText;

    public float m_moveTime;

    SnakeController sc;

    public int m_currentNumberValue;

    void Awake()
    {
        sc = FindObjectOfType<SnakeController>();
        
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        m_numberText.text = m_currentNumberValue.ToString();
    }

    public void MoveBody(Vector2 dir)
    {
        StartCoroutine(MovePlayer(dir));
    }

    public IEnumerator MovePlayer(Vector2 dir)
    {
        float elapsedTime = 0;

        Vector2 origPos = transform.position;
        Vector2 targetPos = origPos + dir;

        while (elapsedTime < m_moveTime)
        {
            transform.position = Vector2.Lerp(origPos, targetPos, (elapsedTime / m_moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("DIED");
        }
    }
}
