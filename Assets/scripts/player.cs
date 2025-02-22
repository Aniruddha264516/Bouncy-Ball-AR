
using UnityEngine;

public class player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteindex;

    public Vector3 directon;
    public float gravity = -9.8f;
    public float strength = 5f;

    public GameObject visualObject;

    private Vector3 previousPosition;

    private Vector3 defaultScale;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        defaultScale = visualObject.transform.localScale;
        InvokeRepeating(nameof(animatesprite) , 0.15f , 0.15f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        directon = Vector3.zero;
    }


    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            directon = Vector3.up * strength;
        }

      if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                directon = Vector3.up * strength;
            }
        }
        directon.y += gravity * Time.deltaTime;
        transform.position += directon * Time.deltaTime;

        visualObject.transform.localScale = defaultScale + new Vector3(Mathf.Lerp(0.5f, 0f, visualObject.transform.localScale.y), Mathf.Abs(transform.position.y - previousPosition.y) * 20f, 0f);

        previousPosition = transform.position;
    }

    private void animatesprite()
    {
        spriteindex++;

        if(spriteindex >= sprites.Length)
        {
            spriteindex = 0;
        }
        spriteRenderer.sprite = sprites[spriteindex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "obstacal")
        {
            FindObjectOfType<gamemanager>().gameover();
        }else if(other.gameObject.tag == "score")
        {
            FindObjectOfType<gamemanager>().increasesore();            
        }
    }
}
