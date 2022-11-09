using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCreateSpriteTexture2D : MonoBehaviour
{
    public Image image;

    public Sprite sprite;
    public Texture2D texture;

    public Vector2 pivot = new Vector2(0.5f, 0.5f);

    public int index = 0;
    public string[] FileNames;

    void Start()
    {

        var bytes = File.ReadAllBytes(Path.Combine(Application.dataPath, "../", FileNames[index]));

        // new 的宽高意义不大，会在LoadImage后调整问图片的大小！
        texture = new Texture2D(0, 0);
        if (texture.LoadImage(bytes))
        {
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);
            image.sprite = sprite;
        }
    }

    [UnityEditor.MenuItem("Test/Create DynamicCreateSpriteTexture2D")]
    static void Create2()
    {
        GameObject.Find("Image").GetComponent<DynamicCreateSpriteTexture2D>().Create();
    }

    public void Create()
    {
        var bytes = sprite.texture.EncodeToJPG();

        var path = Time.time.ToString() + FileNames[index].ToString();

        File.WriteAllBytes(Application.dataPath + "/" + path, bytes);

        UnityEditor.AssetDatabase.ImportAsset("Assets/" + path);
    }


    public void Refresh()
    {
        OnDestroy();

        Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        if (sprite) Destroy(sprite);

        if (texture) Destroy(texture);
    }
}
