using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Search.Providers;
using UnityEditor.SearchService;
using UnityEngine;
using Object = UnityEngine.Object;

[SceneSearchEngine]
class TestSceneFilterEngine : ISceneSearchEngine
{
    public string name => "My Custom Engine";

    public void BeginSession(ISearchContext context)
    {
        Debug.Log("BeginSession Done." + context.ToString());
    }

    public void EndSession(ISearchContext context)
    {
        Debug.Log("EndSession Done." + context.ToString());
    }

    public void BeginSearch(ISearchContext context, string query)
    {
        Debug.Log("BeginSearch Done." + context.ToString());
    }

    public void EndSearch(ISearchContext context)
    {
        Debug.Log("EndSearch Done." + context.ToString());
    }

    public bool Filter(ISearchContext context, string query, HierarchyProperty objectToFilter)
    {
        Debug.Log("Filter Done." );
        Debug.Log(context.ToString());
        Debug.Log(query);
        Debug.Log(objectToFilter.ToString());

        var instanceId = objectToFilter.instanceID;
        var obj = Object.FindObjectsOfType<GameObject>().FirstOrDefault(o => o.GetInstanceID() == instanceId);
        return obj != null && obj.name.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0;
    }


    [SceneQueryEngineFilter("lights", new[] { "=", "!=", "<", ">", "<=", ">=" })]
    internal static int? MeshRendererAffectedByLightsSceneFilter(GameObject go)
    {
        if (!go.TryGetComponent<MeshRenderer>(out var meshRenderer))
            return null;

        if (!meshRenderer.isVisible)
            return null;

        var lightEffectCount = 0;
        var gp = go.transform.position;
        foreach (var light in Object.FindObjectsOfType<Light>())
        {
            if (!light.isActiveAndEnabled)
                continue;

            var lp = light.transform.position;

            var distance = Vector3.Distance(gp, lp);
            if (distance > light.range)
                continue;

            if (light.type == UnityEngine.LightType.Spot)
            {
                var da = Vector3.Dot(light.transform.forward, lp - gp);
                if (da > Mathf.Deg2Rad * light.spotAngle)
                    continue;
            }

            lightEffectCount++;
        }

        return lightEffectCount;
    }
}