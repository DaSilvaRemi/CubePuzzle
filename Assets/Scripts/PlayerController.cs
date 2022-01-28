using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mvt Setup")]
    [Tooltip("unit m/s")]
    [SerializeField] private float m_TranslationSpeed;
    [Tooltip("unit: °/s")]
    [SerializeField] private float m_RotatingSpeed;

    private Rigidbody m_Rigidbody;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    /**
     * <summary>Move the player according to the Vertical and Horizontal input</summary> 
     */
    private void Move()
    {
        
        //VERTICAL INPUT
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 worldmovVect = m_TranslationSpeed * Time.fixedDeltaTime * transform.forward * verticalInput;
        m_Rigidbody.MovePosition(transform.position + worldmovVect);

        //HORIZONTAL INPUT
        float horizontalInput = Input.GetAxis("Horizontal");
        float deltaAngle = m_RotatingSpeed * Time.fixedDeltaTime * horizontalInput;
        Quaternion qRot = Quaternion.AngleAxis(deltaAngle, transform.up);
        m_Rigidbody.MoveRotation(qRot * transform.rotation);

    }
}
