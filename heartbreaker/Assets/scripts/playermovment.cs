using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("walljumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    


    private void Awake()
    {
        //Grabs references for rigidbody and animator from game object.
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        body.rotation = 0;

        //Flip player when facing left/right.
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(8, 8, 8);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-8, 8, 8);

        if (Input.GetKey(KeyCode.Space) && IsGrounded())
            Jump();

        //sets animation parameters
        anim.SetBool("run", (horizontalInput != 0));
        anim.SetBool("grounded", IsGrounded());
        anim.SetBool("jump", !IsGrounded());

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        //Adjustable Jump Height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        if (OnWall())
        {
            body.drag = 5;
            body.gravityScale = 0.2f;
            //body.velocity = Vector2.zero;
        }
        else
        {
            body.drag = 0;
            body.gravityScale = 6f;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }
    }

    private void Jump()
    {

        if (OnWall())
            WallJump();
        else
        {

            if (IsGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
        }
        
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sin(transform.localScale.x) * wallJumpX, wallJumpY));
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private bool IsGrounded()
    {
        RaycastHit2D RaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return RaycastHit.collider != null;
    }
    private bool OnWall()
    {
        RaycastHit2D RaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return RaycastHit.collider != null;
    }
}
