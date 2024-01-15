using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Grabbable : MonoBehaviour
{
    public enum GrabMode
    {
        FollowParentTeleport,
        FollowParentSmooth,
        SetOnParent,
        SetOnParentZeroPosition
    }
    [System.Flags]
    public enum GrabbableType
    {
        Weapon = 1, //000001
        Throwable = 2, //000010
        Consumible = 4 //000100
    }


    [Header("Setup")]
    [SerializeField] private GrabMode _grabMode = GrabMode.FollowParentTeleport;
    [SerializeField, Min(0)] private float _smoothTime = 0.25f;
    [SerializeField] public GrabbableType grabbableType = GrabbableType.Consumible;

    [Header("Events")]
    public UnityEvent<GameObject, GameObject> OnStartGrab;
    public UnityEvent<GameObject, GameObject> OnEndGrab;

    private IEnumerator _followCorroutine;
    private Vector3 _currentVelocity = Vector3.zero;

    private bool _isGrabbed = false;

    public void GrabSwitch(GameObject parent)
    {
        if(_isGrabbed)
        {
            EndGrab(parent);
        }
        else
        {
            StartGrab(parent);
        }
    }

    public void StartGrabHold(GameObject parent)
    {
        StartGrab(parent);
    }
    private void StartGrab(GameObject parent)
    {
        _isGrabbed = false;

        if(_followCorroutine != null)
        {
            StopCoroutine(_followCorroutine);
            _followCorroutine = null;
        }
        
        switch (_grabMode)
        {
            case GrabMode.FollowParentTeleport:// Ejecuta el mismo codigo que el de abajo 
            case GrabMode.FollowParentSmooth:
                transform.parent = null;

                _followCorroutine = FollowCorroutine(parent);
                StartCoroutine(_followCorroutine);
                break;
            case GrabMode.SetOnParent:
                transform.parent = parent.transform;
                break;
            case GrabMode.SetOnParentZeroPosition:
                transform.parent = parent.transform;
                transform.localPosition = Vector3.zero; 
                break;
        }
        OnStartGrab.Invoke(this.gameObject, parent);
    }
    public void EndGrab(GameObject parent)
    {
        _isGrabbed = false;
        
        if (_followCorroutine != null)
        {
            StopCoroutine(_followCorroutine);
            _followCorroutine = null;
        }

        transform.parent = null;

        TryDrop(parent);

        OnEndGrab.Invoke(this.gameObject, parent);
    }

    private IEnumerator FollowCorroutine(GameObject parent)
    {
        while(true)
        {
            switch (_grabMode)
            {
                case GrabMode.FollowParentTeleport:
                    transform.position = parent.transform.position;
                    break;
                case GrabMode.FollowParentSmooth:
                    transform.position = Vector3.SmoothDamp(transform.position, parent.transform.position, ref _currentVelocity, _smoothTime);
                    break;
                case GrabMode.SetOnParent:
                case GrabMode.SetOnParentZeroPosition:
                    break;
            }

            yield return null;
        }
    }

    private void TryDrop(GameObject parent)
    {
        Interactor interactorParent = parent.GetComponent<Interactor>();

        if (interactorParent == null)
        {
            return;
        }

        Vector3 position = parent.transform.position;
        int inverseMask = ~(parent.layer);

        

        List<Collider> colliders = Physics.OverlapSphere(position, interactorParent.RadiusRange, inverseMask, QueryTriggerInteraction.Collide).ToList();

        colliders.Sort((a, b) => //compara todos los objectos
        {
            float dA = Vector3.Distance(a.ClosestPoint(position) /*punto mas cercano del centro*/, position); // .ClosestPointOnBounds(position) punto mas cercano de los bordes
            float dB = Vector3.Distance(b.ClosestPoint(position), position);
            return dA.CompareTo(dB);
        });

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out DropPlace dropPlace)) 
            {
                if (dropPlace.IsValid(this))
                {
                    dropPlace.OnDrop(this);

                }

                return;//EARLY EXIT
            }
        }

    }
}
