using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollision : MonoBehaviour
{
    //Adjusting the rotation of the arrow
    void Start()
    {
        transform.rotation = Quaternion.Euler(-135, 0, 0);
    }

    //After hitting objects they disappear and appropriate sounds are played
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Balloon"))
        {
            string collidedObjectName = collision.gameObject.name;

            if (collidedObjectName.Contains("BalloonBody"))
            {
                AudioManager.instance.PlayBalloonShootSound();
                Color balloonColor = collision.gameObject.GetComponent<Renderer>().material.color;
                int points = 0;

                if (balloonColor == Color.red)
                {
                    points = 30; 
                }
                else if (balloonColor == Color.blue)
                {
                    points = 20; 
                }
                else if (balloonColor == Color.green)
                {
                    points = 10; 
                }

                //Adding points to game manager
                GameManager gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.AddScore(points);
                }

                Destroy(collision.transform.parent.gameObject); //Destroys balloon stem and balloon body
                Destroy(gameObject); //destroys the arrow
            }
        }
     
        if (collision.gameObject.CompareTag("Paper"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            AudioManager.instance.PlayErrorSound();

            if (gameManager != null)
            {
                gameManager.AddScore(-10);
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Fireball"))
        {
            AudioManager.instance.PlayErrorSound();
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddScore(-50);
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}
