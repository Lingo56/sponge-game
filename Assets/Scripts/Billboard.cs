using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private float scaleFactor = 3f; // Scale factor for the billboard
    [SerializeField] private bool isBillboard = false; // Scale factor for the billboard
    
    private Transform cam;

    void Start()
    {
        if (isBillboard)
        {
            Texture texture = GetComponent<Renderer>().material.mainTexture;
            float aspectRatio = (float)texture.width / texture.height;
        
            transform.localScale = new Vector3(aspectRatio * scaleFactor, scaleFactor, scaleFactor);
        }
        
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        Vector3 camPosition = cam.position;
        camPosition.y = transform.position.y; // Ignore vertical angle
        transform.LookAt(camPosition);
        
        if (isBillboard)
            transform.Rotate(0f, 180f, 0f);
    }

}