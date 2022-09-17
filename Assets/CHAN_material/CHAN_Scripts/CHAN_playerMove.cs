using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAN_playerMove : MonoBehaviour
{
    Vector3 dir;

    [SerializeField] float speed=5;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        dir = (h * Vector3.right + v * Vector3.forward).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }
}
