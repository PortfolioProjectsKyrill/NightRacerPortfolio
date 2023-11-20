using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

interface IMoveAble
{
    void Move();
    void Drift();
    void TireSteering();
    void Steer();
    void GroundNormalRotation();

}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;


    public float currentSpeed = 0;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float realSpeed;//to show velocity

    [Header("Tires")]
    public Transform frontLeftTire;
    public Transform frontRightTire;
    public Transform backLeftTire;
    public Transform backRightTire;

    //drift and steering stuffz
    [SerializeField] private float steerDirection;
    [SerializeField] private float driftTime;

    public bool isSliding = false;

    private bool touchingGround;

    [Header("Particles Drift Sparks")]
    public Transform leftDrift;
    public Transform rightDrift;
    public Color drift1;
    public Color drift2;
    public Color drift3;

    [HideInInspector]
    public float BoostTime = 0;

    public Transform boostFire;
    public Transform boostExplosion;

    [SerializeField] private float IsGroundedRaycast;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        move();
        tireSteer();
        steer();
        groundNormalRotation();
        driftTimerFunc();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && touchingGround)//wont work in FixedUpdate() bc of timing so it has to be here
        {
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Hop");
        }
    }

    /// <summary>
    /// Controls the backwards and forwards direction
    /// </summary>
    private void move()
    {
        realSpeed = transform.InverseTransformDirection(rb.velocity).z;//real velocity before setting the value. This can be useful if say you want to have hair moving on the player, but don't want it to move if you are accelerating into a wall, since checking velocity after it has been applied will always be the applied value, and not real

        if (Input.GetKey(KeyCode.W))
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * 0.5f);//speed forward
        else if (Input.GetKey(KeyCode.S))
            currentSpeed = Mathf.Lerp(currentSpeed, -maxSpeed / 1.75f, Time.deltaTime);//speed backwards
        else
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * 1.5f);//comes to standstill

        Vector3 vel = transform.forward * currentSpeed;//forward movement functionality in vector3 var
        vel.y = rb.velocity.y;//normal velocity gets set to rigidbody downwards velocity since gravity = -75m/s
        rb.velocity = vel;//velocity of the rigidbody on the car gets set to the speed calc
    }
    /// <summary>
    /// Controls drifting rotation of the car and controls the steering
    /// </summary>
    private void steer()
    {
        steerDirection = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        Vector3 steerDirVect; //this is used for the final rotation of the kart for steering
        float steerAmount;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (steerDirection == 1 && Input.GetKey(KeyCode.D)){
                if (transform.GetChild(0).localRotation == Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 20f, 0), 8f * Time.deltaTime))
                    isSliding = true;

                if (Input.GetAxis("Horizontal") < 0)
                    steerDirection = 1.5f;
                else
                    steerDirection = 0.5f;
                

                transform.GetChild(0).localRotation =
                Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 20f, 0), 8f * Time.deltaTime);
            }
            else if (steerDirection == -1 && Input.GetKey(KeyCode.A)){
                if (transform.GetChild(0).localRotation == Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, -20f, 0), 8f * Time.deltaTime))
                    isSliding = true;

                if (Input.GetAxis("Horizontal") > 0)
                    steerDirection = -1.5f;
                else
                    steerDirection = -0.5f;

                transform.GetChild(0).localRotation =
                Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, -20f, 0), 8f * Time.deltaTime);
            }
            else if (steerDirection == 0){
                isSliding = false;
                transform.GetChild(0).localRotation =
                Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 0f, 0), 8f * Time.deltaTime);
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift)){
            transform.GetChild(0).localRotation =
            Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 0f, 0), 8f * Time.deltaTime);
            isSliding = false;
        }

        //since handling is supposed to be stronger when car is moving slower, we adjust steerAmount depending on the real speed of the kart, and then rotate the kart on its y axis with steerAmount
        if (realSpeed > 30)
            steerAmount = (currentSpeed / 0.8f) * steerDirection;
        else
            steerAmount = (currentSpeed / 1f) * steerDirection;

        if (steerAmount > 0)
            steerDirection = 1;
        else if (steerAmount < 0)
            steerDirection = -1;
        else
            steerDirection = 0;

        steerDirVect = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + steerAmount, transform.eulerAngles.z);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, steerDirVect, 3 * Time.deltaTime);
    }
    /// <summary>
    /// sets the rotation of the car to match the surface its driving on
    /// </summary>
    private void groundNormalRotation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 5f)){
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation, 10 * Time.deltaTime);
            touchingGround = true;
        }
        else
            touchingGround = false;
    }
    
    /// <summary>
    /// Controls the y rotation of the front wheels & the x rotation according to the direction of the car
    /// </summary>
    private void tireSteer()
    {
        if (steerDirection == -1)
        {
            EulerAnglesChange(frontLeftTire, -35);
            EulerAnglesChange(frontRightTire, -35);
        }
        else if (steerDirection == 1)
        {
            EulerAnglesChange(frontLeftTire, 35);
            EulerAnglesChange(frontRightTire, 35);
        }
        else
        {
            EulerAnglesChange(frontLeftTire, 0);
            EulerAnglesChange(frontRightTire, 0);
        }

        #region tire spinning
        if (currentSpeed > 30)
        {
            frontLeftTire.GetChild(0).Rotate(-90 * Time.deltaTime * currentSpeed * 0.5f, 0, 0);
            frontRightTire.GetChild(0).Rotate(-90 * Time.deltaTime * currentSpeed * 0.5f, 0, 0);
            TireRotationNormal(backLeftTire, currentSpeed);
            TireRotationNormal(backRightTire, currentSpeed);
        }
        else
        {
            frontLeftTire.GetChild(0).Rotate(-90 * Time.deltaTime * realSpeed * 0.5f, 0, 0);
            frontRightTire.GetChild(0).Rotate(-90 * Time.deltaTime * realSpeed * 0.5f, 0, 0);
            TireRotationNormal(backLeftTire, realSpeed);
            TireRotationNormal(backRightTire, realSpeed);
        }
        #endregion
    }

    /// <summary>
    /// Controls the Driftime
    /// </summary>
    private void driftTimerFunc()
    {
        if (isSliding == true)
            driftTime += Time.deltaTime;
        else 
            driftTime = 0;
    }

    /// <summary>
    /// changes localEulerAngles (used in the front tires atm)
    /// </summary>
    /// <param name="tire"></param>
    /// <param name="changeAmount"></param>
    private void EulerAnglesChange(Transform tire, int changeAmount)
    {
        tire.localEulerAngles = Vector3.Lerp(tire.localEulerAngles, new Vector3(0, changeAmount + 180, 0), 5 * Time.deltaTime);
    }

    /// <summary>
    /// changes the x rotation of the tires to simulate spinning of the tires
    /// </summary>
    /// <param name="Tire"></param>
    /// <param name="TypeSpeed"></param>
    private void TireRotationNormal(Transform Tire, float TypeSpeed)
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
            Tire.Rotate(90 * Time.deltaTime * TypeSpeed * 0.5f, 0, 0);
        else
            Tire.Rotate(0, 90 * Time.deltaTime * TypeSpeed * 0.5f, 0);
    }
}