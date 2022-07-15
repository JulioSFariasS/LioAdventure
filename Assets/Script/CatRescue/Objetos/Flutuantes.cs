using UnityEngine;

public class Flutuantes : MonoBehaviour
{
    protected float contaTempo;
    protected Rigidbody2D rb;

    //movimento flutuante
    public float velocidade;
    public float floatStrength;
    public Transform posBase;
    public bool horizontal;
    public bool basePresa;
    private Vector2 position;
    public bool stopContaTempo;

    protected void Start()
    {
        if (!basePresa)
        {
            posBase.parent = null;
        }
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        if (posBase == null)
        {
            Destroy(gameObject);
        }
        if (!stopContaTempo)
        {
            contaTempo += Time.deltaTime;
        }
    }

    protected void FixedUpdate()
    {
        if(!stopContaTempo && posBase!=null)
        {
            if (!horizontal)
            {
                float newPosition = Mathf.Sin(contaTempo * velocidade) * floatStrength;
                position = new Vector2(0, newPosition) + new Vector2(posBase.position.x, posBase.position.y);
            }
            else
            {
                float newPosition = Mathf.Cos(contaTempo * velocidade) * floatStrength;
                position = new Vector2(newPosition, 0) + new Vector2(posBase.position.x, posBase.position.y);
            }
        }

        rb.MovePosition(Vector2.MoveTowards(transform.position, position, 1 * Time.deltaTime));
    }

    public Transform GetBase()
    {
        return posBase;
    }

}
