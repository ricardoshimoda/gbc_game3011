using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance { get  { return _instance; } }
    private int score = 0;
    public int[] timers;
    public int[] goals;
    public float[] validDistance;
    public float[] validSpawn;
    public int level = 0;
    private float secondTimer = 0;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        secondTimer += Time.deltaTime;
        if(secondTimer > 1){
            timers[level]--;
            UIController.Instance.UpdateTimer(timers[level]);
            secondTimer--;
        }
    }

    public void Score(){
        score++;
        UIController.Instance.UpdateScore(score);
    }
    public void Fail(){
        score = 0;
        UIController.Instance.UpdateScore(score);
    }
    public void Setup(){

    }
    public void GameEnd(){

    }


}
