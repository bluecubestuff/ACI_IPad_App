[System.Serializable]
public class Tools
{
    public int iD;
    public string name;
    public bool unlocked;
    public enum CleanType
    {
        CLOTH = 0,
        SOAPWATER,
        WATERHOSE,
        TOWEL,
        CHEMICAL,
        VACUUM,
        BROOM,
        WARNINGSIGN,
        SANITIZER,
        BRUSH,
        MOP
    }
    public CleanType cleanType;
    public string imageSource;
    public string prefabLocation;
}
