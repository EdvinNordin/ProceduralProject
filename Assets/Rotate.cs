using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10f;
   
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.World);
        //rb.AddForce(Physics.gravity*rb.mass);
        transform.RotateAround(Vector3.zero, Vector3.up, 2f * speed * Time.deltaTime);
        /*if(transform.position.y>0){
            transform.RotateAround(Vector3.zero, Vector3.right, transform.position.y * Time.deltaTime);
        }else{
            transform.RotateAround(Vector3.zero, -Vector3.right, transform.position.y * Time.deltaTime);
        }*/

    }
}
