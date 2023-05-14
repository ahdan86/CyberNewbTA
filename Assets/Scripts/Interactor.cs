using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionRange = 0.5f;
    [SerializeField] private LayerMask _interactionLayer;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _colliderCount;

    private IInteractable _currentInteractable;

    private ThirdPersonController _characterController;

    private void Start()
    {
        _characterController = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        _colliderCount = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionRange, _colliders, 
            _interactionLayer);

        if (_colliderCount > 0)
        {
            for (int i = 0; i < _colliderCount; i++)
            {
                _currentInteractable = _colliders[i].GetComponent<IInteractable>();
                if (_currentInteractable != null)
                {
                    _currentInteractable.SetUpPromptUI();
                    Debug.Log(_currentInteractable.InteractionPrompt);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(InteractWithDelay());
                    }
                }
            }
        } 
        else
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.CloseUI();
                _currentInteractable = null;
            }
        }
    }

    private IEnumerator InteractWithDelay()
    {
        yield return _currentInteractable.Interact(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionRange);
    }
}
