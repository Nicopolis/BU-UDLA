using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SnakeController : MonoBehaviour
{
    InputManager m_inputManager;

    public List<Transform> m_childList;

    public List<SnakeBody> m_bodyList;

    public List<Vector2> m_dirHistory;

    public List<int> m_bodyValues;

    public GameObject m_snakeBody;

    public float m_moveTime;

    Vector2 origPos, targetPos;

    public RaycastHit2D rayHit;
    
    float x, y;

    bool m_isMoving,m_canSpawnBody;

    public int bodyQueue;

    private void Start()
    {
        m_inputManager = GetComponent<InputManager>();
        x = 1;
    }

    public void LateUpdate()
    {
        Movement();
    }

    private void Update()
    {
        if (m_inputManager.spacePressed)
        {
            bodyQueue++;
        }
        
        MyInput();
    }

    public void AddBodyToList(int x)
    {
        m_bodyValues.Insert(0, x);
        bodyQueue++;
    }

    public void SpawnBody()
    {
        var newBody = Instantiate(m_snakeBody, m_childList[m_childList.Count - 1].transform.position, Quaternion.identity);
        m_childList.Add(newBody.transform);
        m_bodyList.Add(newBody.GetComponent<SnakeBody>());
        m_bodyList[m_bodyList.Count - 1].m_currentNumberValue = m_bodyValues[0];

        m_bodyValues.RemoveAt(0);
        bodyQueue--;
        CheckBodys();
    }

    public void CheckBodys()
    {
        for(int i = 0; i < m_childList.Count - 1; i++)
        {
            if(m_bodyList[i].m_currentNumberValue == m_bodyList[i + 1].m_currentNumberValue)
            {
                m_bodyList[i].m_currentNumberValue *= 2;
                m_bodyList[i].UpdateText();
                Debug.Log("AD");

                Destroy(m_bodyList[i + 1].gameObject,1f);
                m_bodyList.Remove(m_bodyList[i + 1]);
                m_childList.Remove(m_childList[i + 1]);
                Invoke("CheckBodys",.5f);
            }
        }
    }

    private void MyInput()
    {
        if (m_inputManager.m_movementInput.y == 1 && y != -1)
        {
            y = 1;
            x = 0;
        }
        else if (m_inputManager.m_movementInput.x == 1 && x != -1)
        {
            y = 0;
            x = 1;
        }
        else if (m_inputManager.m_movementInput.y == -1 && y != 1)
        {
            y = -1;
            x = 0;
        }
        else if (m_inputManager.m_movementInput.x == -1 && x != 1)
        {
            y = 0;
            x = -1;
        }
    }
    

    public void Movement()
    {
        if (!m_isMoving)
        {
            if (y == 1)
            {
                StartCoroutine(MovePlayer(Vector2.up));
                MoveChildren();
                SaveLastMovement(Vector2.up);
            }
            if (x == 1)
            {
                StartCoroutine(MovePlayer(Vector2.right));
                MoveChildren();
                SaveLastMovement(Vector2.right);
            }
            if (y == -1)
            {
                StartCoroutine(MovePlayer(Vector2.down));
                MoveChildren();
                SaveLastMovement(Vector2.down);
            }
            if (x == -1)
            {
                StartCoroutine(MovePlayer(Vector2.left));
                MoveChildren();
                SaveLastMovement(Vector2.left);
            }

            if (bodyQueue > 0)
            {
                SpawnBody();
            }
        }
    }   

    public IEnumerator MovePlayer(Vector2 dir)
    {
        m_isMoving = true;
        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + dir ;

        while (elapsedTime < m_moveTime)
        {
            transform.position = Vector2.Lerp(origPos, targetPos, (elapsedTime / m_moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        m_isMoving = false;
    }

    public void SaveLastMovement(Vector2 dir)
    {
        m_dirHistory.Insert(0, dir);
        
        if(m_dirHistory.Count >= m_childList.Count)
        {
            m_dirHistory.RemoveAt(m_childList.Count + 1);
        }


        m_canSpawnBody = true;
    }

    public void MoveChildren()
    {
        if(m_dirHistory.Count > 0)
        {
            for(int i = 0; i < m_childList.Count; i++)
            {
                Vector2 dir = m_dirHistory[i];
                m_childList[i].GetComponent<SnakeBody>().MoveBody(dir);
            }
        }

    }
}
