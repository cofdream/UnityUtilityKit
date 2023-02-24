using Cofdream.ToolKitEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ReorderableList : VisualElement
{

    // Factory class, required to expose this custom control to UXML
    public new class UxmlFactory : UxmlFactory<ReorderableList, UxmlTraits> { }

    // Traits class
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    private int _size = 5;

    List<int> _datas;


    Vector2 _contentSize;

    public ReorderableList()
    {
        _datas = new List<int>();

        styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Devlop/UIToolKit/Scripts/Editor/ReorderableList/ReorderableSet.uss"));

        this.ClearClassList();
        this.AddToClassList("root");

        DrawList(this);
    }

    private void DrawList(VisualElement root)
    {
        root.Add(DrawHead());

        //DrawElement(root);
    }
    private VisualElement DrawHead()
    {
        var head = new VisualElement();

        head.style.backgroundImage = EditorGUIUtilityExtensions.LoadIconRequired("OL box");
        head.name = "Head";
        head.ClearClassList();
        head.AddToClassList("head");

        head.Add(GetTitle());

        head.Add(GetOptions());

        return head;
    }

    private VisualElement GetTitle()
    {
        var label = new Label("我的集合");
        label.ClearClassList();
        label.AddToClassList("head-lable");
        return label;
    }

    private VisualElement GetOptions()
    {
        var options = new VisualElement();
        options.ClearClassList();
        options.AddToClassList("options");

        //options.Add(new Button());

        var add = new Button();
        options.Add(add);
        add.style.backgroundImage = EditorGUIUtilityExtensions.LoadIconRequired("Toolbar Plus");
        add.ClearClassList();
        add.AddToClassList("options-button");
        add.clicked += () =>
        {
            _size++;
            options.Q<IntegerField>("size").value = _size;
        };


        var remove = new Button();
        options.Add(remove);
        remove.style.backgroundImage = EditorGUIUtilityExtensions.LoadIconRequired("Toolbar Minus");
        remove.ClearClassList();
        remove.AddToClassList("options-button");
        remove.clicked += () =>
        {
            _size--;
            options.Q<IntegerField>("size").value = _size;
        };


        var temp = new Button();
        options.Add(temp);


        var sizeIntInput = new UnityEditor.UIElements.IntegerField();
        sizeIntInput.name = "size";
        options.Add(sizeIntInput);

        sizeIntInput.ClearClassList();
        sizeIntInput.AddToClassList("head-size");
        sizeIntInput.value = _size;

        sizeIntInput.RegisterValueChangedCallback((value) =>
        {
            _size = value.newValue;
        });


        return options;
    }


    private void DrawElement(VisualElement root)
    {
        var bg = new VisualElement();
        bg.name = "element background";
        bg.style.backgroundImage = EditorGUIUtilityExtensions.LoadIconRequired("OL box");
        bg.Clear();
        bg.AddToClassList("background");

        //bg.style.width = _contentSize.x;
        //bg.style.height = _contentSize.y;

        root.Add(bg);
    }
}