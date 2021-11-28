using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailCubeMaker : MonoBehaviour
{
    [SerializeField] private GameObject CubePrefab;

    [SerializeField] private float spacing;
    [SerializeField] private float range;

    private void Start()
    {
        Generate();
    }

    public void RegenerateCubes(float range)
    {
        this.range = range;
        Generate();
    }

    private void Generate()
    {
        // Destroy previous GameObjects
        List<Transform> ChildTransforms = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            ChildTransforms.Add(transform.GetChild(i));
        }
        foreach (Transform ct in ChildTransforms)
        {
            Destroy(ct.gameObject);
        }

        // Start creating new GameObjects
        Quaternion ZeroRotation = Quaternion.Euler(Vector3.zero);
        int newCount = 1;
        InstantiateLocal(CubePrefab, Vector3.zero, ZeroRotation);
        for (float z = spacing; z < range; z += spacing)
        {
            InstantiateLocal(CubePrefab, new Vector3(0, 0, z), ZeroRotation);
            InstantiateLocal(CubePrefab, new Vector3(0, 0, -z), ZeroRotation);
            newCount += 2;
        }
        Debug.Log($"RailCubeMaker initialized {newCount} GameObjects");
    }

    private GameObject InstantiateLocal(GameObject prefab, Vector3 localPosition, Quaternion localRotation)
    {
        GameObject current = Instantiate(prefab, transform);
        current.transform.localPosition = localPosition;
        current.transform.localRotation = localRotation;
        return current;
    }
}
