using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, ActionMap.IPlayerActions
{
    public GameObject menuCanvas;
    public Transform MainCamera;
    public Transform RightController;
    public Transform LeftController;
    public ActionMap controls;
    
    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }
    
    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new ActionMap();
            controls.Player.SetCallbacks(this);
            controls.UIbuttons.SetCallbacks(this);
            controls.Teleportation.SetCallbacks(this);
        }
        //controls.Player.Enable();
    }
    
    public void OnOpenUI(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controls.UIbuttons.Enable();
            controls.Player.Disable();

            RectTransform rectTransform = menuCanvas.GetComponent<RectTransform>();
            rectTransform.position = MainCamera.position + MainCamera.rotation * new Vector3(0, 0, 2f);
            rectTransform.rotation = MainCamera.rotation;
        
            SetUIMenuComponents(true);
        }
    }
    
    public void OnExitUI(InputAction.CallbackContext context)
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
        controls.UIbuttons.Disable();
        controls.Player.Enable();
        SetUIMenuComponents(false);
    }
}
