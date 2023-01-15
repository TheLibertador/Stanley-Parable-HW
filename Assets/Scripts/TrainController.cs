using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    [SerializeField] private Vector3 StopPosition_EndOfTheRail;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 startLocation;
    public bool startTrain= false;

    private float increment;
    private void Update()
    {
        if (startTrain)
        {
            if (gameObject.transform.position.z >= StopPosition_EndOfTheRail.z)
            {
                increment += 0.001f * speed;
                gameObject.GetComponent<Rigidbody>().MovePosition(new Vector3(startLocation.x,startLocation.y,startLocation.z-increment));
            }

        }
    }

    public void ResetTrain()
    {
        Debug.Log("Reset Train Position");
        increment = 0;
        gameObject.transform.position = startLocation;
    }
}
