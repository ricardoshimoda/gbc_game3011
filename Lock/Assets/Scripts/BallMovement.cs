using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [Range(0.5f, 5)]
    public float angularSpeed = 1.0f;
    public float radius = 1;
    public GameObject player;
    public int control;
    private float currentAngle = 270;
    private float minAngle = 0;
    private float maxAngle = 90;
    private bool clockwise = true;
    private bool passedZero = true;
    private SpriteRenderer rd;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<SpriteRenderer>();
        radius = Mathf.Abs(player.transform.localPosition.y);

        /* Transforming angle in radians - easier to calculate trajectory */
        currentAngle = currentAngle * Mathf.PI / 180;
        SetNewTarget(true);

    }

    public void SetNewTarget(bool first = false){
        float vSpwn = GameController.Instance.validSpawn[GameController.Instance.level];
        float vDist = GameController.Instance.validDistance[GameController.Instance.level];
        float eulerAngle = (currentAngle * 180 / Mathf.PI) % 360;

        /*
         * Always generate minimum angle between 0 and 360 - distance between max and min
         */
        minAngle = (eulerAngle + vSpwn + Random.Range(0,25) + (first? 25 * control:0)) % 360 ;
        if(minAngle + vDist > 360){
            minAngle -= vDist;
        }
        maxAngle = minAngle + vDist;

        rd.material.SetFloat("_ArcStart", minAngle);
        rd.material.SetFloat("_ArcEnd", maxAngle);

        if((eulerAngle < minAngle) && clockwise){
            passedZero = false;
        }
        if((eulerAngle > maxAngle) && !clockwise){
            passedZero = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float finalSpeed = clockwise ? -angularSpeed * Time.deltaTime : angularSpeed * Time.deltaTime;
        currentAngle += finalSpeed;

        /*
         * Angle is always between 0 and 2PI
         */
        if(clockwise){
            while(currentAngle < 0){
                currentAngle += 2 * Mathf.PI;
                passedZero = true;
            }
        }
        else {
            while(currentAngle > 2 * Mathf.PI){
                passedZero = true;
                currentAngle -= 2 * Mathf.PI;
            }
        }

        player.transform.localPosition = new Vector3(radius * Mathf.Cos(currentAngle), radius * Mathf.Sin(currentAngle), 0);
        float eulerAngle = (currentAngle * 180 / Mathf.PI) % 360;

        // Senses whether the target has escaped        
        if(clockwise && passedZero && eulerAngle < minAngle){
            Debug.Log("Fail by inactivity - Clockwise: ");
            GameController.Instance.Fail();
            SetNewTarget();
        }
        if(!clockwise && passedZero && eulerAngle > maxAngle){
            Debug.Log("Fail by inactivity - CounterClockwise");
            GameController.Instance.Fail();
            SetNewTarget();
        }
        if(Input.GetButtonDown("Fire" + control)){
            Debug.Log(minAngle + " < " + eulerAngle + " < " + maxAngle);
            // Changes direction before verifying scoring requirements
            clockwise = !clockwise;
            // Verifies if the currentAngle
            if(eulerAngle > minAngle && eulerAngle < maxAngle){
                GameController.Instance.Score();
                SetNewTarget();
            }
            else{
                GameController.Instance.Fail(true);
                SetNewTarget();
            }
        }
    }
}
