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

    private bool _Move = false;
    private bool _stopMovement = false;
    private bool _jump = false;
    private bool _useAbility = false;

    private Vector2 _characterVelocity;
    private Rigidbody2D _rb;
    private gameManager _gm;

    void Start () {
        _speed = speed;
        _jumpSpeed = jumpspeed;

       _rb = this.gameObject.GetComponent<Rigidbody2D>();
       _gm = GameObject.Find("Manager").GetComponent<gameManager>();

        Debug.Assert(_rb != null, "rigid body is not found!");
        Debug.Assert(_gm != null, "game manager could not be found");
    }

    void Update () {
        
        // check for input here 
        float leftRight = Input.GetAxis("Horizontal")  * _speed ;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            _jump = true;
        }

        if (Input.GetKeyDown ( KeyCode.X))
        {
            _stopMovement = true;
            return;
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && _gm.isAbilityReady())
        {    
            // activate ability
            //Debug.Log("Use ability ");
            _useAbility = true;
        }

        if (leftRight > 0 || leftRight < 0)
        {
            _characterVelocity = new Vector2(leftRight, 0);
            _Move = true;
        }
            
        
    }

    private void FixedUpdate()
    {
        // do the actual movement here 
        // as we would like to make as much use
        // of unity's physics system as possible 

        if ( _useAbility == true)
        {
            Vector2 point = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] targets = Physics2D.OverlapCircleAll(point, blastRadius);
            Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(blastRadius, 0, 0));
            if (targets.Length > 0)
            {
                Debug.Log(targets.Length);
                foreach (Collider2D target in targets)
                {
                    Rigidbody2D trb = target.GetComponent<Rigidbody2D>();
                    if (trb == null || target.name == "player" || target.name == "Player")
                        continue;
                    Vector3 direction = this.transform.position - target.transform.position ;
                    Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
                    //Debug.Log("Target Name: " + target.name + " direction target: " + direction2D);
                    trb.AddForce(direction2D * blastPower, ForceMode2D.Impulse);
                    health h = target.GetComponent<health>();
                    if (h == null)
                        return;
                    h.loseHealth(10);
                }

            }

            // show some cool animation
            _gm.resetAbilityTimer();
            _useAbility = false;

        }


        if (_jump)
        {
            // add _jump movement 
            // Debug.Log("Y Velocity: " + _rb.velocity.y);
            _rb.AddForce((new Vector2 ( 0 , _jumpSpeed)  * PhysicsMultiplier), ForceMode2D.Impulse);
            _jump = false;
        }

        if (_stopMovement)
        {

            _rb.velocity = Vector2.zero;
            _stopMovement = false;
            return;

        }

        if ( _Move == true)
        {   
            // grab the rigidbody 2d  , so we can use the physics system
            if (_rb == null)
                Debug.LogError("Can't find a rigidbody 2D on the player!");
            
            // add left and right movement 
            float newX = _characterVelocity.x + (-(_rb.velocity.x));
            Vector2 newleftright = new Vector2(newX, 0) * 2;
           // Debug.Log(newleftright + " current velocity X: " + _rb.velocity.x );
            _rb.AddForce( newleftright  , ForceMode2D.Impulse);
          
            // after we are done giving our character 
            // a driving force, set this to false
            // to prevent giving it a constant force every frame 
            _Move = false;
        }

        _rb.velocity = new Vector2(Mathf.Clamp (_rb.velocity.x, -2, 2 ) , Mathf.Clamp(_rb.velocity.y, -3, 3));
    }
}
