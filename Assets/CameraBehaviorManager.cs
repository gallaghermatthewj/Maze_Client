using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraBehaviorManager : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject heightenedSenses;
    public GameObject pointCounterText;
    public GameObject userNameOverlayText;
    public GameObject localPlayer;
    


    public AudioSource playerAudioSource; 
    public AudioClip sonarSound;
    public AudioClip cheeseSound;
    public AudioClip pillSound;
    public AudioClip buttonSound;

    public Vector3 offset;
    public float boost = 1.0f;
    public bool rotateWithTarget;
    public bool lockY;
    public int score = 0;
    private float yLock;
    private IEnumerator coroutine;
    float lockPos = 0;
    // Start is called before the first frame update
    void Start()
    {
        userNameOverlayText.GetComponent<Text>().text = "Test Subject: " +  localPlayer.GetComponent<PlayerManager>().username;
        offset = transform.position - targetObject.transform.position;
        yLock = transform.position.y;
        heightenedSenses.GetComponent<ParticleSystem>().Play();
        heightenedSenses.GetComponent<ParticleSystem>().enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void boostOffset(PlayerManager _player)
    {
        coroutine = boostPosition(_player.camera, 2.0f, 2.0f, 3.0f);
        StartCoroutine(coroutine);
    }

    public IEnumerator boostPosition(GameObject objectToBoost, float timeToMove, float boostPotency, float boostDuration)
    {
        playerAudioSource.clip = sonarSound;
        playerAudioSource.Play();
        objectToBoost.transform.parent.Find("HeightenedSenses").GetComponent<ParticleSystem>().enableEmission = true;
        var t = 0f;
        //float currentBoost = objectToBoost.GetComponent<CameraBehaviorManager>().boost;
        float currentBoost = 1.0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            objectToBoost.GetComponent<CameraBehaviorManager>().boost = Mathf.Lerp(currentBoost, boostPotency, t);
            yield return null;
        }

        IEnumerator endCoroutine = withdrawalFromBoost(objectToBoost, timeToMove, boostPotency, currentBoost);
        coroutine = waitForEffectDuration(boostDuration, endCoroutine);
        StartCoroutine(coroutine);

    }
    public IEnumerator withdrawalFromBoost(GameObject objectToBoost, float timeToMove, float boostPotency, float currentBoost)
    {
        playerAudioSource.clip = sonarSound;
        playerAudioSource.Play();
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            objectToBoost.GetComponent<CameraBehaviorManager>().boost = Mathf.Lerp(boostPotency, currentBoost, t);
            yield return null;
        }
        objectToBoost.transform.parent.Find("HeightenedSenses").GetComponent<ParticleSystem>().enableEmission = false;
        
        
    }

    public void boostSpeed(float speedFactor)
    {
        coroutine = speedUp(targetObject.GetComponent<LegacyPlayerController>().speed * speedFactor, 2.0f, 5.0f);
        StartCoroutine(coroutine);
    }




    public IEnumerator speedUp(float speedPotency, float timeToMove, float speedDuration)
    {
        float currentSpeed = targetObject.GetComponent<LegacyPlayerController>().speed;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            targetObject.GetComponent<LegacyPlayerController>().speed = Mathf.Lerp(currentSpeed, speedPotency, t);
            yield return null;
        }
        IEnumerator endCoroutine = slowDown(timeToMove, speedPotency, speedDuration, currentSpeed);
        coroutine = waitForEffectDuration(speedDuration, endCoroutine);
        StartCoroutine(coroutine);
    }



    public IEnumerator slowDown(float timeToMove, float speedPotency, float speedDuration, float currentSpeed)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            targetObject.GetComponent<LegacyPlayerController>().speed = Mathf.Lerp(speedPotency, currentSpeed, t);
            yield return null;
        }
        GameObject.Find("StartingPoint").GetComponent<CreateMaze>().placePill();
    }


    public IEnumerator waitForEffectDuration(float effectDuration, IEnumerator withdrawFromEffect)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / effectDuration;
            yield return null;
        }
        
        StartCoroutine(withdrawFromEffect);
    }


    public void takePill(PlayerManager _player)
    {
        playerAudioSource.clip = pillSound;
        playerAudioSource.Play();
        _player.score += 20;
        pointCounterText.GetComponent<Text>().text = "Science Points: " + _player.score.ToString();
        int pillChoice = Random.Range(0, 3);
        if(pillChoice==1 && boost > 1.0f)
        {
            pillChoice = 0;
        }
        float effectDuration = Random.Range(3.0f, 10.0f);
        switch (pillChoice)
        {
            case 0:
                //placebo
                break;
            case 1:
                //See More Maze
                boostOffset(_player);
                break;
            case 2:
                
                int trippyIndex = Random.Range(0, 8);
                if (trippyIndex == 5) { trippyIndex = 11; }
                _player.camera.GetComponent<SnapshotMode>().filterIndex = trippyIndex;
                Debug.Log("Taking a Pill " + _player.camera.GetComponent<SnapshotMode>().filterIndex);
                break;
            case 3:
                //Speed Down
                //boostSpeed(0.5f);
                break;
            case 4:
                //Speed Up
                //boostSpeed(2.0f);
                break;
            default:
                break;
        }
    }

    public void eatCheese(PlayerManager _player)
    {
        playerAudioSource.clip = cheeseSound;
        playerAudioSource.Play();
        _player.score += 100;
        pointCounterText.GetComponent<Text>().text = "Science Points: " + _player.score.ToString();
    }

    public void stepOnButton()
    {
        playerAudioSource.clip = buttonSound;
        playerAudioSource.Play();
        ClientSend.TriggerMazeRedraw();
    }

    void LateUpdate()
        {
            if (targetObject != null)
            {
                if (transform.tag == "MainCamera")
                {
                    transform.position = targetObject.transform.position + (offset * boost);
                }
                else
                {
                    transform.position = targetObject.transform.position + (offset);
                }
            }
        }
    }
