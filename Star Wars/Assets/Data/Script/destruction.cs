using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class destruction : MonoBehaviour
{

    public Slider healthbar;
    public static int health;

    // Start is called before the first frame update
    void Start()
    {
        health = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = health;

        if (health <= 0)
        {
            SceneManager.LoadScene("Win", LoadSceneMode.Single);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Change the cube color to green.
        Debug.Log(gameObject.name + "From: " + other.name);
        Degat();
    }

    public void Degat()
    {
        health -= 1;
    }
}
