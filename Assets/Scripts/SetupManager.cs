using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : Singleton<SetupManager>
{
    protected SetupManager() { }

    public GameObject prefab;
    public int nbInsatances;
    public float instanciationRadius;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < nbInsatances; i++)
        {
            Vector2 position2D = Random.insideUnitCircle * instanciationRadius;
            Vector3 position = new Vector3(position2D.x, 0f, position2D.y);
            float angle = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            GameObject instance = Instantiate(prefab, position, rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
