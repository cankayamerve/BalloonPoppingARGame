using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float fallSpeed = 0.8f;
    public float destroyHeight = -3f; 
    private void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime); //falling down
        if (transform.position.y < destroyHeight) 
        {
            Destroy(gameObject);
        }
    }
}
