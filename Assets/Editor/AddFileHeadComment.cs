using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
public class AddFileHeadComment : UnityEditor.AssetModificationProcessor
{
    /// <summary>
    /// asset被创建完，文件已经生成到磁盘上，但是没有生成.meta文件和import之前被调用
    /// </summary>
    /// <param name="newFileMeta">由创建文件的path加上.meta组成</param>
    public static void OnWillCreateAsset(string newFileMeta)
    {
        string newFilePath = newFileMeta.Replace(".meta", "");
        string fileExt = Path.GetExtension(newFilePath);
        if(fileExt!=".cs")
        {
            return;
        }

        string realPath = Application.dataPath.Replace("Assets", "") + newFilePath;
        string scriptContent = File.ReadAllText(realPath);

        //这里实现自定义的一些规则  

        scriptContent = scriptContent.Replace("#SCRIPTFULLNAME#", Path.GetFileName(newFilePath));  
//        scriptContent = scriptContent.Replace("#COMPANY#", PlayerSettings.companyName);  
        scriptContent = scriptContent.Replace("#NAMESPACE#", "StopWatch");  
        scriptContent = scriptContent.Replace("#COMPANY#", "延澈左");  
        scriptContent = scriptContent.Replace("#AUTHOR#", "延澈左");  
        scriptContent = scriptContent.Replace("#VERSION#", "1.0");  
//        scriptContent = scriptContent.Replace("#UNITYVERSION#", Application.unityVersion);  
        scriptContent = scriptContent.Replace("#DATE#", System.DateTime.Now.ToString("yyyy-MM-dd"));  
  
        File.WriteAllText(realPath, scriptContent);

    }
}
