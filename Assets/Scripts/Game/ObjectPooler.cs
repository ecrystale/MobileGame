using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject objectPrefab;
        public int maxObjs;
    }

    void Awake()
    {
        if (FindObjectsOfType<ObjectPooler>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;

    void Start()
    {
        InitializePool();
    }

    public void InitializePool()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int i = 0; i < pool.maxObjs; i++)
            {
                GameObject obj = Instantiate(pool.objectPrefab);
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }

            poolDict.Add(pool.tag, objPool);
        }
    }

    public void CleanAll()
    {
        foreach (Pool pool in pools)
        {
            foreach (GameObject obj in poolDict[pool.tag])
            {
                obj.SetActive(false);
            }
        }
    }

    public GameObject instantiateObjFromPool(string tag, Vector3 pos, Quaternion rotation, bool isUI = false)
    {
        if (poolDict == null)
        {
            InitializePool();
        }

        if (!poolDict.ContainsKey(tag))
        {
            return null;
        }

        GameObject curr = poolDict[tag].Dequeue();

        curr.SetActive(true);
        curr.transform.position = pos;
        curr.transform.rotation = rotation;

        ShotBehaviour shot = curr.GetComponent<ShotBehaviour>();
        shot.IsUI = isUI;

        poolDict[tag].Enqueue(curr);

        return curr;
    }
}
