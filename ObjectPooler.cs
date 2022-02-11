using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Tooltip("Active script objects in the scene")] [SerializeField] List<GameObject> activeObjects = new List<GameObject>();
    List<GameObject> inactiveObjects = new List<GameObject>();
    [SerializeField] GameObject spawnObject;
    [Tooltip("The distance within the objects will spawn")] [SerializeField] Vector2 resetPosParameters;
    [SerializeField] Vector3 resetRotParameterMin;
    [SerializeField] Vector3 resetRotParameterMax;

    [Tooltip("Max amount of active objects in the scene")] [SerializeField] int activeObjectsThreshold;
    [Tooltip("Decides wether all the children objects of the gameobject are added to the spawnable pool")] [SerializeField] bool addChildrenObjectsToObjectsArray;

    float timer;
    [Tooltip("Timer that decides after how many seconds a new object can spawn")] [SerializeField] float timerThreshold;

    private void Awake()
    {
        if (addChildrenObjectsToObjectsArray)
        {
            for (int index = 0; index < transform.childCount; index++)
            {
                activeObjects.Add(transform.GetChild(index).gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        activeObjects[activeObjects.IndexOf(other.gameObject)].SetActive(false);
        inactiveObjects.Add(activeObjects[activeObjects.IndexOf(other.gameObject)]);
        activeObjects.Remove(other.gameObject);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timerThreshold && activeObjects.Count < activeObjectsThreshold)
        {
            SpawnObjects();
            timer = 0;
        }
    }

    private void SpawnObjects()
    {
        if (inactiveObjects.Count > 0)
        {
            inactiveObjects[0].SetActive(true);

            inactiveObjects[0].transform.position = spawnObject.transform.position + new Vector3(Random.Range(-resetPosParameters.x, resetPosParameters.x), Random.Range(-resetPosParameters.y, resetPosParameters.y), 0);
            inactiveObjects[0].transform.Rotate(Random.Range(resetRotParameterMin.x, resetRotParameterMax.x), Random.Range(resetRotParameterMin.y, resetRotParameterMax.y), Random.Range(resetRotParameterMin.z, resetRotParameterMax.z));

            activeObjects.Add(inactiveObjects[0]);
            inactiveObjects.RemoveAt(0);
        }
    }
}