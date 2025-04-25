using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private List<GameObject> weatherEnvironments;
    private GameObject currentEnvironment;
    // Start is called before the first frame update
    void Start()
    {
        if (weatherEnvironments != null && weatherEnvironments.Count > 0)
        {
            // Deactivate all environments initially
            foreach (GameObject environment in weatherEnvironments)
            {
                environment.SetActive(false);
            }
            
            // Activate the first environment by default
            currentEnvironment = weatherEnvironments[0];
            currentEnvironment.SetActive(true);
        }
    }
    
    public void ActivateEnvironment(int index)
    {
        if (index >= 0 && index < weatherEnvironments.Count)
        {
            if (currentEnvironment != null)
            {
                currentEnvironment.SetActive(false);
            }
            
            currentEnvironment = weatherEnvironments[index];
            currentEnvironment.SetActive(true);

            if (index == 1)
            {
                GameManager.Instance.Boat.transform.position += new Vector3(0, 20, 0);
            }

            if (index == 0)
            {
                GameManager.Instance.Boat.transform.position -= new Vector3(0, 20, 0);
            }

            if (GameManager.Instance.currentLocationIndex == 0)
            {
                GameManager.Instance.TeleportToCurrentLocation();
            }
        }
    }
}
