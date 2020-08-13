using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int strawberryPointAmt = 50; 
    float volume = 0.2f;

    bool pickupBool = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        pickupBool = true;
        if (pickupBool)
        {
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position, volume); 
            Destroy(gameObject);
            FindObjectOfType<GameSession>().AddToScore(strawberryPointAmt);
        }
    }

}
