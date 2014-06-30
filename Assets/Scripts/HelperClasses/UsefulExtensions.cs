using UnityEngine;

public static class UsefulExtensions
{
    public static Vector3 SetX(this Vector3 vector3, float x)
    {
        return new Vector3(x, vector3.y, vector3.z);
    }

    public static Vector3 SetY(this Vector3 vector3, float y)
    {
        return new Vector3(vector3.x, y, vector3.z);
    }

    public static Vector3 SetZ(this Vector3 vector3, float z)
    {
        return new Vector3(vector3.x, vector3.y, z);
    }
}

public static class TransformExtension
{
    public static void SetX(this Transform transform, float x)
    {
        Vector3 newPosition = transform.position.SetX(x);
        transform.position = newPosition;
    }

    public static void SetY(this Transform transform, float y)
    {
        Vector3 newPosition = transform.position.SetY(y);
        transform.position = newPosition;
    }

    public static void SetZ(this Transform transform, float z)
    {
        Vector3 newPosition = transform.position.SetZ(z);
        transform.position = newPosition;
    }
}

public static class GameObjectExtension
{
    public static T GetSafeComponent<T>(this GameObject obj) where T : MonoBehaviour
    {
        T component = obj.GetComponent<T>();

        if (component == null)
        {
            Debug.LogError("Expected to find component of type "
                           + typeof(T) + " but found none", obj);
        }

        return component;
    }
}

