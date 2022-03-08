using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDiscreteMouseFollower : MonoBehaviour
{

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        var worlPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worlPosition = new Vector3(worlPosition.x, worlPosition.y, 0);
        transform.position = Vector3Int.RoundToInt(worlPosition);
    }
}
