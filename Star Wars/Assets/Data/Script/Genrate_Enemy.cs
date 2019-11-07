using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genrate_Enemy : MonoBehaviour
{

    public int nbrEnemies;
    public static int passEnemies;
    public GameObject[] enemy;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<nbrEnemies; i++)
        {
            Instantiate(enemy[Random.Range(0, enemy.Length)]);
        }
        passEnemies = nbrEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
