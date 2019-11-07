using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerControl : MonoBehaviour
{
    public float speed = 10.0f;
    private bool pause = false;

    bool canSwitch = false;
    bool waitActive = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator Wait()
    {
        waitActive = true;
        yield return new WaitForSeconds(0.1f);
        canSwitch = true;
        waitActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsInputEnabled)
        {
            float translation = Input.GetAxis("Vertical") * speed;
            float straffe = Input.GetAxis("Horizontal") * speed;
            translation += Time.deltaTime;
            straffe *= Time.deltaTime;

            transform.Translate(0, 0, translation);
            transform.Rotate(0, 0,straffe);

            if (Input.GetKeyDown("space"))
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(0, 0.2f, 0);
                    if (!waitActive)
                    {
                        StartCoroutine(Wait());
                    }
                    if (canSwitch)
                    {
                        canSwitch = false;
                    }
                }

            }

            if (Input.GetKeyDown("escape"))
            {
                Cursor.lockState = CursorLockMode.None;

                if (pause)
                    Time.timeScale = 1;
                else
                    Time.timeScale = 0;
                pause = !pause;


            }
        }
    }
}
