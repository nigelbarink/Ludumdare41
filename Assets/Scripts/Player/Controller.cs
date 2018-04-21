using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Controller : MonoBehaviour {
    // Note: these value should not be used in the script except for
    // initialization of the property 
    public float speed;
    public float jumpspeed;
    public float blastRadius;
    public float blastPower;
    // These values are used within the script

    // the physicsMultiplier is used to make the physics more or less extreme on
    // a larger scale while the speed determines the smaller scale 
    private const float PhysicsMultiplier = 100f;

    private float _speed { get; set; }
    private float _jumpSpeed { get; set; }

    private bool Move = false;
    private bool stopMovement = false;
    private bool jump = false;
    private bool useAbility = false;

    private Vector2 characterVelocity;
    private Rigidbody2D rb;
    private gameManager gm;

    void Start () {
        _speed = speed;
        _jumpSpeed = jumpspeed;

       rb = this.gameObject.GetComponent<Rigidbody2D>();
       gm = GameObject.Find("Manager").GetComponent<gameManager>();

        Debug.Assert(rb != null, "rigid body is not found!");
        Debug.Assert(gm != null, "game manager could not be found");
    }

    void Update () {
        float leftRight, upDown;

        // check for input here 
       leftRight = Input.GetAxis("Horizontal")  * _speed ;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
        }

        if (Input.GetKeyDown ( KeyCode.X))
        {
            stopMovement = true;
            return;
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && gm.isAbilityReady())
        {    
            // activate ability
            Debug.Log("Use ability ");
            useAbility = true;
        }


        if (leftRight == 0 )
            return;
        else
        {
           // Debug.Log("Movement: " + leftRight + " : " + upDown);

            characterVelocity = new Vector2(leftRight, 0);
            

            Move = true;
        }
    }

    private void FixedUpdate()
    {
        // do the actual movement here 
        // as we would like to make as much use
        // of unity's physics system as possible 

        if ( useAbility == true)
        {
            Vector2 point = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] targets = Physics2D.OverlapCircleAll(point, blastRadius);
            Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(blastRadius, 0, 0));
            if (targets.Length > 0)
            {
                foreach (Collider2D target in targets)
                {
                    Rigidbody2D trb = target.GetComponent<Rigidbody2D>();
                    if (trb == null || target.name == "player" || target.name == "Player")
                        continue;
                    Vector3 direction = this.transform.position + target.transform.position ;
                    Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
                    Debug.Log("Target Name: " + target.name + " direction target: " + direction2D);
                    trb.AddForce(direction2D * blastPower, ForceMode2D.Impulse);
                    health h = target.GetComponent<health>();
                    if (h == null)
                        return;
                    h.loseHealth(10);
                }

            }

            // show some cool animation

            gm.resetAbilityTimer();
            useAbility = false;

        }


        if (jump)
        {
            // add jump movement 
            // Debug.Log("Y Velocity: " + rb.velocity.y);
            rb.AddForce((new Vector2 ( 0 , _jumpSpeed)  * PhysicsMultiplier), ForceMode2D.Impulse);
            jump = false;
        }

        if (stopMovement)
        {

            rb.velocity = Vector2.zero;
            stopMovement = false;
            return;

        }

        if ( Move == true)
        {   
            // grab the rigidbody 2d  , so we can use the physics system
            if (rb == null)
                Debug.LogError("Can't find a rigidbody 2D on the player!");
            
            // add left and right movement 
            float newX = characterVelocity.x + (-(rb.velocity.x));
            Vector2 newleftright = new Vector2(newX, 0) * 2;
           // Debug.Log(newleftright + " current velocity X: " + rb.velocity.x );
            rb.AddForce( newleftright  , ForceMode2D.Impulse);
          
            // after we are done giving our character 
            // a driving force, set this to false
            // to prevent giving it a constant force every frame 
            Move = false;
        }

        rb.velocity = new Vector2(Mathf.Clamp (rb.velocity.x, -2, 2 ) , Mathf.Clamp(rb.velocity.y, -3, 3));
    }
}
