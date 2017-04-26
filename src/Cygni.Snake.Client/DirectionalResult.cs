namespace Cygni.Snake.Client
{
    /// <summary>
    /// These are supposed to be in descending order of preference, when adding new values, please stay true to this rule.
    /// </summary>
    public enum DirectionalResult
    {
        Food = 1,
        Nothing = 2,
        TailNibble = 3,
        Danger = 4,
        Death = 5,
    }
}