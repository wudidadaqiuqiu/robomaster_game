using UnityEditor;
using UnityEngine;

public class NewMesh : MonoBehaviour
{
    void Start()
    {
        // ����һ���µ�Mesh
        Mesh mesh = new Mesh();
        // ...�����ö��㡢�����Ρ����ߡ�UV�ȣ�

        // ��mesh��ֵ��MeshFilter���
        GetComponent<MeshFilter>().mesh = mesh;

        // ����Mesh��AssetsĿ¼
        SaveMesh(mesh, "Assets/Workspace/Model/robot/MyMesh.mesh");
    }

    // ����Mesh�ķ���
    void SaveMesh(Mesh mesh, string path)
    {
        // ��Mesh��Ϊ�ʲ����浽ָ��·��
        AssetDatabase.CreateAsset(mesh, path);
        // ������ĵ�����
        AssetDatabase.SaveAssets();
    }
}
