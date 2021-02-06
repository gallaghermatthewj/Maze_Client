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
    public MeshRenderer model;
    public GameObject camera;

    public int itemCount = 0;
    public float score = 0;
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

        if(health <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        model.enabled = false;

    }
    public void Respawn()
    {
        model.enabled = true;
        SetHealth(maxHealth);
    }

}