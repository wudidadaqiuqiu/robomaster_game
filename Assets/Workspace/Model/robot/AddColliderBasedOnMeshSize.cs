using UnityEngine;

public class AddColliderBasedOnMeshSize : MonoBehaviour
{
    void Start()
    {
        // 确保有一个MeshFilter组件
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("No MeshFilter component found on the object.");
            return;
        }

        // 获取Mesh的边界框
        Bounds bounds = meshFilter.mesh.bounds;

        // 添加BoxCollider组件
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();

        // 设置Collider的尺寸
        // 注意：Mesh的边界框是以本地坐标计算的，所以我们需要将其转换为世界坐标
        Vector3 size = bounds.size;
        boxCollider.size = size;

        // 如果需要，可以调整Collider的中心点
        // 默认情况下，BoxCollider的中心点是在物体的本地坐标原点
        // 如果Mesh的中心不在原点，你可能需要调整Collider的中心
        boxCollider.center = bounds.center;
    }
}
