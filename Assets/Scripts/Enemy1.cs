using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemy;
    public float interval = 2f;
    public string filename;

    private string[] spawn;
    private float origint;
    private int line;

    void Start()
    {
        spawn = System.IO.File.ReadAllLines("Assets/Scripts/Stages/"+filename);
        /*foreach (string lines in spawn){
          print(lines);
        }*/
        origint = interval;
        line = 0;
    }

    // Update is called once per frame
    void Update()
    {
     interval -= Time.deltaTime;
     if ((interval <= 0) && (line < spawn.Length))
     {
        int offset = 0;
        foreach(char part in spawn[line]){
           if (Char.IsDigit(part)){
             Vector3 pos = transform.position;
             pos.x+=offset;
             GameObject enem = Instantiate(enemy[(int)Char.GetNumericValue(part)],pos, transform.rotation);
            //enem.velocity = transform.TransformDirection(Vector3.forward * 10);
           }
           offset+=3;
         }
         line+=1;
         interval = origint;
     }

    }
}
