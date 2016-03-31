namespace Smurf.GlobalOffensive.Enums
{
    public enum SignonState
    {
        None = 0, // Menu
        Challenge = 1,
        Connected = 2, // Server welcome message?
        New = 3, // nfi when this is used
        PreSpawn = 4, // Selecting team
        Spawn = 5, // Spawn protected
        Full = 6, // Can move, shoot, etc.
        ChangingLevel = 7
    }
}