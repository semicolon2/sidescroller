using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float m_MaxSpeed = 10f;
    [SerializeField] private float m_JumpForce;

    private Animator m_Anim;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private bool m_Jump = false;
    private bool m_Grounded;

    // Use this for initialization
    void Start () {
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (!m_Jump) {
            m_Jump = Input.GetKey("z");
        } else {
            if (m_Grounded == false) {
                m_Jump = false;
            }
        }
	}

    void FixedUpdate() {
        m_Rigidbody2D.rotation = 0;
        Move();
        Vector2 jump = new Vector2(0.0F, m_JumpForce);
        if (m_Jump == true && m_Grounded == true) {
            Debug.Log("jump is true");
            m_Rigidbody2D.AddForce(jump, ForceMode2D.Impulse);
            m_Anim.SetTrigger("Jump");
        }
        
    }

    void OnCollisionStay2D (Collision2D col) {
        if (col.gameObject.tag == "ground") {
            m_Grounded = true;
        }
    }

    void OnCollisionExit2D (Collision2D col) {
        if (col.gameObject.tag == "ground") {
            m_Grounded = false;
        }
    }

    void FlipHorizontal() {
        m_FacingRight = !m_FacingRight;
        m_Anim.transform.Rotate(0, 180, 0);
    }

    void Move() {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(horizontalMovement, m_Rigidbody2D.velocity.y);
        if (horizontalMovement > 0 && !m_FacingRight) {
            FlipHorizontal();
        } else if (horizontalMovement < 0 && m_FacingRight) {
            FlipHorizontal();
        }
        m_Rigidbody2D.velocity = movement * m_MaxSpeed;
        if (horizontalMovement != 0) {
            m_Anim.SetTrigger("Walk");
        } else {
            m_Anim.SetTrigger("Stop");
        }
    }
}
