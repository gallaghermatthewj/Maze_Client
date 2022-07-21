using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraBehaviorManager : MonoBehaviour
{
    public GameObject targetObject;
    
    public GameObject pointCounterText;
    public GameObject userNameOverlayText;
    public GameObject localPlayer;

    bool isBoosted = false;

    public AudioSource electricAudioSource;
    public AudioSource playerAudioSource; 
    public AudioClip sonarSound;
    public AudioClip cheeseSound;
    public AudioClip pillSound;
    public AudioClip buttonSound;
    public AudioClip electricSound;

    public Animator animator;

    public Vector3 offset;
    public float boost = 1.0f;
    public bool rotateWithTarget;
    Quaternion startRot;
    public bool lockY;
    public int score = 0;
    private float yLock;
    private IEnumerator coroutine;
    float lockPos = 0;
    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation;
        userNameOverlayText.GetComponent<Text>().text = "Test Subject: " +  localPlayer.GetComponent<PlayerManager>().username;
        offset = transform.position - targetObject.transform.position;
        yLock = transform.position.y;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void boostOffset(PlayerManager _player)
    {
        if (!isBoosted)
        {
            isBoosted = true;
            coroutine = boostPosition(_player.camera, 2.0f, 2.0f, 3.0f);
            StartCoroutine(coroutine);
        }
        
    }

    public IEnumerator boostPosition(GameObject objectToBoost, float timeToMove, float boostPotency, float boostDuration)
    {
        animator.SetTrigger("IsLooking");
        playerAudioSource.clip = sonarSound;
        playerAudioSource.Play();

        //GameObject.Find("cheese").transform.Find("HeightenedSenses").GetComponent<ParticleSystem>().enableEmission = true;
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
        animator.ResetTrigger("IsLooking");
        playerAudioSource.clip = sonarSound;
        playerAudioSource.Play();
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            objectToBoost.GetComponent<CameraBehaviorManager>().boost = Mathf.Lerp(boostPotency, currentBoost, t);
            yield return null;
        }
        isBoosted = false;
        //GameObject.Find("cheese").transform.Find("HeightenedSenses").GetComponent<ParticleSystem>().enableEmission = false;


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
        //pillChoice = 1;
        Debug.LogError(pillChoice);
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
                //Whoa!
                int trippyIndex = Random.Range(0, 8);
                if (trippyIndex == 5) { trippyIndex = 11; }
                _player.camera.GetComponent<SnapshotMode>().filterIndex = trippyIndex;
                Debug.Log("Taking a Pill " + _player.camera.GetComponent<SnapshotMode>().filterIndex);
                break;
            case 3:
                IEnumerator coroutine = rotateCameraTemporarily(_player.camera, 1.0f);
                StartCoroutine(coroutine);
                //boostSpeed(0.5f);
                break;
            case 4:
                //Ghost Mouse!
                //Mouse Looks spectral/transparent. Walks through walls, but watch out for edge walls!
                break;
            case 5:
                //Pidgeon Rat?
                //Rat grows wings all of a sudden that can be used to flap up for 4 seconds
                break;
            case 6:
                //LaserEyes
                //Can shoot other mice, or walls!
                break;
            case 7:
                //FlatPerspective
                //Camera switches betwen perspective and orthographic
                break;
            case 8:
                //Satellite View
                //Super zoomed out view
                break;
            case 9:
                //I AM SPEED
                //Double speed
                break;
            case 10:
                //Become Tiny
                //Become super small
                break;
            case 11:
                //Become HUGE
                //become big enough to block path, but slow
                break;
            case 12:
                //1st Person Perspective
                //Switch to 1st-person camera
                break;
            case 13:
                //I am become death
                //Any mice that comes near stinky mouse starts to die
                break;
            case 14:
                //Health Restored
                //Yay
                break;
            case 15:
                //Poison
                //Health reduced. Bummer.
                break;
            case 16:
                //Slippery
                //Friction reduced on server-side. Careful about messaging
                break;
            case 17:
                //Invisible Walls
                //Walls stop rendering briefly.
                break;
            case 18:
                //Bad Trip
                //Overlay of weird trippy stuff
                break;
            case 19:
                //Love!
                //Other players look like cheese
                break;
            case 20:
                //Teleportation?
                //Randomly appear somewhere else every 5-10 seconds for 1 minute
                break;
            case 21:
                //Anti-Gravity?
                //Mouse floats up and spins
                break;
            case 22:
                //Telekinesis
                //Other Able to control other mice or objects
                break;
            case 23:
                //Extreme Empathetic Experience
                //Switch places with Cheese. Run away from other mice for 5 seconds and become mouse again.
                break;
            case 24:
                //HyperIntelligence
                //Path To Cheese Revealed
                break;
            case 25:
                //Pyrokinesis
                //Able to set fire to areas
                break;
            case 26:
                //Zombie mouse!
                //Become zombie mouse! Infect other mice. 
                break;
            case 27:
                //Walls Gone
                //All Walls except perimeter vanish
                break;
            case 28:
                //Eldritch Blast
                //
                break;
            case 29:
                //Endtimes
                //Evil Mouse grows larger than building, larger than town, begins rampaging across the earth, bringing destruction, other players become tanks trying to shoot lasers at giant mouse, but not get trampled
                break;
            case 30:
                //Out of ideas
                //Change colors?
                break;
            default:
                break;
        }
    }

    private IEnumerator rotateCameraTemporarily(GameObject camera, float timeToMove)
    {
        Quaternion currentRotation = camera.transform.rotation;
        float rotationDirection = Random.Range(0, 3);
        float moveSpeed = 1.0f;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            camera.transform.rotation = Quaternion.Lerp(currentRotation, Quaternion.Euler(new Vector3(90,90,-90+(90*rotationDirection))), t * moveSpeed);
            yield return null;
        }
        IEnumerator endCoroutine = rotateCameraToNormal(camera, 1.0f);
        coroutine = waitForEffectDuration(5.0f, endCoroutine);
        StartCoroutine(coroutine);
    }

    private IEnumerator rotateCameraToNormal(GameObject camera, float timeToMove)
    {
        Quaternion currentRotation = camera.transform.rotation;
        float moveSpeed = 1.0f;
        var t = 0f;        
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            camera.transform.rotation = Quaternion.Lerp(currentRotation, startRot, t * moveSpeed);
            yield return null;
        }
    }

    public void eatCheese(PlayerManager _player)
    {
        animator.ResetTrigger("EatsSomething");
        animator.SetTrigger("EatsSomething");
        playerAudioSource.clip = cheeseSound;
        playerAudioSource.Play();
        _player.score += 100;
        pointCounterText.GetComponent<Text>().text = "Science Points: " + _player.score.ToString();
    }

    public void electrify()
    {
        electricAudioSource.clip = electricSound;
        electricAudioSource.Play();
        foreach (GameObject gob in GameObject.FindGameObjectsWithTag("ElectrifiedWalls"))
        {
            gob.GetComponent<LineRenderer>().enabled = gob.GetComponentInParent<MeshRenderer>().enabled;
        }
        IEnumerator coroutine = makeWallsElectric();
        StartCoroutine(coroutine);
    }

    private IEnumerator makeWallsElectric()
    {
        yield return new WaitForSeconds(10f);
        electricAudioSource.Stop();
        
        foreach (GameObject gob in GameObject.FindGameObjectsWithTag("ElectrifiedWalls"))
        {
            gob.GetComponent<LineRenderer>().enabled = false;
        }
    }

    public void stepOnButton()
    {

            playerAudioSource.clip = buttonSound;
            playerAudioSource.Play();
            ClientSend.TriggerMazeRedraw(Random.Range(0,3));
            //ClientSend.TriggerMazeRedraw(1);
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
