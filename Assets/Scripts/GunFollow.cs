using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFollow : MonoBehaviour
{
    public RectTransform UGUICamera;
    public Camera mainCamera;

    private void Update()
    {
        Vector3 mousePosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(UGUICamera, new Vector2(Input.mousePosition.x, Input.mousePosition.y), mainCamera, out mousePosition);
        float z;
        if (mousePosition.x>transform.position.x)
        {
            z = Mathf.Max(-Vector3.Angle(Vector3.up, mousePosition - transform.position), -85);
        }
        else
        {
            z = Mathf.Min(Vector3.Angle(Vector3.up, mousePosition - transform.position), 85);
        }

        transform.localRotation = Quaternion.Euler(0, 0, z);
    }

}
