using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [Header("Debug Support")]
    [SerializeField] private Interactable  _currentInteractable;

    [Header("Setup")]
    [SerializeField] private float _radiusRange = 0.5f;

    private IEnumerator _cheakBreakDistanceCorroutine;

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            TryStartInteract();
        }
        else
        {
            TryEndInteract();
        }
    }

    private void TryStartInteract()
    {
        Vector3 position = transform.position;
        int inverseMask = ~(gameObject.layer);
        List<Collider> colliders = Physics.OverlapSphere(position, _radiusRange, inverseMask, QueryTriggerInteraction.Collide).ToList();

        colliders.Sort((a, b) => //compara todos los objectos
        {
            float dA = Vector3.Distance(a.ClosestPoint(position) /*punto mas cercano del centro*/, position); // .ClosestPointOnBounds(position) punto mas cercano de los bordes
            float dB = Vector3.Distance(b.ClosestPoint(position), position);
            return dA.CompareTo(dB);
        });

        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.TryGetComponent(out Interactable interactable)) //comprueva si el interactable es nullo y retorna la instancia
            {
                _currentInteractable = interactable;
                _currentInteractable.StartInteract(this.gameObject);

                if(interactable.breakWithDistance)
                {
                    _cheakBreakDistanceCorroutine = CheckBreakDistanceCorroutine(collider);
                    StartCoroutine(_cheakBreakDistanceCorroutine);
                }

                return;//EARLY EXIT
            }
        }

    }
    private void TryEndInteract()
    {
        if( _currentInteractable != null)
        {
            _currentInteractable.EndInteract(this.gameObject);
            _currentInteractable=null;
        }

        if( _cheakBreakDistanceCorroutine != null)
        {
            StopCoroutine(_cheakBreakDistanceCorroutine);
            _cheakBreakDistanceCorroutine = null;
        }
    }
    
    private IEnumerator CheckBreakDistanceCorroutine(Collider targetCollider)
    {
        while (Vector3.Distance(targetCollider.ClosestPoint(transform.position), transform.position) <= _radiusRange) //no utilizamos el transform en variable pq se tiene que ejecutar varas veces
        {
            yield return null;
        }

        TryEndInteract();
    }

#if UNITY_EDITOR //Inspector only DOES NOT WORK ON BUILD
    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, _radiusRange);  
    }
#endif
}
