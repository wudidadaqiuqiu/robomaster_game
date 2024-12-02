using UnityEditor;
using UnityEngine;

public class NewMesh : MonoBehaviour
{
    void Start()
    {
        // 创建一个新的Mesh
        Mesh mesh = new Mesh();
        // ...（设置顶点、三角形、法线、UV等）

        // 将mesh赋值给MeshFilter组件
        GetComponent<MeshFilter>().mesh = mesh;

        // 保存Mesh到Assets目录
        SaveMesh(mesh, "Assets/Workspace/Model/robot/MyMesh.mesh");
    }

    // 保存Mesh的方法
    void SaveMesh(Mesh mesh, string path)
    {
        // 将Mesh作为资产保存到指定路径
        AssetDatabase.CreateAsset(mesh, path);
        // 保存更改到磁盘
        AssetDatabase.SaveAssets();
    }
}
