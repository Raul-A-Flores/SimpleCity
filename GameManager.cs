using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public CameraMovement cameraMovement;
    public RoadManager roadManager;


    public InputManager inputManager;

    public UIController uiController;

    private void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnSpecialPlacement += SpecialPlacementHandler;

    }

    private void SpecialPlacementHandler()
    {
        ClearInputActions();
    }

    private void HousePlacementHandler()
    {
        ClearInputActions();
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += roadManager.PlaceRoad;
        inputManager.OnMouseHold += roadManager.PlaceRoad;
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
    }

    private void ClearInputActions()
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
    }

    /*private void HandleMouseClick(Vector3Int position)
    {
       Debug.Log(position);
        roadManager.PlaceRoad(position);
    }
    */

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0, inputManager.CameraMovementVector.y)); 
    }
}
