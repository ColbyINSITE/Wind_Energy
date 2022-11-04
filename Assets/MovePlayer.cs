using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    public Transform[] player_locations;
    public Transform[] boat_locations;
    public Transform boat;
    public Transform turbine;
    public Transform newTurbine;
    private int player_currentLocation = 0;
    private int boat_currentLocation = 0;
    private int turbine_curLocation = 0;
    private bool switchScene = false;
    private string[] scenes = { "Turbines - DayScene", "Turbines - Evening", "Turbines - Night" };
    private int index = 1;

    // Update is called once per frame
    void Update()
    {
        /* Teleport the player between 6 locations respectively: mountain, beach, and 4.5 miles,
         * 1 mile, 400 feet, 100 feet from the wind turbine, within Array View
        */
        if (Input.GetKeyDown(KeyCode.Return) && player_locations.Length > 0 && boat_locations.Length > 0)
        {
            if (player_currentLocation >= 2)
            {
                if (boat_currentLocation >= 5 && turbine != null)
                {
                    turbine.position = newTurbine.position;
                }
                else if (boat != null && this != null && !switchScene)
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