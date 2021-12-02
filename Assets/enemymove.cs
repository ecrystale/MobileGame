using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemymove : MonoBehaviour
{
    public Vector3 endpos = new Vector3(0, 0, 0);
    public float speed;
    //public float orig = 0.01f;
    //public float interval = 40f;
    // Start is called before the first frame update
    void Start()
    {
        speed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        //interval -= Time.deltaTime;
        //if (interval <= 0){
        //transform.position = Vector3.Lerp(transform.position, endpos, spd*(Time.deltaTime/interval));
        //  interval = orig;
        //}
        transform.position = Vector3.Lerp(transform.position, endpos, speed * Time.deltaTime);
    }

    public void Spawner(Vector3 pos)
    {
        endpos = pos;
    }
}
