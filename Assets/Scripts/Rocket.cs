using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;


    [SerializeField] ParticleSystem mainEngineParticles;


    Rigidbody rigidBody;


    bool isTransitioning = false;
    bool collisionsDisabled = false;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled; // toggle
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsDisabled) { return; }

        switch (collision.gameObject.tag)
        {
            case "Coin":
                Destroy(collision.gameObject);
                break;
            case "Friendly":
                break;
            case "Finish":
                  GameObject.Find("CoinsText").GetComponent<Text>().text= "You Won ";
                StartSuccessSequence();
                break;
            default:
                GameObject.Find("CoinsText").GetComponent<Text>().text = "You Crashed";
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; // loop back to start
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {

        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(rcsThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-rcsThrust * Time.deltaTime);
        }
    }

    private void RotateManually(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidBody.freezeRotation = false; // resume physics control of rotation
    }
}