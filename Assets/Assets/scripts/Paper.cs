using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour
{
    public float fallSpeed = 0.5f; 
    public float destroyHeight = -3f;  
    private void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime); //falling down
        //Destroy if the y coordinate of the paper falls below a certain value
        if (transform.position.y < destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}
