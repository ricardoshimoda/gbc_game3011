using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float angularSpeed = 10.0f;
    public float radius = 1;
    bool clockwise = true;
    float currentAngle = 270;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        radius = Mathf.Abs(player.transform.localPosition.y);
        currentAngle = currentAngle * Mathf.PI / 180;
    }

    // Update is called once per frame
    void Update()
    {
        currentAngle += clockwise ? -angularSpeed * Time.deltaTime : angularSpeed * Time.deltaTime;
        player.transform.localPosition = new Vector3(radius * Mathf.Cos(currentAngle), radius * Mathf.Sin(currentAngle), 0);
    }
}
