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

    private bool Move;
    private bool stopMovement;
    private bool jump;

    private Vector2 characterVelocity;
    private Rigidbody2D rb;

    void Start () {
        _speed = speed;
        _jumpSpeed = jumpspeed;

       rb = this.gameObject.GetComponent<Rigidbody2D>();
        Debug.Assert(rb != null, "rigid body is not found!");

    }

    void Update () {
        float leftRight, upDown;

        // check for input here 
       leftRight = Input.GetAxis("Horizontal")  * _speed ;
        jump = Input.GetKeyDown(KeyCode.UpArrow);

        if (Input.GetKeyDown ( KeyCode.X))
        {
            stopMovement = true;
            return;
        }
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // activate ability
            Debug.Log("Use ability ");
            // show some cool animation
            Vector2 point = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] targets = Physics2D.OverlapCircleAll(point , blastRadius);
            Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(blastRadius, 0, 0));
            if ( targets.Length > 0)
            {
               foreach ( Collider2D target in targets)
                {
                    Rigidbody2D trb = target.GetComponent<Rigidbody2D>();
                    if (trb == null || target.name == "player" || target.name == "Player")
                        continue;
                    Vector3 direction = this.transform.position + target.transform.position * blastPower;
                    Vector2 direction2D = new Vector2(direction.x, direction.y);
                    Debug.Log("Target Name: " + target.name + " direction target: " + direction2D);
                    trb.AddForce( direction2D , ForceMode2D.Impulse);
                    health h = target.GetComponent<health>();
                    if (h == null)
                        return;
                    h.loseHealth(-10);
                }
            }
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

        if (jump)
        {
            // add jump movement 
            // Debug.Log("Y Velocity: " + rb.velocity.y);
            rb.AddForce((new Vector2 ( 0 , _jumpSpeed)  * PhysicsMultiplier), ForceMode2D.Impulse);
        }

        if ( Move == true)
        {
            // Do movement!
            
            // grab the rigidbody 2d  , so we can use the physics system
            if (rb == null)
                Debug.LogError("Can't find a rigidbody 2D on the player!");

            if (stopMovement)
            {

                rb.velocity = Vector2.zero;
                stopMovement = false;
                return;

            }

            // add left and right movement 
            float newX = characterVelocity.x + (-(rb.velocity.x));
            Vector2 newleftright = new Vector2(newX, 0) * 10;
           // Debug.Log(newleftright + " current velocity X: " + rb.velocity.x );
            rb.AddForce( newleftright , ForceMode2D.Force);
          
            // after we are done giving our character 
            // a driving force, set this to false
            // to prevent giving it a constant force every frame 
            Move = false;
        }

        rb.velocity = new Vector2(Mathf.Clamp (rb.velocity.x, -5, 5 ) , Mathf.Clamp(rb.velocity.y, -4, 4));
    }
}
