using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using ET;
using TMPro;
namespace ETEditor
{
    enum ScrpitType
    {
        All,
        UIType,
        System,
        View,
        AssetBundleName,
        UIEvent
    }
    enum CoompotType
    {

        Normal,
        Child,
        PopUp,
        Hint,
        Error,
    }
    public class UICommpotScriptWindow : EditorWindow
    {
        [MenuItem("Tools/生成UIComponent脚本")]
        public static void ShowWindow()
        {
            GetWindow(typeof(UICommpotScriptWindow));
        }
        //UILoginComponent
        private string className = "NewComponent";
        private string scriptText = "";
        private CoompotType coompotType = CoompotType.Normal;
        private void OnGUI()
        {
            EditorGUILayout.LabelField("类名根据预制体自动生成");
            //GUILayout.BeginHorizontal();
            if (GUILayout.Button("生成全部脚本"))
            {
                CreateScript(ScrpitType.All);
            }
            //this.coompotType = (CoompotType)EditorGUILayout.EnumPopup("CoompotType: ", this.coompotType);
            //GUILayout.EndHorizontal();
            //if (GUILayout.Button("生成UIItem脚本"))
            //{
            //    CreateScript(ScrpitType.Item);
            //}
            GUILayout.Label("单独生成指令");
            if (GUILayout.Button("生成View脚本"))
            {
                CreateScript(ScrpitType.View);
            }
            if (GUILayout.Button("添加UIType"))
            {
                CreateScript(ScrpitType.UIType);
            }
            if (GUILayout.Button("标记AssetBundleName"))
            {
                SignAssetBundleName();
            }
            if (GUILayout.Button("添加UIEVENT"))
            {
                CreateScript(ScrpitType.UIEvent);
            }
            //scriptText = EditorGUILayout.TextField("Script Text", scriptText);
        }
        //创建UI脚本
        private void CreateScript(ScrpitType type)
        {
            //获得选择的所有对象
            GameObject[] arr = Selection.gameObjects;
            //判断对象是否为空
            if (arr != null && arr.Length > 0 && arr[0] != null)
            {
                //只对选择的第一个对象进行处理
                GameObject go = arr[0];
                ReferenceCollector referenceCollector = go.GetComponent<ReferenceCollector>();
                if (referenceCollector==null)
                {
                    Log.Error("请选择一个带有RC脚本的预制体");
                    return;
                }
                switch (type)
                {
                    case ScrpitType.All:
                        CreateAllScript(go);
                        break;
                    case ScrpitType.UIType:
                        CreateUIType(go);
                        break;
                    case ScrpitType.System:
                        CreateSystemScript(go);
                        break;
                    case ScrpitType.View:
                        CreateViewScript(go);
                        break;
                    case ScrpitType.AssetBundleName:
                        break;
                    case ScrpitType.UIEvent:
                        CreateUIEvent(go);
                        break;
                }

            }
            else
            {
                Log.Error("请选择一个带有RC脚本的预制体");
            }
        }
        //一步到位
        private void CreateAllScript(GameObject go)
        {
            CreateUIType(go);
            CreateSystemScript(go);
            CreateViewScript(go);
            CreateUIEvent(go);
            SignAssetBundleName();
        }
        private void CreateUIEvent(GameObject go)
        {
            using (var sr = new StreamReader(@"Assets\Editor\CreaterScritEditor\UIEvent.txt"))
            {
                DirectoryInfo di = new DirectoryInfo($"Assets/HotfixView/UI/{go.name}");
                di.Create();
                var finalFileName = $"Assets/HotfixView/UI/{go.name}/{go.name}Event.cs";
                File.Delete(finalFileName);
                using (var fs = new FileStream(finalFileName, FileMode.OpenOrCreate))
                {
                    if (!fs.CanWrite)
                    {
                        throw new System.Security.SecurityException("文件fileName=" + finalFileName + "是只读文件不能写入!");
                    }
                    var sw = new StreamWriter(fs);
                    sw.Write(sr.ReadToEnd().Replace("*", go.name));
                    sw.Dispose();
                    sw.Close();
                }
            }
        }
        //创建View脚本
        private void CreateViewScript(GameObject go)
        {
            //获取变量存储工具
            ReferenceCollector referenceCollector = go.GetComponent<ReferenceCollector>();
            if (referenceCollector != null)
            {
                StringBuilder strVar = new StringBuilder();
                StringBuilder strProcess = new StringBuilder();
                Ergodic(referenceCollector.data, ref strVar, ref strProcess);
                StringBuilder strFile = new StringBuilder();

                strFile.AppendLine("using System;");
                strFile.AppendLine("using UnityEngine;");
                strFile.AppendLine("using UnityEngine.UI;");

                strFile.AppendLine();
                strFile.AppendLine("namespace ET");
                strFile.AppendLine("{");


                strFile.AppendLine($"public class {go.name}Component:Entity");
                strFile.AppendLine("{");
                strFile.Append(strVar);
                strFile.AppendLine("public void Awake()");
                strFile.AppendLine("{");
                strFile.AppendLine("\tReferenceCollector rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();");

                strFile.Append(strProcess);
                strFile.AppendLine("}");
                strFile.AppendLine("}");
                strFile.AppendLine("}");
                string finalFileName = $"Assets/ModelView/UI/{go.name}/{go.name}Component.cs";
                DirectoryInfo di = new DirectoryInfo($"Assets/ModelView/UI/{go.name}");
                di.Create();
                using (var fs = new FileStream(finalFileName, FileMode.OpenOrCreate))
                {
                    if (!fs.CanWrite)
                    {
                        throw new System.Security.SecurityException("文件fileName=" + finalFileName + "是只读文件不能写入!");
                    }

                    var sw = new StreamWriter(fs);
                    sw.WriteLine(strFile);
                    sw.Dispose();
                    sw.Close();
                }
                Log.Debug($"成功生成{go.name}");
            }
            else
            {
                Log.Error("无法获取Panel上的ReferenceCollector组件");
            }
        }

