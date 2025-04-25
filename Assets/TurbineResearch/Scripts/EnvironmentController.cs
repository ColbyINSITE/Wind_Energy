using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeatherRopeManagerPair
{
    public GameObject weather;
    public GameObject ropeManager;
}

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private List<WeatherRopeManagerPair> environments;
    private WeatherRopeManagerPair currentEnvironment;
    // Start is called before the first frame update
    void Start()
    {
        if (environments != null)
        {
            // Deactivate all environments initially
            foreach (var pair in environments)
            {
                pair.weather.SetActive(false);
                pair.ropeManager.SetActive(false);
            }
            
            // Activate the normal weather by default
            currentEnvironment = environments[0];
            currentEnvironment.weather.SetActive(true);
            currentEnvironment.ropeManager.SetActive(true);
        }
    }
    
    public void ActivateEnvironment(int index)
    {
        if (environments != null && index >= 0 && index < environments.Count)
        {
            if (currentEnvironment != null)
            {
                currentEnvironment.weather.SetActive(false);
                currentEnvironment.ropeManager.SetActive(false);
            }
            
            currentEnvironment = environments[index];
            currentEnvironment.weather.SetActive(true);
            currentEnvironment.ropeManager.SetActive(true);

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
