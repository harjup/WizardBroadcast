using Assets.Scripts.Player;
using UnityEngine;
using System.Collections;

public class CameraFocus : MonoBehaviour
{
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        StartCoroutine(DoFocus());
    }

    IEnumerator DoFocus()
    {
        while (true)
        {
            yield return new WaitForSeconds(4);
            yield return StartCoroutine(FocusIn() );
            yield return new WaitForSeconds(.5f);
            yield return StartCoroutine(FocusOut());
        }
    }

    IEnumerator FocusIn()
    {
        while (_camera.fieldOfView - 40f >= .5f)
        {
            _camera.fieldOfView = iTween.FloatUpdate(_camera.fieldOfView, 40f, 10f);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FocusOut()
    {
        while (90f - _camera.fieldOfView >= .5f)
        {
            _camera.fieldOfView = iTween.FloatUpdate(_camera.fieldOfView, 90f, 10f);
            yield return new WaitForEndOfFrame();
        }   
    }


}
