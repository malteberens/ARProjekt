using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject hitEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        // Treffer-Effekt erzeugen
        if (hitEffectPrefab != null)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(
                hitEffectPrefab,
                contact.point,
                Quaternion.LookRotation(contact.normal)
            );
        }

        // Schaden anwenden
        TargetHealth target = collision.gameObject.GetComponent<TargetHealth>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}