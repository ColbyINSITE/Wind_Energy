using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    public Transform[] player_locations;
    public Transform[] boat_locations;
    public Transform boat;
    private int player_currentLocation = 0;
    private int boat_currentLocation = 0;
    private bool switchScene = false;
    private string[] scenes = { "Turbines - DayScene", "Turbines - Evening", "Turbines - Night" };
    private int index = 1;

    // Update is called once per frame
    void Update()
    {
        /* Teleport the player between 6 locations respectively: mountain, beach, and 4.5 miles,
         * 1 mile, 400 feet from the wind turbine (1)
         * If the boat is going through all of the 6 location, switch to the next scene in this order:
         * "Turbines - Day" is currently played, then "Turbines - Evening", then "Turbines - Night"
        */
        if (Input.GetKeyDown(KeyCode.Return) && player_locations.Length > 0 && boat_locations.Length > 0)
        {
            if (player_currentLocation >= 2)
            {
                /*
                if (player_currentLocation >= 7)
                {
                    Debug.Log(scenes[index]);
                    switchScene = true;
                    SceneManager.LoadScene(sceneName: scenes[index]);
                    index++;
                    Debug.Log(index);
                }
                */

                Debug.Log(player_currentLocation.ToString());

                if (boat != null && this != null && !switchScene)
                {
                    boat.position = boat_locations[(++boat_currentLocation) % boat_locations.Length].position;
                    this.transform.position = player_locations[(++player_currentLocation) % player_locations.Length].position;
                }
            }
            else
            {
                this.transform.position = player_locations[(++player_currentLocation) % player_locations.Length].position;
            }
        }
    }
}