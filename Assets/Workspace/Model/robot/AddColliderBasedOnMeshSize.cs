using UnityEngine;

public class AddColliderBasedOnMeshSize : MonoBehaviour
{
    void Start()
    {
        // ȷ����һ��MeshFilter���
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("No MeshFilter component found on the object.");
            return;
        }

        // ��ȡMesh�ı߽��
        Bounds bounds = meshFilter.mesh.bounds;

        // ���BoxCollider���
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();

        // ����Collider�ĳߴ�
        // ע�⣺Mesh�ı߽�����Ա����������ģ�����������Ҫ����ת��Ϊ��������
        Vector3 size = bounds.size;
        boxCollider.size = size;

        // �����Ҫ�����Ե���Collider�����ĵ�
        // Ĭ������£�BoxCollider�����ĵ���������ı�������ԭ��
        // ���Mesh�����Ĳ���ԭ�㣬�������Ҫ����Collider������
        boxCollider.center = bounds.center;
    }
}
