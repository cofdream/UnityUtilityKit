//#if UNITY_EDITOR
//using UnityEngine;
//using TMPro;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;

//[CreateAssetMenu(menuName = "DropDownPopulator")]
//public class PopulateDropDownSO : ScriptableObject
//{
//    public string DropdownName;

//    [SerializeField] private List<c_OptionData> countries = new List<c_OptionData>();

//    [ContextMenu("Populate CountryOptionData With Names")]
//    public void PopulateCountryOptionDataWithNames()
//    {
//        var nameList = GetCountryNames();

//        for (int i = 0; i < nameList.Count; i++)
//        {
//            c_OptionData data = new c_OptionData();
//            data.Text = nameList[i];
//            countries.Add(data);
//        }
//    }

//    [ContextMenu("Populate Dropdown With Countries")]
//    public void PopulateWithCountryNames()
//    {
//        if (string.IsNullOrEmpty(DropdownName))
//        {
//            Debug.Log("DropdownName field is empty");
//            return;
//        }
//        var dropdownGO = GameObject.Find(DropdownName);
//        TMP_Dropdown dropdown;

//        var result = false;
//        if (dropdownGO != null)
//        {
//            var text = "Dropdown found. Parent name : " + dropdownGO.transform.parent.name;
//            result = UnityEditor.EditorUtility.DisplayDialog("Dropdown Found", text, "Continue", "Cancel");
//        }
//        else
//        {
//            UnityEditor.EditorUtility.DisplayDialog("", "Couldnt find dropdown named : " + "dropdown", "Ok");
//            return;
//        }
//        if (result)
//        {
//            dropdown = dropdownGO.GetComponent<TMP_Dropdown>();

//            dropdown.ClearOptions();
//            dropdown.AddOptions(CreateOptionDataFromCustom());
//        }
//    }

//    private List<TMP_Dropdown.OptionData> CreateOptionDataFromCustom()
//    {
//        var optionDataList = new List<TMP_Dropdown.OptionData>();

//        foreach (var item in countries)
//        {
//            var data = new TMP_Dropdown.OptionData(item.Text, item.Flag);
//            optionDataList.Add(data);
//        }

//        return optionDataList;
//    }

//    private List<string> GetCountryNames()
//    {
//        var list = new List<string>();
//        var countries = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(c =>
//        new RegionInfo(c.LCID)).Distinct().OrderBy(c => c.EnglishName).ToList();

//        list.Add("None");
//        for (int i = 0; i < countries.Count; i++)
//        {
//            list.Add(countries[i].DisplayName);
//        }

//        return list;
//    }

//    [System.Serializable]
//    public struct c_OptionData
//    {
//        public string Text;
//        public Sprite Flag;
//    }
//}
//#endif