using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxspeed;//переменная максимальной скорости
    private float speed;//переменная текущей скорости
    [SerializeField] private float boost;//переменная ускорения
    [SerializeField] private float slow;//переменная замедления
    private float direction;//направление 
    private bool facingdirection;//направление
    [SerializeField] private float jumpforce;//переменная силы прыжка
    private bool isGrounded;//на земле?
    private bool isFall;//падает?

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public LayerMask wallsLayer;
    /*private States State//функция для смены состояний
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }*/
    private void Awake() //функция которая вызывает другие функции еще до начала программы
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        CheckFall();
        CheckGround();
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
    }
    void Update()
    {
        Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetKeyDown(KeyCode.W))
        {
            interact();
        }
    }
    private void CheckFall() //проверка падения
    {
        isFall = rb.linearVelocity.y < 0;
        //if (!isGrounded && isFall) State = States.fall;
    }
    private void CheckGround() //проверка земли под ногами
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, wallsLayer);
        //if (!isGrounded && !isFall) State = States.jump;
    }
    private void Run()//функция бега
    {
        //if (isGrounded) State = States.run;
        direction = Input.GetAxisRaw("Horizontal");

        if (direction != 0)
        {
            if ((direction > 0 && speed < 0) || (direction < 0 && speed > 0))
            {
                speed = Mathf.MoveTowards(speed, 0, slow * Time.deltaTime);
            }
            else
            {
                speed = Mathf.MoveTowards(speed, direction * maxspeed, boost * Time.deltaTime);
            }
        }
        else
        {
            speed = Mathf.MoveTowards(speed, 0, slow * Time.deltaTime);
        }

        if (speed > 0 && !facingdirection)
            Flip();
        else if (speed < 0 && facingdirection)
            Flip();
    }

    private void Flip()
    {
        facingdirection = !facingdirection; 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Jump()//прыжок
    {
        rb.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
    }

    private void Interact()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.3f);
        if (hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null && interactable.isInteractable)
            {
                interactable.Use();
            }
        }
    }

    /*public enum States //состояния героя
    {
        idle,
        run,
        jump,
        fall,
        work
    }*/
}
