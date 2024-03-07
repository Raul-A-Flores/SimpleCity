using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action OnRoadPlacement, OnHousePlacement, OnSpecialPlacement, OnBigStructurePlacement;

    public Button placeRoadButton, placeHouseButton, placeSpecialButton, placeBigStructureButton;

    public Color outlineColor;

    List<Button> buttonsList;


    private void Start()
    {
        buttonsList = new List<Button> { placeHouseButton, placeRoadButton, placeSpecialButton, placeBigStructureButton };

        placeRoadButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModiftyOutline(placeRoadButton);
            OnRoadPlacement?.Invoke();
        });

        placeHouseButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModiftyOutline(placeHouseButton);
            OnHousePlacement?.Invoke();
        });

        placeSpecialButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModiftyOutline(placeSpecialButton);
            OnSpecialPlacement?.Invoke();
        });

        placeBigStructureButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModiftyOutline(placeBigStructureButton);
            OnBigStructurePlacement?.Invoke();
        });


    }

    private void ModiftyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;

    }

    private void ResetButtonColor()
    {
        foreach (Button button in buttonsList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}
