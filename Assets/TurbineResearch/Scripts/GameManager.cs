using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, ActionMap.IPlayerActions, ActionMap.IUIActions
{
    
    public GameObject menuCanvas;
    public Transform XRRigPlayer;
    public Transform MainCamera;
    public Transform RightController;
    public Transform LeftController;
    public ActionMap controls;
    public TeleportLocations teleportLocations;
    public int currentLocationIndex = 0;

    public Transform Boat;
    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new ActionMap();
            controls.Player.SetCallbacks(this);
            controls.UI.SetCallbacks(this);
        }
        controls.Player.Enable();
    }
    
    public void OnOpenUI(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controls.UI.Enable();
            controls.Player.Disable();

            RectTransform rectTransform = menuCanvas.GetComponent<RectTransform>();
            rectTransform.position = MainCamera.position + MainCamera.rotation * new Vector3(0, 0, 2f);
            rectTransform.rotation = MainCamera.rotation;
        
            SetUIMenuComponents(true);
        }
    }
    
    public void OnCloseUI(InputAction.CallbackContext context)
    {
        if(context.performed)
            ExitUIState();
    }
    
    private void SetUIMenuComponents(bool state)
    {
        menuCanvas.SetActive(state);
        
        LeftController.GetComponent<XRRayInteractor>().enabled = state;
        RightController.GetComponent<XRRayInteractor>().enabled = state;
        LeftController.GetComponent<LineRenderer>().enabled = state;
        RightController.GetComponent<LineRenderer>().enabled = state;
    }

    private void ExitUIState()
    {
        controls.UI.Disable();
        controls.Player.Enable();
        SetUIMenuComponents(false);
    }
    
    public void EnableNextLocation()
    {
        if (teleportLocations != null && teleportLocations.locations.Count > 0)
        {
            currentLocationIndex = (currentLocationIndex + 1) % teleportLocations.locations.Count;
            TeleportToCurrentLocation();
        }
    }
    
    public void EnablePreviousLocation()
    {
        if (teleportLocations != null && teleportLocations.locations.Count > 0)
        {
            currentLocationIndex--;
            if (currentLocationIndex < 0)
                currentLocationIndex = teleportLocations.locations.Count - 1;
            TeleportToCurrentLocation();
        }
    }
    
    public void TeleportToCurrentLocation()
    {
        if (XRRigPlayer != null && teleportLocations != null && 
            currentLocationIndex >= 0 && currentLocationIndex < teleportLocations.locations.Count)
        {
            XRRigPlayer.position = teleportLocations.locations[currentLocationIndex].transform.position;
            XRRigPlayer.rotation = teleportLocations.locations[currentLocationIndex].transform.rotation;
            ExitUIState();
        }
    }
}
