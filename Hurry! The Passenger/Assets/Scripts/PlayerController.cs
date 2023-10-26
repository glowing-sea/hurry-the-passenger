using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The script only used to control player's movement
// Other information about the game mechanics are not included
public class PlayerController : MonoBehaviour
{
    // Control
    private Vector2 movement; // ref to keyboard inputs
    private float mouseX; // ref to the mouse inputs
    private Rigidbody playerRb;
    public float speed; // speed of the player
    public float maxStamina; // determine how long the player can run
    private float stamina; // current stamina

    public bool energyDrink = false; // if the player has energy drink
    public float jumpForce; // force apply to the player when they jump
    public float gravityModifier; // set to 9.8

    // Animation
    private Animator playerAnim;

    // Particles
    public ParticleSystem dirtParticle;

    // Audio
    public AudioClip jumpSound;
    public AudioSource soundEffect;

    // UI
    public Slider staminaGauge; // to display stamina

    // Script
    private GameManager gameManager; // reference to the game manager script


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        playerAnim = GetComponent<Animator>(); // initialise animation
        playerAnim.SetBool("Static_b", true);
        playerRb = GetComponent<Rigidbody>(); // initialise rigid body
        soundEffect = GetComponent<AudioSource>(); // get audio playing reference
        stamina = maxStamina; // initialise stamina
        staminaGauge.gameObject.SetActive(false);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // get reference
    }


    // Update is called once per frame
    void Update()
    {
        // If the game is not over or pause, make player movement active
        if (gameManager.gameState == GameState.Running)

        {
            PlayerMovement();

            // Die if player is below the ground
            if (transform.position.y < -40)
            {
                gameManager.GameOver();
            }
        }
    }

    // Move the player according to the keyboard and mouse inputs
    private void PlayerMovement()
    {
        // Get keyboard inputs for movement
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // Adjusted speed
        float newSpeed = speed;

        // Analyse player inputs
        bool isMoving = movement.x != 0 || movement.y != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Test if the player is on the ground (within 0.01 tolerance)
        bool isOnGround = Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, 0.02f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        // Set up walking or idel animation
        if (isMoving)
        {
            // Let player run
            if (isRunning && stamina > 0)
            {
                if (energyDrink)
                {
                    newSpeed *= 5;
                    Debug.Log("energy drink fueled ultra run");
                }
                else
                {
                    newSpeed *= 2;
                }
                playerAnim.SetFloat("Speed_f", 0.51f);
                stamina -= Time.deltaTime * 20;
                staminaGauge.gameObject.SetActive(true);
                if (isOnGround)
                {
                    dirtParticle.Play();
                }
            }
            // Let player walk
            else
            {
                playerAnim.SetFloat("Speed_f", 0.4f);
                dirtParticle.Stop();
                if (stamina < 100)
                {
                    stamina += Time.deltaTime * 20;
                }
                else
                {
                    staminaGauge.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // Let player stay
            playerAnim.SetFloat("Speed_f", 0f);
            dirtParticle.Stop();
            if (stamina < 100)
            {
                stamina += Time.deltaTime * 40;
            }
            else
            {
                staminaGauge.gameObject.SetActive(false);
            }
        }

        // Update the stamina gauge
        staminaGauge.value = stamina;

        // Apply walk/run/stap
        playerRb.AddRelativeForce(Vector3.forward * newSpeed * 10000 * movement.y * Time.deltaTime);
        playerRb.AddRelativeForce(Vector3.right * newSpeed * 10000 * movement.x * Time.deltaTime);


        // Rotate the character based on the mouse inpupt
        mouseX = Input.GetAxis("Mouse X"); // seems to be in pixels, documentation not clear
        if (mouseX != 0)
        {
            transform.Rotate(Vector3.up, mouseX * GameSettings.instance.sensitivity);
        }


        // Make the player jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // apply force to the rigid body reference
            playerAnim.SetTrigger("Jump_trig"); // play jump animation
            dirtParticle.Stop();
            soundEffect.PlayOneShot(jumpSound, 1.0f); // play jump sound
        }
    }

    public void DrinkEnergyDrink()
    {
        energyDrink = true;
        Debug.Log("Player has energy drink");
    }

}