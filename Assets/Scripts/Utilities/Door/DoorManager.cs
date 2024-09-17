using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [Header("Door Properties")]
    [SerializeField] private FloorButtonManager floorButton;

    private void OnEnable()
    {
        floorButton.OnFloorButtonActivated += OnFloorButtonActived;
    }

    private void OnDisable()
    {
        floorButton.OnFloorButtonActivated -= OnFloorButtonActived;
    }

    private void OnFloorButtonActived()
    {
        this.gameObject.SetActive(false);
    }

}
