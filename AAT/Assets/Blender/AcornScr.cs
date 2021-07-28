using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornScr : MonoBehaviour
{
    [SerializeField] LayerMask badlayer;
    [SerializeField] float speed;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bad")
        {
            collision.gameObject.GetComponent<IHealth>().ModifyHealth(-5f);
            Destroy(gameObject);
        }
    }
}
