//using System;
//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using System.Reflection;
//using System.Text;
//using Object = UnityEngine.Object;

//    public static class ActiveObjectHelper
//    {

//        public static void ClearProjectSearchField()
//        {
//            try
//            {
//                var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.ProjectBrowser");
//                if (type == null)
//                {
//                    Debug.LogError("type is null");
//                    return;
//                }
//                Object projectView = EditorWindow.GetWindow(type);
//                if (projectView == null)
//                {
//                    Debug.LogError("projectView is null");
//                    return;
//                }
//                MethodInfo method = type.GetMethod("ClearSearch", BindingFlags.NonPublic | BindingFlags.Instance);
//                if (method == null)
//                {
//                    Debug.LogError("method is null");
//                    return;
//                }
//                method.Invoke(projectView, new object[] { });
//            }
//            catch (Exception e)
//            {
//            }
//        }

//        public static void WriteFile(string filePath, string content)
//        {
//            FileInfo t = new FileInfo(filePath);
//            if (!t.Directory.Exists)
//                t.Directory.Create();
//            FileStream fs = new FileStream(filePath, FileMode.Create);
//            //获得字节数组
//            byte[] data = new UTF8Encoding().GetBytes(content);
//            //开始写入
//            fs.Write(data, 0, data.Length);
//            //清空缓冲区、关闭流
//            fs.Flush();
//            fs.Close();
//        }

//        public static string ReadFile(string filePath)
//        {
//            FileInfo t = new FileInfo(filePath);
//            if (!t.Directory.Exists)
//                t.Directory.Create();
//            string s = "";
//            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
//            byte[] byteData = new byte[fs.Length];
//            //返回读取的文件内容真实字节数
//            int length = fs.Read(byteData, 0, byteData.Length);
//            if (length > 0)
//            {
//                //将数组转以UTF-8字符集编码格式的字符串
//                s = Encoding.UTF8.GetString(byteData);
//            }
//            fs.Close();
//            return s;
//        }

//        public static Texture GetTextureByType(Type type)
//        {
//            var activeObjectType = ActiveObjectConfig.allTypes.Find(x => x.type == type);
//            if (activeObjectType != null)
//                return activeObjectType.gUIContent.image;
//            return null;
//        }

//        public static ActiveObjectType GetTypeByName(string typeName)
//        {
//            return ActiveObjectConfig.allTypes.Find(x => x.typeName.Equals(typeName));
//        }

//        public static string GetNameByType(Type type)
//        {
//            var activeObjectType = ActiveObjectConfig.allTypes.Find(x => x.type == type);
//            if (activeObjectType != null)
//                return activeObjectType.typeName;
//            return string.Empty;
//        }
//    }