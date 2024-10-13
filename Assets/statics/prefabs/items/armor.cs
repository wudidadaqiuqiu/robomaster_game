using UnityEngine;

namespace Robots {
    class Armor : MonoBehaviour
    {
        void OnTriggerEnter(Collider other) {
            Debug.Log("Armor hit");
            // if (other.gameObject.tag == "Player") {
            //     other.gameObject.GetComponent<Player>().addArmor(1);
            //     Destroy(gameObject);
            // }
        }
        
    }

}