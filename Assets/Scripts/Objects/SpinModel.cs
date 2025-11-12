using UnityEngine;

namespace Objects
{
    public class SpinModel : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 10f);
        }
    }
}
