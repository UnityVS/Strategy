using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    [SerializeField] Knight knight;
    public void Attack()
    {
        if (knight != null)
        {
            knight.PullDamage();
        }
    }
}
