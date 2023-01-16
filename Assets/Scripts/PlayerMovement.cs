using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jump = 1f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public GameObject Button1;
    public GameObject Button2;
    private Vector3 button1firstpos;
    private Vector3 button2firstpos;
    [SerializeField] private float wait_before_trains_start = 2;

    [SerializeField] private Transform[] AllDoors;

    [SerializeField] private float kapi_kac_derece_acilsin = -147f;
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        CheckButtons();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StartTrain"))
        {
            StartCoroutine(startTrain());
        }
        
    }

    IEnumerator startTrain()
    {
        yield return new WaitForSeconds(wait_before_trains_start);
        GameObject.Find("Train1").GetComponent<TrainController>().startTrain = true;
        GameObject.Find("Train2").GetComponent<TrainController>().startTrain = true;
    }

    private void Start()
    {
        button1firstpos = Button1.transform.localPosition;
        button2firstpos = Button2.transform.localPosition;
    }

    private void CheckButtons(){
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject.Find("Train1").GetComponent<TrainController>().ResetTrain();
            var pos = new Vector3(Button1.transform.localPosition.x, -0.018f, Button1.transform.localPosition.z);
            Button1.transform.DOLocalMove(pos, 0.2f)
                .OnComplete(()=> Button1.transform.DOLocalMove(button1firstpos, 0.2f));
        }
        else if (Input.GetKey(KeyCode.R))
        {
            GameObject.Find("Train2").GetComponent<TrainController>().ResetTrain();
            var firstpos = Button2.transform.position;
            var pos = new Vector3(Button2.transform.localPosition.x, -0.018f, Button2.transform.localPosition.z);
            Button2.transform.DOLocalMove(pos, 0.2f)
                .OnComplete(()=> Button2.transform.DOLocalMove(button2firstpos, 0.2f));
        }
        else if (Input.GetKey(KeyCode.F))
        {
            var a = GetClosestDoor(AllDoors);
            a.DORotate(new Vector3(0, kapi_kac_derece_acilsin,0), 1);
        }
    }
    
    Transform GetClosestDoor (Transform[] doors)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in doors)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }
    
    
}