using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [Header("Animation Komponenten")]
    [SerializeField] private Animator animator;
    [SerializeField] private SkinnedMeshRenderer[] meshRenderers;
    
    [Header("Dissolve Einstellungen")]
    [SerializeField] private float dissolveSpeed = 1f;
    private float dissolveValue = 1f;
    private bool isDissolving = false;
    private bool isDead = false;
    
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (animator == null)
        {
            Debug.LogError($"[{gameObject.name}] Kein Animator gefunden!");
            return;
        }
        
        // Starte mit Idle
        animator.Play("idle", 0, 0f);
        Debug.Log($"[{gameObject.name}] Animation System gestartet");
        
        ResetDissolve();
    }
    
    void Update()
    {
        // DEBUG: Zeige Status
        if (isDead)
        {
            Debug.Log($"[{gameObject.name}] Update - isDead: {isDead}, isDissolving: {isDissolving}, dissolveValue: {dissolveValue:F2}");
        }
        
        if (isDissolving)
        {
            UpdateDissolve();
        }
    }
    
    public void PlayAttackAnimation()
    {
        if (animator != null && !isDead)
        {
            animator.Play("attack_shift", 0, 0f);
            Debug.Log($"[{gameObject.name}] Attack Animation gestartet");
        }
    }
    
    public void StartDissolve()
    {
        if (isDead) return;
        
        isDead = true;
        isDissolving = true;
        
        if (animator != null)
        {
            animator.Play("dissolve", 0, 0f);
        }
        
        Debug.Log($"[{gameObject.name}] Dissolve gestartet");
    }
    
    private void UpdateDissolve()
    {
        dissolveValue -= Time.deltaTime * dissolveSpeed;
        dissolveValue = Mathf.Max(0f, dissolveValue);
        
        Debug.Log($"[{gameObject.name}] Dissolve Value: {dissolveValue:F2}");
        
        if (meshRenderers != null)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                if (meshRenderer != null)
                {
                    meshRenderer.material.SetFloat("_Dissolve", dissolveValue);
                }
            }
        }
        
        if (dissolveValue <= 0f)
        {
            isDissolving = false;
            Debug.Log($"[{gameObject.name}] Dissolve abgeschlossen - Zerstöre GameObject!");
            Destroy(gameObject, 0.1f);
        }
    }
    
    private void ResetDissolve()
    {
        dissolveValue = 1f;
        isDissolving = false;
        isDead = false;
        
        if (meshRenderers != null)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                if (meshRenderer != null)
                {
                    meshRenderer.material.SetFloat("_Dissolve", dissolveValue);
                }
            }
        }
    }
    
    public bool IsDead()
    {
        return isDead;
    }
    
    public bool IsDissolving()
    {
        return isDissolving;
    }
}