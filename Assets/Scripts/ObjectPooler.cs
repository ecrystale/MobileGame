using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour{

    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject objectPrefab;
        public int maxObjs;
    }

    void Awake(){
        if(FindObjectsOfType<ObjectPooler>().Length > 1){
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;

    void Start(){
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools){
            Queue<GameObject> objPool = new Queue<GameObject>();

            for(int i = 0; i < pool.maxObjs; i++){
                GameObject obj = Instantiate(pool.objectPrefab);
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }

            poolDict.Add(pool.tag, objPool);
        }
    }

    public void instantiateObjFromPool(string tag, Vector3 pos, Quaternion rotation){
        if(!poolDict.ContainsKey(tag)){
            return;
        }

        GameObject curr = poolDict[tag].Dequeue();

        curr.SetActive(true);
        curr.transform.position = pos;
        curr.transform.rotation = rotation;

        poolDict[tag].Enqueue(curr);
    }
}
