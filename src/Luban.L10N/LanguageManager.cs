using System.Collections.Generic;

public class LanguageManager //: Singleton<LanguageManager>
{
    private Dictionary<string, string> _languageDictionary;

    public void Load(string str)
    {
        string[] lines = str.Split("\n");
        _languageDictionary?.Clear();
        _languageDictionary ??= new Dictionary<string, string>(lines.Length);
        foreach (string line in lines)
        {
            string[] keyValue = line.Split('=');
            if (keyValue.Length == 1) continue;
            _languageDictionary.Add(keyValue[0], keyValue[1]);
        }
    }

    public bool Has(string key)
    {
        return _languageDictionary.ContainsKey(key);
    }

    public string Get(string key)
    {
        if (_languageDictionary.TryGetValue(key, out var str)) return str;
        return key;
    }

    public string Get(string key, object param1)
    {
        if (!_languageDictionary.TryGetValue(key, out var str)) str = key;
        return string.Format(str, param1);
    }

    public string Get(string key, object param1, object param2)
    {
        if (!_languageDictionary.TryGetValue(key, out var str)) str = key;
        return string.Format(str, param1, param2);
    }

    public string Get(string key, object param1, object param2, object param3)
    {
        if (!_languageDictionary.TryGetValue(key, out var str)) str = key;
        return string.Format(str, param1, param2, param3);
    }

    public string Get(string key, params object[] param)
    {
        if (!_languageDictionary.TryGetValue(key, out var str)) str = key;
        return string.Format(str, param);
    }
}
