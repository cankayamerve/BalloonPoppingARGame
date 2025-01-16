using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsScript : MonoBehaviour
{
    //Update is called once per frame
    void Update()
    {
        //To fix crooked balloon positions
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        transform.Translate(Vector3.forward * Time.deltaTime * 0.3f); //For balloon movement
    } 
}
