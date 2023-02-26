using Cofdream.ToolKitEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ReorderableList : VisualElement, INotifyValueChanged<int>
{

    // Factory class, required to expose this custom control to UXML
    public new class UxmlFactory : UxmlFactory<ReorderableList, UxmlTraits> { }

    // Traits class
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    private int _size = 5;

    public ReorderableList()
    {
        styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Devlop/UIToolKit/Scripts/Editor/ReorderableList/ReorderableSet.uss"));

        this.ClearClassList();
        this.AddToClassList("root");

        DrawList(this);
    }

    private void DrawList(VisualElement root)
    {
        root.Add(GetHead());
        root.Add(GetContent());
    }
    private VisualElement GetHead()
    {
        var head = new VisualElement();
        head.name = "head";

        head.Add(GetTitle());
        head.Add(GetOptions());

        return head;
    }

    private VisualElement GetTitle()
    {
        var label = new Label("我的集合");
        //label.ClearClassList();
        //label.AddToClassList("head-lable");
        return label;
    }

    private VisualElement GetOptions()
    {
        var options = new VisualElement();
        options.name = "list-view__options";

        var add = new Button();
        options.Add(add);
        add.name = "list-view__add-button";
        add.tooltip = "Add to the list";
        add.text = "+";
        add.clicked += () =>
        {
            _size++;
            //options.Q<IntegerField>("size").value = _size;
        };


        var remove = new Button();
        options.Add(remove);
        remove.name = "list-view__remove-button";
        remove.tooltip = "Remove selection from the list";
        remove.text = "-";
        remove.clicked += () =>
        {
            _size--;
            //options.Q<IntegerField>("size").value = _size;
        };

        //var sizeIntInput = new UnityEditor.UIElements.IntegerField();
        //sizeIntInput.name = "size";
        //options.Add(sizeIntInput);

        //sizeIntInput.ClearClassList();
        //sizeIntInput.AddToClassList("head-size");
        //sizeIntInput.value = _size;

        //sizeIntInput.RegisterValueChangedCallback((value) =>
        //{
        //    _size = value.newValue;
        //});


        return options;
    }


    private VisualElement GetContent()
    {
        var content = new VisualElement();
        content.name = "list-view__content-container";

        for (int i = 0; i < _size; i++)
        {
            DrawElement(content);
        }

        return content;
    }

    VisualElement temp;
    int index = 0;
    private void DrawElement(VisualElement root)
    {
        var item = new VisualElement();
        {
            item.name = "list-view__reorderable-item";

            item.AddToClassList("unity-collection-view__item");
            item.AddToClassList("unity-list-view__item");
            item.AddToClassList("unity-list-view__reorderable-item");

            item.RegisterCallback<PointerDownEvent>((eventData) =>
            {
                if (temp != null)
                {
                    temp.RemoveFromClassList("unity-collection-view__item--selected");
                }
                item.AddToClassList("unity-collection-view__item--selected");
                temp = item;

                Debug.Log(item.Q<Label>()?.text);
            });

            var handle = new VisualElement();
            {
                handle.name = "list-view__reorderable-item-handle";

                for (int i = 0; i < 2; i++)
                {
                    var bar = new VisualElement();
                    bar.name = "list-view__reorderable-item-handle-bar";
                    handle.Add(bar);
                }
            }
            item.Add(handle);

            var container = new VisualElement();
            {
                container.name = "list-view__reorderable-item-container";

                container.Add(new Label($"{index++}"));
                container.Add(new Label($"数据AA"));
                container.Add(new Label($"数据BBBBBBBBBB"));
            }
            item.Add(container);
        }
        root.Add(item);
    }

    public void SetValueWithoutNotify(int newValue)
    {
        Debug.Log(newValue);
    }

    int _value;
    public int value { get => _value; set => _value = value; }
}