using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private float movementSpeed;
    private float horizontal;
    private float vertical;
    private float currentVelocity;
    private float smoothTurnTime = 0.1f;
    private Vector3 direction;
    private Animator anim;
    private Transform cam;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }



    private void PlayerMovement()
    {
        if (Enviroment.Instance.freeLookActive)
        {
            anim.SetFloat("Walk", direction.magnitude);
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            direction = new Vector3(horizontal, 0, vertical).normalized;
            if ((direction.magnitude > 0.01))
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTurnTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                rb.MovePosition(transform.position + (moveDir.normalized * movementSpeed * Time.fixedDeltaTime));
            }
        }
        if (Enviroment.Instance.freeLookActive == false)
        {
            anim.SetFloat("Walk", 0);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Portal")
        {
            transform.position = new Vector3(13f, 0.32f, .5f);
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }


}
