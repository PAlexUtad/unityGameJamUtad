using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraOrbit : MonoBehaviour
{
    // Punto sobre el que rotar
    [SerializeField] public Transform targetperspective;
    [SerializeField] public Transform targettopelement;
    [SerializeField] public float CameraHeight;

    [SerializeField] public GameObject PieceToSpawn;

    public float rotationSpeed = 80f;
    private float distance     = 0.5f;

    private Vector3 offset;

    public float heightChangeSpeed = 1f;
    public float minHeight = 1f;
    public float maxHeight = 100f;

    //true  = vista perspectiva
    //false = vista top
    private bool typeofview = true; 

    void Start()
    {
        if (targetperspective == null || targettopelement == null)
        {
            Debug.LogError("Faltan targets por determinar.");
            return;
        }

        // Calcular la distancia inicial y ángulo de la cámara respecto al target
        offset         = transform.position - targetperspective.position;
        distance       = offset.magnitude * distance;
        CameraHeight   = Quaternion.LookRotation(offset).eulerAngles.y;
    }

    void Update()
    {
        if (targetperspective == null) return;

        // movimiento adelante atras camara
        if (Input.GetKey(KeyCode.W))
            distance -= Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            distance += Time.deltaTime;



        // cambiar tipo camara
        if (Input.GetKeyDown(KeyCode.Space))
            typeofview = !typeofview;
        int typeofviewint = typeofview ? 1 : 0;
        switch (typeofviewint)
        {
            case 1:
                //A D
                float inputX = 0f;
                if (Input.GetKey(KeyCode.A)) inputX = 1f;
                if (Input.GetKey(KeyCode.D)) inputX = -1f;
                
                CameraHeight += inputX * rotationSpeed * Time.deltaTime;

                Quaternion newRotation = Quaternion.Euler(0, CameraHeight, 0);
                Vector3 newPosition = targetperspective.position - (newRotation * Vector3.forward * distance);

                transform.position = newPosition;
                transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);

                /*
                if(Input.GetKeyDown(KeyCode.E))
                {
                    offset.y += heightChangeSpeed * Time.deltaTime;
                    offset.y = Mathf.Clamp(offset.y, minHeight, maxHeight);
                    transform.position += offset;
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    offset.y -= heightChangeSpeed * Time.deltaTime;
                    offset.y = Mathf.Clamp(offset.y, minHeight, maxHeight);
                    transform.position += offset;
                }
                */
                transform.LookAt(targetperspective);
                break;
            case 0:
                transform.position = targettopelement.position;
                //transform.rotation = targettopelement.rotation;
                transform.LookAt(targetperspective);
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(PieceToSpawn, targettopelement.position, Quaternion.identity);

        }
    }
}
