using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    const int SOUND_FX = 0;
    const int BG_CLOCK = 1;
    private static GameController _instance;
    public static GameController Instance { get  { return _instance; } }
    private int score = 0;
    public GameObject lockPrefab;
    public int[] timers;
    public int[] goals;
    public float[] validDistance;
    public float[] validSpawn;
    public Transform[] lockSpawnPoints;
    public AudioClip[] pointFx;
    public AudioClip clock;
    public AudioClip error;
    public AudioClip alarm;
    public AudioClip newLevel;
    public AudioClip win;
    public AudioClip lose;
    private AudioSource[] audioList;
    private List<GameObject> lockList = new List<GameObject>();
    public int level = 0;
    private float secondTimer = 0;
    private bool gameOn = true;
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
        audioList = GetComponents<AudioSource>();
        audioList[SOUND_FX].loop = false;
        audioList[SOUND_FX].volume = 0.05f;

        audioList[BG_CLOCK].loop = true;
        audioList[BG_CLOCK].clip = clock;
        audioList[BG_CLOCK].volume = 0.2f;
        audioList[BG_CLOCK].Play();

        Setup();
    }
    // Update is called once per frame
    void Update()
    {
        if(!gameOn) return;

        secondTimer += Time.deltaTime;
        if(secondTimer > 1){
            timers[level]--;
            UIController.Instance.UpdateTimer(timers[level]);
            secondTimer--;
            if(timers[level] == 0){
                GameEnd(false);
            }
        }
    }

    public void Score(){
        score++;
        audioList[SOUND_FX].clip = pointFx[(score-1)%pointFx.Length];
        audioList[SOUND_FX].Play();
        UIController.Instance.UpdateScore(score);
        if(score == goals[level]){
            // Levels Up
            level++;
            if(level == timers.Length){
                GameEnd(true);
            }
            else{
                StartCoroutine("LevelTransition");
            }
        }
    }
    public void Fail(bool zeroScore = false){
        if (zeroScore){
            score = 0;
            audioList[SOUND_FX].clip = error;
            audioList[SOUND_FX].Play();
            UIController.Instance.UpdateScore(score);
        }
    }
    public IEnumerator LevelTransition(){
        score = 0;
        UIController.Instance.UpdateScore(score);
        audioList[BG_CLOCK].Stop();
        audioList[SOUND_FX].clip = newLevel;
        audioList[SOUND_FX].Play();
        foreach (var levelLock in lockList){
            Destroy(levelLock);
        }
        lockList.Clear();
        yield return new WaitForSeconds (2.5f);
        Setup();
    }
    public void Setup(){
        UIController.Instance.UpdateGoal(goals[level]);
        UIController.Instance.UpdateLevel(level + 1);
        if(level == 0){
            var lock0 = Instantiate(lockPrefab, lockSpawnPoints[0].position, lockSpawnPoints[0].rotation);
            lock0.GetComponent<BallMovement>().control = 1;
            lockList.Add(lock0);                        
        } else if (level == 1) {
            var lock0 = Instantiate(lockPrefab, lockSpawnPoints[1].position, lockSpawnPoints[1].rotation);
            lock0.GetComponent<BallMovement>().control = 1;
            lockList.Add(lock0);

            var lock1 = Instantiate(lockPrefab, lockSpawnPoints[2].position, lockSpawnPoints[2].rotation);
            lock1.GetComponent<BallMovement>().control = 2;
            lockList.Add(lock1);                        
        } else if (level == 2) {

            var lock0 = Instantiate(lockPrefab, lockSpawnPoints[1].position, lockSpawnPoints[1].rotation);
            lock0.GetComponent<BallMovement>().control = 1;
            lockList.Add(lock0);

            var lock1 = Instantiate(lockPrefab, lockSpawnPoints[2].position, lockSpawnPoints[2].rotation);
            lock1.GetComponent<BallMovement>().control = 2;
            lockList.Add(lock1);

            var lock2 = Instantiate(lockPrefab, lockSpawnPoints[0].position, lockSpawnPoints[0].rotation);
            lock2.GetComponent<BallMovement>().control = 3;
            lockList.Add(lock2);                        
        }
        audioList[BG_CLOCK].Play();

    }
    public void GameEnd(bool victory){
        audioList[BG_CLOCK].Stop();
        if(victory){
            audioList[SOUND_FX].clip = win;
            audioList[SOUND_FX].Play();
        }
        else{
            audioList[SOUND_FX].clip = lose;
            audioList[SOUND_FX].Play();
        }
        gameOn = false;
        foreach (var levelLock in lockList){
            Destroy(levelLock);
        }
        lockList.Clear();
    }


}
