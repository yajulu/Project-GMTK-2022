using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallexEffect : MonoBehaviour
{
    private float length, currentPos, endPos, startingPos;

    public float speed, parallexEffect;
    public Transform cameraPos;


    void Start()
    {

        currentPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        endPos = cameraPos.position.x - (length);


    }

    // Update is called once per frame
    void Update()
    {

        if (parallexEffect != 0)
        {
            transform.Translate(Vector3.left * (speed * parallexEffect * Time.deltaTime));
            if (transform.position.x <= endPos)
            {
                // transform.position = new Vector3(endPos, transform.position.y, transform.position.z);
                transform.position = new Vector3(endPos + 90, transform.position.y, transform.position.z);
            }

        }




    }
}
