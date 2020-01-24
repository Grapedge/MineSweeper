using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float moveSpeed = 30f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            float x = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
            float y = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
            transform.Translate(new Vector2(-x, -y));
        }
    }
}
