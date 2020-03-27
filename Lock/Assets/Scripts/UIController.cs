using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    private static UIController _instance;
    public static UIController Instance { get  { return _instance; } }
    public TMP_Text txtScore;
    public TMP_Text txtGoal;
    public TMP_Text txtTimer;
    public TMP_Text txtLevel;
    public void UpdateScore(int score) {
        txtScore.text = "Score: " + score;
    }
    public void UpdateGoal(int goal) {
        txtGoal.text = "Goal: " + goal;
    }
    public void UpdateTimer(int timer) {
        txtTimer.text = "Time: " + timer;
    }
    public void UpdateLevel(int level){
        txtLevel.text = "Level " + level;
    }
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
        
    }
}
