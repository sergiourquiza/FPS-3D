using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float shootDistance = 4f;
    [SerializeField] private ParticleSystem shootPS;
    [SerializeField] private ParticleSystem shotGunPS;
    [SerializeField] private GameObject shotGun;
    [SerializeField] private GameObject rifle;

    private ParticleSystem particleSystemSetted;
    public float health { private set; get; } = 100f;
    private bool shotGunState = true;

    private Rigidbody mRb;
    private Vector2 mDirection;
    private Vector2 mDeltaLook;
    private Transform cameraMain;
    private GameObject debugImpactSphere;
    private GameObject bloodObjectParticles;
    private GameObject otherObjectParticles;
    private GameObject shotGunParticles;



    private void Start()
    {
        mRb = GetComponent<Rigidbody>();
        cameraMain = transform.Find("Main Camera");

        debugImpactSphere = Resources.Load<GameObject>("DebugImpactSphere");
        bloodObjectParticles = Resources.Load<GameObject>("BloodSplat_FX Variant");
        otherObjectParticles = Resources.Load<GameObject>("GunShot_Smoke_FX Variant");
        shotGunParticles = Resources.Load<GameObject>("VFX_M4 Muzzle Flash");

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        mRb.velocity = mDirection.y * speed * transform.forward 
            + mDirection.x * speed * transform.right;

        transform.Rotate(
            Vector3.up,
            turnSpeed * Time.deltaTime * mDeltaLook.x
        );
        cameraMain.GetComponent<CameraMovement>().RotateUpDown(
            -turnSpeed * Time.deltaTime * mDeltaLook.y
        );

    }

    private void OnMove(InputValue value)
    {
        mDirection = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        mDeltaLook = value.Get<Vector2>();
    }

    private void OnFire(InputValue value)
    {
        if (value.isPressed)
        {
            Shoot();
        }
    }

    private void OnSwitch(InputValue value)
    {
        if (value.isPressed)
        {
            shotGunState = !shotGunState;
            rifle.SetActive(!shotGunState);
            shotGun.SetActive(shotGunState);
        }
    }

    private void Shoot()
    {
        if (shotGunState)
        {
            shotGunPS.Play();
        }
        else
        {
            shootPS.Play();
        }

        RaycastHit hit;
        if (Physics.Raycast(
            cameraMain.position,
            cameraMain.forward,
            out hit,
            shotGunState ? shootDistance : shootDistance * 2f
        ))
        {
            if (hit.collider.CompareTag("Enemigos"))
            {
                var bloodPS = Instantiate(bloodObjectParticles, hit.point, Quaternion.identity);
                Destroy(bloodPS, 2f);
                var enemyController = hit.collider.GetComponent<EnemyController>();
                enemyController.TakeDamage(shotGunState ? 2.5f : 1f);
            }else
            {
                var otherPS = Instantiate(otherObjectParticles, hit.point, Quaternion.identity);
                otherPS.GetComponent<ParticleSystem>().Play();
                Destroy(otherPS, 3f);
            }
            
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            // Fin del juego
            Debug.Log("Fin del juego");
            SceneManager.LoadScene("Scenes/MainScene");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemigo-Attack"))
        {
            Debug.Log("Player recibio danho");
            TakeDamage(4f);
        }
        
    }

}
