using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{

    public float MovementSpeed = 2.0f;

    public float JumpForce = 5.0f;

    private Rigidbody _Rigidbody;

    private bool IsGrounded;

    public float DistationToGround = 0.1f;

    public List<GameObject> WeaponInventory = new List<GameObject>();

    public List<GameObject> WeaponMeshes = new List<GameObject>();

    public int SelectedWeaponId = 0;

    public float RotationSmothing = 20f;

    public GameObject HandMeshes;

    private float pitch, yaw;

    public float SprintSpeed = 4.0f;

    private GameManager _GameManager;

    private Weapon _Weapon;

    private AnimationManager _AnimationManager;

    private bool IsSprinting = false;

    // Start is called before the first frame update
    private void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _GameManager = FindObjectOfType<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (WeaponMeshes[SelectedWeaponId] != null)
        {
            _AnimationManager = WeaponMeshes[SelectedWeaponId].GetComponent<AnimationManager>();
            if (_AnimationManager == null)
            {
                Debug.LogError("AnimationManager is missing on the selected weapon mesh.");
            }
        }
        else
        {
            Debug.LogError("WeaponMeshes[SelectedWeaponId] is null.");
        }






        if (WeaponInventory.Count > 0)
        {
            _Weapon = WeaponInventory[SelectedWeaponId].GetComponent<Weapon>();
            WeaponMeshes[SelectedWeaponId].SetActive(true);
        }

        if (WeaponMeshes.Count > SelectedWeaponId)
        {
            _AnimationManager = WeaponMeshes[SelectedWeaponId].GetComponent<AnimationManager>();
            if (_AnimationManager == null)
            {
                Debug.LogError("No AnimationManager found on the selected weapon mesh.");
            }
        }
        else
        {
            Debug.LogError("WeaponMeshes list does not contain the selected weapon.");
        }

        if (WeaponMeshes.Count > 0)
        {
            _AnimationManager = WeaponMeshes[SelectedWeaponId].GetComponent<AnimationManager>();
        }
        else
        {
            Debug.LogError("WeaponMeshes is empty.");
        }
    }



    // Update is called once per frame
    private void Jump()
    {
        _Rigidbody.AddForce(UnityEngine.Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void GroundCheck()
    {
        IsGrounded = Physics.Raycast(transform.position, UnityEngine.Vector3.down, DistationToGround);
    }

    private UnityEngine.Vector3 CalculateMovment()
    {
        float HorizontalDirection = Input.GetAxis("Horizontal");
        float VerticalDirection = Input.GetAxis("Vertical");

        UnityEngine.Vector3 Move = transform.right * HorizontalDirection + transform.forward * VerticalDirection;
        return _Rigidbody.transform.position + Move * Time.fixedDeltaTime * MovementSpeed;
    }

    private void FixedUpdate()
    {
        GroundCheck();

        if (Input.GetKey(KeyCode.Space) && IsGrounded) Jump();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            _Weapon.Fire();
            _AnimationManager.SetAnimationFire();
        }

        if (Input.GetKey(KeyCode.R))
        {
            _Weapon.Reload();
            _AnimationManager.SetAnimationReload();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) SelectNextWeapon();
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) SelectPrevWeapon();

        _Rigidbody.MovePosition(CalculateMovment());
        SetRotation();

        // Проверка на бег
        IsSprinting = Input.GetKey(KeyCode.LeftShift) && !_GameManager.IsStaminaRestoring;
        if (IsSprinting)
        {
            _GameManager.SpendStamina();
            _Rigidbody.MovePosition(CalculateSprint());
        }

        SetAnimation();
    }

    private void OnDrawnGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (UnityEngine.Vector3.down * DistationToGround));
    }

    private void SetRotation()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, -60, 90);

        UnityEngine.Quaternion SmoothRotation = UnityEngine.Quaternion.Euler(pitch, yaw, 0);

        HandMeshes.transform.rotation = UnityEngine.Quaternion.Slerp(HandMeshes.transform.rotation, SmoothRotation,
        RotationSmothing * Time.fixedDeltaTime);

        SmoothRotation = UnityEngine.Quaternion.Euler(0, yaw, 0);

        transform.rotation = UnityEngine.Quaternion.Slerp(transform.rotation, SmoothRotation, RotationSmothing * Time.fixedDeltaTime);
    }

    private void SelectPrevWeapon()
    {
        if (SelectedWeaponId > 0) // Проверка индекса
        {
            WeaponMeshes[SelectedWeaponId].SetActive(false);
            SelectedWeaponId -= 1;

            _Weapon = WeaponInventory[SelectedWeaponId].GetComponent<Weapon>();
            WeaponMeshes[SelectedWeaponId].SetActive(true);

            // Обновляем компонент AnimationManager
            _AnimationManager = WeaponMeshes[SelectedWeaponId].GetComponent<AnimationManager>();

            if (_AnimationManager == null)
            {
                Debug.LogError("No AnimationManager found on the selected weapon mesh.");
            }

            Debug.Log("Пушка: " + _Weapon.WeaponType);
        }
        else
        {
            Debug.LogWarning("No previous weapon.");
        }
    }


    private void SelectNextWeapon()
    {
        if (WeaponInventory.Count > SelectedWeaponId + 1) // Проверка индекса
        {
            WeaponMeshes[SelectedWeaponId].SetActive(false);
            SelectedWeaponId += 1;

            _Weapon = WeaponInventory[SelectedWeaponId].GetComponent<Weapon>();  // Обновляем _Weapon
            WeaponMeshes[SelectedWeaponId].SetActive(true);

            // Обновляем компонент AnimationManager
            _AnimationManager = WeaponMeshes[SelectedWeaponId].GetComponent<AnimationManager>();

            if (_AnimationManager == null)
            {
                Debug.LogError("No AnimationManager found on the selected weapon mesh.");
            }

            Debug.Log("Пушка: " + _Weapon.WeaponType);
        }
        else
        {
            Debug.LogWarning("No next weapon.");
        }
    }



    private UnityEngine.Vector3 CalculateSprint()
    {
        IsSprinting = true;

        float HorizontalDirection = Input.GetAxis("Horizontal");
        float VerticalDirection = Input.GetAxis("Vertical");
        UnityEngine.Vector3 Move = transform.right * HorizontalDirection + transform.forward * VerticalDirection;
        return _Rigidbody.transform.position + Move * Time.fixedDeltaTime * SprintSpeed;

    }

    public void PickupWeapon(GameObject newWeapon, GameObject weaponModel)
    {
        WeaponInventory.Add(newWeapon);
        WeaponMeshes.Add(weaponModel);

        BoxCollider boxCollider = newWeapon.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        weaponModel.transform.SetParent(HandMeshes.transform);

        weaponModel.transform.localPosition = new UnityEngine.Vector3(0, 0.24f, 0.3f);
        weaponModel.transform.localRotation = UnityEngine.Quaternion.identity;

        weaponModel.SetActive(false);
        SelectNextWeapon();

        Debug.Log("Pick the gun: " + newWeapon.GetComponent<Weapon>().WeaponType);
    }

    private bool IsMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 ;
    }


    private void SetAnimation()
    {
        bool isMoving = IsMoving();

        if (isMoving)
        {
            if (IsSprinting)
            {
                _AnimationManager.SetAnimationRun(); // анимация бега
                Debug.Log("Setting run animation for " + _Weapon.WeaponType);

            }
            else
            {
                _AnimationManager.SetAnimationWalk(); // анимация ходьбы
                Debug.Log("Setting walk animation for " + _Weapon.WeaponType);

            }
        }
        else
        {
            _AnimationManager.SetAnimationIdle();
            Debug.Log("Setting idle animation for " + _Weapon.WeaponType);

        }
    }


}
