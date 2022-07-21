using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    public GameObject userNameIndicatorText;
    public int id;
    public string username;
    public float health;
    public float maxHealth;
    public MeshRenderer DamageRenderer;
    public SkinnedMeshRenderer model;
    public MeshRenderer InsulatedModel;
    public GameObject camera;
    private bool isInsulated;
    public int itemCount = 0;
    public float score = 0;
    public AudioClip powerupSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioSource playerAudioSource;
    public Animator animator;
    public bool isLocalPlayer;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
        userNameIndicatorText.GetComponent<TextMesh>().text = username;
    }
    public void Update()
    {
        if(transform.rotation!= new Quaternion(0f,0f,0f,0f))
        {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    public void SetHealth(float _health)
    {
        health = _health;
        IEnumerator coroutine = tookSomeDamage();
        StartCoroutine(coroutine);
        if(health <= 0f)
        {
            Die();
        }
    }
    private IEnumerator tookSomeDamage()
    {
        animator.ResetTrigger("GetsZapped");
        animator.SetTrigger("GetsZapped");
        if (isLocalPlayer)
        {
            playerAudioSource.clip = damageSound;
            playerAudioSource.Play();
        }
        DamageRenderer.enabled = true;
        Debug.Log("Took Some Damage!");
        //showDamageMaterial
        float t = 0f;
        float timeToFlash = 1f;
        float r = DamageRenderer.material.color.r;
        float g = DamageRenderer.material.color.g;
        float b = DamageRenderer.material.color.b;

        while (t < 1)
        {
            t += Time.deltaTime / timeToFlash;
            
            DamageRenderer.material.color = new Color(r,g,b,Mathf.Lerp(0.0f, 255.0f, t));
            yield return null;
        }
        IEnumerator stopDamage = stopTakingDamage();
        StartCoroutine(stopDamage);
    }
    private IEnumerator stopTakingDamage()
    {
        animator.ResetTrigger("GetsZapped");
        //showDamageMaterial
        float t = 0f;
        float timeToFlash = 1f;
        float r = DamageRenderer.material.color.r;
        float g = DamageRenderer.material.color.g;
        float b = DamageRenderer.material.color.b;

        while (t < 1)
        {
            t += Time.deltaTime / timeToFlash;

            DamageRenderer.material.color = new Color(r, g, b, Mathf.Lerp(255.0f, 0.0f, t));
            yield return null;
        }
        DamageRenderer.enabled = false;
    }

    public void Die()
    {
        if (isLocalPlayer)
        {
            score = score - 500;
            PlayerController pc = transform.GetComponent<PlayerController>();
            pc.isDead = true;
        }
        
        playerAudioSource.clip = deathSound;
        playerAudioSource.Play();
        model.enabled = false;

    }
    public void Respawn()
    {
        if (isLocalPlayer)
        {
            PlayerController pc = transform.GetComponent<PlayerController>();
            pc.isDead = false;
        }
        model.enabled = true;
        SetHealth(maxHealth);
    }
    public void gainTemporaryInsulation()
    {
        if (isLocalPlayer)
        {
            playerAudioSource.clip = powerupSound;
            playerAudioSource.Play();
        }
        isInsulated = true;
        InsulatedModel.enabled = true;
        IEnumerator tempInsulation = temporaryInsulation();
        StartCoroutine(tempInsulation);
    }
    private IEnumerator temporaryInsulation()
    {
        yield return new WaitForSeconds(5f);
        isInsulated = false;
        InsulatedModel.enabled = false;
    }
}