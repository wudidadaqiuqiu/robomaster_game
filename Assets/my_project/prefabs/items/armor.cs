using UnityEngine;

namespace RoboticItems {
    class Armor : MonoBehaviour
    {
        void OnTriggerEnter(Collider other) {
            Debug.Log("Armor triggered");
            // if (other.gameObject.tag == "Player") {
            //     other.gameObject.GetComponent<Player>().addArmor(1);
            //     Destroy(gameObject);
            // }
        }

        void OnCollisionEnter(Collision other) {
            Debug.Log("Armor collided");
            // if (other.gameObject.tag == "Player") {
            //     other.gameObject.GetComponent<Player>().addArmor(1);
            //     Destroy(gameObject);
            // }
        }
        
    }

}