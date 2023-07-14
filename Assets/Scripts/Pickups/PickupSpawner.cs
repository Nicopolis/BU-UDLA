using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PickupSpawner : MonoBehaviour
{
    private ObjectPool<GameObject> m_pool;

    public GameObject m_pickupPrefab;

    public int m_maxSpawnedPickups;

    public float m_spawnInterval;

    public bool m_usePool;

    private void Start()
    {
        m_pool = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(m_pickupPrefab);
        }, gameObject =>
        {
            gameObject.SetActive(true);
        }, gameObject =>
        {
            gameObject.SetActive(false);
        }, gameObject =>
        {
            Destroy(gameObject);
        }, false, 10, 50);

        InvokeRepeating(nameof(Spawn), 0.2f, m_spawnInterval);
    }

    public void Spawn()
    {
        if(m_pool.CountActive < m_maxSpawnedPickups)
        {
            var spawnedObj = m_usePool ? m_pool.Get() : Instantiate(m_pickupPrefab);

            int randomx = Random.Range(-7, 8);

            int randomy = Random.Range(-7, 8);

            int randomValue = Random.Range(0, 2);

            int outputValue = 1;

            for(int i = 0; i < randomValue; i++)
            {
                outputValue *= 2;
            }

            spawnedObj.GetComponent<NumberPickup>().m_numberValue = outputValue * 3;
            spawnedObj.GetComponent<NumberPickup>().UpdateText();

            spawnedObj.transform.parent = gameObject.transform;

            spawnedObj.transform.localPosition = new Vector3(randomx, randomy, 0);

            spawnedObj.GetComponent<NumberPickup>().Init(RemovePickup);
        }

    }


    private void RemovePickup(GameObject gameObject)
    {
        if (m_usePool) m_pool.Release(gameObject);
        else Destroy(gameObject);
    }
}


