using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemy;
    public GameObject[] spawnpts;
    public float interval = 2f;
    public string filename;

    private string[] spawn;
    private float origint;
    private int line;

    void Start()
    {
        spawn = System.IO.File.ReadAllLines("Assets/Scripts/Stages/"+filename);
        foreach (string lines in spawn){
          print(lines);
        }
        origint = interval;
        line = 0;
    }

    // Update is called once per frame
    void Update()
    {
     interval -= Time.deltaTime;
     if ((interval <= 0) && (line < spawn.Length))
     {
        string group = spawn[line];
        int offset = 1;
        int count = (int)Char.GetNumericValue(group[0]);
        //print(group[-1]);
        Vector3 spawnpt = spawnpts[(int)Char.GetNumericValue(group[count+5])].transform.position;
        Vector3 pos = transform.position;
        char format = group[count+3];
        GameObject currenemy;

        if(format == 'v'){
          pos.y-=(offset*(1+count/2));
          spawnpt.y-=(offset*(1+count/2));
          for(int i=0; i<count; i++){
            pos.y += offset;
            spawnpt.y +=offset;
            currenemy = Instantiate(enemy[(int)Char.GetNumericValue(group[2+i])], pos, transform.rotation);
            currenemy.SendMessage("Spawner", spawnpt);
            //currenemy.transform.Translate(spawnpt);

          }
        }
        else if (format == 'h') {
          pos.x-=(offset*(1+count/2));
          spawnpt.x-=(offset*(1+count/2));
          for(int i=0; i<count; i++){
            pos.x += offset;
            spawnpt.x +=offset;
            currenemy = Instantiate(enemy[(int)Char.GetNumericValue(group[2+i])], pos, transform.rotation);
            currenemy.SendMessage("Spawner", spawnpt);
            //currenemy.transform.Translate(spawnpt);

          }
        }

        /*
        foreach(char part in spawn[line]){
           if (Char.IsDigit(part)){
             Vector3 pos = transform.position;
             pos.x+=offset;
             GameObject enem = Instantiate(enemy[(int)Char.GetNumericValue(part)],pos, transform.rotation);
            //enem.velocity = transform.TransformDirection(Vector3.forward * 10);
           }
           offset+=3;
         }*/
         line+=1;
         interval = origint;
     }

    }
}
