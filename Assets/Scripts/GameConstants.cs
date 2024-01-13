using Photon.Pun;
using ExitGames.Client.Photon;


// Game Constant is a static class used for mnanaging the global static variables
public static class GameConstants
{

    public const string SpawnPlayerNoGlobalValueConst = "SpawnPlayerNoGlobalValueConst";
    
    
    public static void SetGlobalIntValue(string key,int value)
    {
        // Use Custom Properties to update the global integer value
        Hashtable customProperties = new Hashtable();
        customProperties.Add(key, value);

        PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
    }
    
   public static int GetGlobalIntValue(string Intkey,int defaultValue=0)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(Intkey, out object value))
        {
            // Successfully retrieved the value from custom properties
            return (int)value;
        }
        else
        {
            // Key not found we will create that key Globally
            SetGlobalIntValue(Intkey,defaultValue);
            return defaultValue;
        }
    }
}
