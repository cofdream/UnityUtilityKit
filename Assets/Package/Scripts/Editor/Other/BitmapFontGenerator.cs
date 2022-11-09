using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace other
{
    /// <summary>
    /// 用于创建位图文字
    /// </summary>
    public sealed class BitmapFontGenerator : EditorWindow
    {
        //[MenuItem("DATools/Other/Font/创建BitmapFont")]
        private static void OpenBitmapFontEditor()
        {
            BitmapFontGenerator win = EditorWindow.GetWindow<BitmapFontGenerator>();
            win.minSize = new Vector2(598, 300);
            // win.maxSize = new Vector2(600,301);
            win.Show();
        }

        private string _bitmapFullPath;
        private string _bitmapFolder;
        private string _fontName = "";
        private UnityEngine.Object _sprite;

        private UnityEngine.Object[] _fontItems;
        private CharacterGUIData[] _characterGuiDatas;
        private CharacterInfo[] _ci;
        private float _bitmapW;
        private float _bitmapH;
        private Material _fontMat;
        private bool _valid;//是否合法，不可包含字符/ \ : * " < > | ？

        Regex regExp = new Regex("[~!@#$%^&*()=+[\\]{}''\";:/?.,><`|！·￥…—（）\\-、；：。，》《]");

        private void OnGUI()
        {
            GUILayout.Label("注意：生成好字体后，如果需要换行功能，请在fontsetting里设置lineSpacing为字体高度");
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("选择字体对应的位图");
            _sprite = EditorGUILayout.ObjectField(_sprite, typeof(Texture), false);

            if (_sprite != null)
            {
                _bitmapW = (_sprite as Texture).width;
                _bitmapH = (_sprite as Texture).height;
            }

            if (GUILayout.Button(new GUIContent("设置字体参数")))
            {
                _valid = true;
                _bitmapFullPath = AssetDatabase.GetAssetPath(_sprite);

                if (!string.IsNullOrEmpty(_bitmapFullPath))
                {
                    _bitmapFolder = Path.GetDirectoryName(_bitmapFullPath);
                    _fontName = Path.GetFileNameWithoutExtension(_bitmapFullPath);
                    //
                    _fontItems = AssetDatabase.LoadAllAssetsAtPath(_bitmapFullPath);

                    _characterGuiDatas = new CharacterGUIData[_fontItems.Length - 1];

                    for (int i = 0; i < _fontItems.Length - 1; i++)
                    {
                        _characterGuiDatas[i] = new CharacterGUIData();
                        var name = SplitName(_fontItems[i + 1].name);
                        _characterGuiDatas[i].UnicodeStr = name;
                        var nameValid = !regExp.IsMatch(name); //含有非法字符;
                        _characterGuiDatas[i].valid = nameValid;
                        if (!nameValid)
                            _valid = false;
                    }
                }
                else
                {
                    Debug.LogError("没有指定需要制作字体的图集！");
                }
            }

            GUILayout.Label("设置字体名称:");
            _fontName = GUILayout.TextField(_fontName, 1200, GUILayout.Width(120));

            if (GUILayout.Button(new GUIContent("创建字体")))
            {
                if (!_valid)
                {
                    EditorUtility.DisplayDialog("Error", "字体包含违法字符，请检查", "OK");
                    return;
                }
                if (!string.IsNullOrEmpty(_bitmapFolder))
                {
                    if (_characterGuiDatas != null)
                    {
                        CreatFontSetting();
                    }
                    else
                    {
                        Debug.LogError("字体数据为空！");
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            if (_characterGuiDatas == null) return;

            //
            for (int i = 0; i < _characterGuiDatas.Length; i++)
            {
                UnityEngine.Object obj = _fontItems[i + 1];
                CharacterGUIData data = _characterGuiDatas[i];
                data.index = i;
                data.ItemObj = obj;
                data.CharacterInfo = new CharacterInfo();
                SetFontCfgParameterItem(i, data);
            }
        }


        /// <summary>
        /// 将输入的文字转换为unicode
        /// </summary>
        /// <param name="srcText"></param>
        /// <returns></returns>
        private int ConvertStringToUnicode(string srcText)
        {
            string dst = "";
            char[] src = srcText.ToCharArray();
            for (int i = 0; i < src.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
                string str = bytes[1].ToString("X2") + bytes[0].ToString("X2");
                dst += str;
            }

            try
            {
                return Convert.ToInt32(dst, 16);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return 0;
            }
        }

        private void SetFontCfgParameterItem(int index, CharacterGUIData data)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("第{0}个字体", data.index), GUILayout.Width(80));
            EditorGUILayout.ObjectField(data.ItemObj, typeof(Sprite), false);
            GUILayout.Label("名称：");
            GUILayout.TextField(data.ItemObj.name);
            GUILayout.Label("对应的字体文字：");
            if (string.IsNullOrEmpty(data.UnicodeStr))
                data.UnicodeStr = data.ItemObj.name;
            data.UnicodeStr = EditorGUILayout.TextField(data.UnicodeStr);
            EditorGUILayout.EndHorizontal();
        }

        private string SplitName(string value)
        {
            string fontName = "";
            string[] tmp = value.Split('_');
            if (tmp.Length > 1)
            {
                fontName = tmp[1];
            }
            return fontName;
        }

        //创建fontsetting
        private void CreatFontSetting()
        {
            CharacterGUIData[] finalSettings = FilterSetting();

            _ci = new CharacterInfo[finalSettings.Length];
            int linespace;
            for (int i = 0; i < finalSettings.Length; i++)
            {
                CharacterGUIData cgd = finalSettings[i];
                Sprite sp = cgd.ItemObj as Sprite;
                Rect spRect = sp.rect;
                _ci[i] = new CharacterInfo();
                _ci[i].index = ConvertStringToUnicode(cgd.UnicodeStr);
                //_ci[i].uvBottomLeft = new Vector2(spRect.x/_bitmapW, spRect.y/_bitmapH);
                //_ci[i].uvTopRight = new Vector2((spRect.x + spRect.width)/_bitmapW, (spRect.y +spRect.height)/_bitmapH);
                //_ci[i].minX = 0;
                //_ci[i].maxX = (int)spRect.width;
                //_ci[i].minY = (int)-spRect.height/2;
                //_ci[i].maxY = (int)spRect.height/2;
                //_ci[i].advance = (int) spRect.width;

                CharacterInfo info = _ci[i];
                info.index = ConvertStringToUnicode(cgd.UnicodeStr);
                info.uvBottomLeft = new Vector2(spRect.x / _bitmapW, spRect.y / _bitmapH);
                info.uvTopRight = new Vector2((spRect.x + spRect.width) / _bitmapW, (spRect.y + spRect.height) / _bitmapH);

                info.minX = 0;
                info.minY = -(int)spRect.height / 2;   // 这样调出来的效果是ok的，原理未知  
                info.glyphWidth = (int)spRect.width;
                info.glyphHeight = (int)spRect.height; // 同上，不知道为什么要用负的，可能跟unity纹理uv有关  
                info.advance = (int)spRect.width;

                _ci[i] = info;
                linespace = (int)spRect.height;
            }

            string fntPath = _bitmapFolder + "/" + _fontName + ".fontsettings";
            //AssetDatabase.CreateAsset(_fnt, fntPath);
            Font fnt = AssetDatabase.LoadAssetAtPath<Font>(fntPath);
            if (fnt == null)
            {
                fnt = new Font();
                AssetDatabase.CreateAsset(fnt, fntPath);
                AssetDatabase.ImportAsset(fntPath);
            }
            fnt.characterInfo = _ci;
            CreateFontMaterial(fnt);
            EditorUtility.SetDirty(fnt);

            AssetDatabase.SaveAssets();
        }

        private List<CharacterGUIData> tmpDatas = new List<CharacterGUIData>();
        /// <summary>
        /// 过滤掉没有指定文本的
        /// </summary>
        private CharacterGUIData[] FilterSetting()
        {
            //int len = 0;
            tmpDatas.Clear();
            foreach (CharacterGUIData data in _characterGuiDatas)
            {
                if (!string.IsNullOrEmpty(data.UnicodeStr))
                {
                    tmpDatas.Add(data);
                }
            }
            return tmpDatas.ToArray();
        }

        private void CreateFontMaterial(Font fnt)
        {
            string fontMatPath = _bitmapFolder + "/" + _fontName + ".mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(fontMatPath);
            if (mat == null)
            {
                _fontMat = new Material(Shader.Find("UI/Default"));
                _fontMat.mainTexture = _sprite as Texture;
                fnt.material = _fontMat;
                AssetDatabase.CreateAsset(_fontMat, fontMatPath);
            }
            else
            {
                mat.mainTexture = _sprite as Texture;
            }
        }

    }


    public class CharacterGUIData
    {
        public int index;
        public UnityEngine.Object ItemObj;
        public string UnicodeStr;
        public CharacterInfo CharacterInfo;
        public bool valid;
    }

}