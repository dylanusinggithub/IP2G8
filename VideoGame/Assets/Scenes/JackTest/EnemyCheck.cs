using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    public Transform enemyParent;
    public int enemyCount = 0; //Tracks enemy count

    [SerializeField]
    private GameObject[] objectsToDisable;

    [SerializeField]
    private GameObject[] objectsToEnable;

    void Start()
    {
        //Runs initial check to see if a room is empty by default
        if (enemyParent == null)
        {
            Debug.LogError("Parent object reference is not assigned!");
            return;
        }
        enemyCount = enemyParent.childCount;

        if(enemyCount == 0)
        {
            Objects(); 
        }
    }

    void Update()
    {
        //Runs the object script if the enemy count is equal to 0
        int currentEnemyCount = enemyParent.childCount;

        if (currentEnemyCount != enemyCount)
        {
            enemyCount = currentEnemyCount;
            if (currentEnemyCount == 0)
            {
                Objects();
            }
        }
    }

    void Objects()
    {
        //Disables / Enables the object based on which array they were entered into
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