        //创建system脚本
        private void CreateSystemScript(GameObject go)
        {
            //获取变量存储工具
            using (var sr = new StreamReader(@"Assets\Editor\CreaterScritEditor\UIComponentSystem.txt"))
            {
                DirectoryInfo di = new DirectoryInfo($"Assets/HotfixView/UI/{go.name}");
                di.Create();
                var finalFileName = $"Assets/HotfixView/UI/{go.name}/{go.name}ComponentSystem.cs";
                using (var fs = new FileStream(finalFileName, FileMode.OpenOrCreate))
                {
                    if (!fs.CanWrite)
                    {
                        throw new System.Security.SecurityException("文件fileName=" + finalFileName + "是只读文件不能写入!");
                    }
                    var sw = new StreamWriter(fs);
                    sw.Write(sr.ReadToEnd().Replace("*", go.name));
                    sw.Dispose();
                    sw.Close();
                }
            }
        }

        private void SignAssetBundleName()
        {
            foreach (var go in Selection.gameObjects)
            {
                string path = AssetDatabase.GetAssetPath(go);
                string prefabPath = AssetDatabase.GetAssetPath(go);
                AssetImporter importer = AssetImporter.GetAtPath(prefabPath);
                importer.assetBundleName = go.name + ".unity3d";
            }
        }
        //创建UIType
        private void CreateUIType(GameObject go)
        {
            FileStream fs = null;
            string filePath = "Assets/ModelView/Module/UI/UIType.cs";
            //将待写的入数据从字符串转换为字节数组  
            Encoding encoder = Encoding.UTF8;
            byte[] bytes = encoder.GetBytes($"\t\tpublic const string {go.name} = \"{go.name}\";\n" + "\t}\n}");
            if (!File.Exists(filePath))
            {
                Log.Error($"文件不存在{filePath}");
                return;
            }
            try
            {
                if (File.ReadAllText(filePath).Contains($"public const string {go.name} = \"{go.name}\";"))
                {
                    Log.Error($"UIType已经存在{go.name}");
                    return;
                }
                fs = File.OpenWrite(filePath);
                //设定书写的开始位置为文件的末尾  
                fs.Position = fs.Length - 4;
                //将待写入内容追加到文件末尾  
                fs.Write(bytes, 0, bytes.Length);
                Log.Debug("添加完成UIType:" + go.name);
            }
            catch (Exception ex)
            {
                Log.Error($"文件打开失败{ex.ToString()}");
            }
            fs.Close();
        }

        private void Ergodic(List<ReferenceCollectorData> datas, ref StringBuilder strVar, ref StringBuilder strProcess)
        {
            foreach (var data in datas)
            {
                string name = data.key;
                string type = "";
                if ((data.gameObject as GameObject).GetComponent<TextMeshProUGUI>() != null)
                {
                    type = "TextMeshProUGUI";
                }
                else if ((data.gameObject as GameObject).GetComponent<ToggleGroup>() != null)
                {
                    type = "ToggleGroupValue";
                }
                else if ((data.gameObject as GameObject).GetComponent<Text>() != null)
                {
                    type = "Text";
                }
                else if ((data.gameObject as GameObject).GetComponent<Button>() != null)
                {
                    type = "Button";
                }
                else if ((data.gameObject as GameObject).GetComponent<InputField>() != null)
                {
                    type = "InputField";
                }

                else if ((data.gameObject as GameObject).GetComponent<Image>() != null)
                {
                    type = "Image";
                }

                else if ((data.gameObject as GameObject).GetComponent<Slider>() != null)
                {
                    type = "Slider";
                }
                else
                {
                    type = "GameObject";
                }
                strVar.AppendLine($"\tpublic {type} M{name};");
                if (type == "GameObject")
                {
                    strProcess.AppendLine($"\tM{name}=rc.Get<GameObject>(\"{name}\");");
                }
                else
                {
                    strProcess.AppendLine($"\tM{name}=rc.Get<GameObject>(\"{name}\").GetComponent<{type}>();");
                }
            }
        }

    }
}