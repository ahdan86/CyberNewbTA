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

    public IInteractable _currentInteractable;

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
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        _currentInteractable.Interact(this);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionRange);
    }
}
