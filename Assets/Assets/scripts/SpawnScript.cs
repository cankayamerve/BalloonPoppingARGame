using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnScript : MonoBehaviour
{
    //prefabs
    public GameObject[] balloonPrefabs; 
    public GameObject paperPrefab; 
    public GameObject fireballPrefab; 

    public float spawnInterval = 2.0f; //Objects production frequency
    private ARTrackedImageManager trackedImageManager;
    private bool isTracking = false; //Image Target's tracking status
    private Transform trackedImageTransform;

    public float spawnRange = 0.5f; //Range of horizontal random occurrences (paper and fireball)
    public int maxPapers = 20;  //Maximum number of papers that can be on screen at the same time
    public int maxFireballs = 8;
    private Vector3 smoothedPosition; //To smooth position transitions

    private List<GameObject> activePapers = new List<GameObject>(); 
    private List<GameObject> activeFireballs = new List<GameObject>();

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); 

            if (isTracking && trackedImageTransform != null)
            {
                SpawnBalloons();
                SpawnPaper();
                SpawnFireball();
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    void SpawnBalloons()
    {
        float totalWidth = (balloonPrefabs.Length - 1) * 0.4f; 
        float startX = -totalWidth / 2; 

        for (int i = 0; i < balloonPrefabs.Length; i++)
        {
            float xOffset = startX + (i * 0.6f);
            Vector3 targetPosition = trackedImageTransform.position + new Vector3(xOffset, -3f, 2f);
            smoothedPosition = Vector3.Lerp(smoothedPosition, targetPosition, 1.5f);

            GameObject newBalloon = Instantiate(balloonPrefabs[i], smoothedPosition, Quaternion.identity);
            newBalloon.transform.parent = trackedImageTransform;
        }
    }

    void SpawnPaper()
    {
        if (activePapers.Count >= maxPapers) return;

        float randomX = Mathf.Abs(Random.Range(-spawnRange, spawnRange));
        float randomZ = Mathf.Abs(Random.Range(-spawnRange, spawnRange));
        Vector3 spawnPosition = trackedImageTransform.position + new Vector3(randomX, 1.0f, randomZ * 2 + 0.4f);

        GameObject newPaper = Instantiate(paperPrefab, spawnPosition, Quaternion.identity);
        activePapers.Add(newPaper);

        Destroy(newPaper, 15f); //It disappears from the screen after 15 seconds
        activePapers.Remove(newPaper);
    }

    void SpawnFireball()
    {
        if (activeFireballs.Count >= maxFireballs) return;

        float randomX = Mathf.Abs(Random.Range(-spawnRange, spawnRange));
        float randomZ = Mathf.Abs(Random.Range(-spawnRange, spawnRange));
        Vector3 spawnPosition = trackedImageTransform.position + new Vector3(randomX, 1.4f, randomZ * 3f);

        GameObject newFireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
        activeFireballs.Add(newFireball);

        Destroy(newFireball, 15f); 
        activeFireballs.Remove(newFireball);
    }

    // This method handles the changes in tracked images, updating tracking status and transform based on added updated or removed images
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                isTracking = true;
                trackedImageTransform = trackedImage.transform;
            }
        }

        foreach (var trackedImage in args.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                isTracking = true;
                trackedImageTransform = trackedImage.transform;
            }
            else
            {
                isTracking = false;
                trackedImageTransform = null;
            }
        }

        foreach (var trackedImage in args.removed)
        {
            isTracking = false;
            trackedImageTransform = null;
        }
    }
}
