using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    public GameObject arrowPrefab; 
    public float shootForce = 500f; //Throwing power of the arrow
    public Transform shootPoint; //The starting point of the arrow
    public float maxRayDistance = 100f; //Max beam distance
    private bool hasStartedTimer = false; //Timer started?

    //When you touch the screen it causes the arrow to appear and the timer starts working
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                ShootArrow(touch.position); 

                if (!hasStartedTimer)
                {
                    FindObjectOfType<GameManager>().StartGame(); //Start timer
                    hasStartedTimer = true;
                }
            }
        }
    }

    //Shoots an arrow towards the target point based on the user's touch input
    void ShootArrow(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            targetPoint = hit.point; //Take the collision point as target
        }
        else
        {
            // If it doesn't hit any objects, set a point away in the direction of the rail
            targetPoint = ray.origin + ray.direction * maxRayDistance;
        }

        GameObject newArrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        Vector3 direction = (targetPoint - shootPoint.position).normalized;
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();
        rb.AddForce(direction * shootForce);
    }
}
