using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxspeed;//переменная максимальной скорости
    public float speed;//переменная текущей скорости
    [SerializeField] private float boost;//переменная ускорения
    [SerializeField] private float slow;//переменная замедления
    private float direction;//направление 
    private bool facingdirection;//направление
    [SerializeField] private float jumpforce;//переменная силы прыжка
    private bool isGrounded;//на земле?
    private bool isFall;//падает?
    public bool isWork = false;//работает?

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public LayerMask wallsLayer;
    public LayerMask interactableLayer;
    private States State//функция для смены состояний
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
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
        if (!isWork)
        {
            if (speed == 0 && isGrounded) State = States.idle;
            Run();
            if (isGrounded && Input.GetButtonDown("Jump"))
                Jump();
            Interact();
        }
        else State = States.work;
    }
    private void CheckFall() //проверка падения
    {
        isFall = rb.linearVelocity.y < 0;
        if (!isGrounded && isFall) State = States.fall;
    }
    private void CheckGround() //проверка земли под ногами
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, wallsLayer);
        if (!isGrounded && !isFall) State = States.jump;
    }
    private void Run()//функция бега
    {
        if (speed != 0) State = States.run;
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

    private void Interact() //использование
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("использование");
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.3f, interactableLayer);
            if (hits.Length > 0)
            {
                Interactable interactable = hits[0].GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.Use();
                }
            }
        }
    }

    public enum States //состояния героя
    {
        idle,
        run,
        jump,
        fall,
        work
    }
}
