using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    private Vector2 facingDir = Vector2.right;

    private void Update()
    {
        // Update facing direction from input
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput != Vector2.zero)
            facingDir = moveInput.normalized;

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    void Shoot()
    {
        GameObject proj = ProjectilePool.Instance.GetPooledObject();
        proj.transform.position = firePoint.position;
        proj.transform.rotation = Quaternion.identity;

        proj.GetComponent<Projectile>().Launch(facingDir, this.gameObject);
    }

}
