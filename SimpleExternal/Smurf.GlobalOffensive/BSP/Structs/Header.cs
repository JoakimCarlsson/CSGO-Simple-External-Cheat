namespace Smurf.GlobalOffensive.BSP.Structs
{
    public struct Header
    {
        public int ident;
        public int version;
        public Lump[] lumps;
        public int mapRevision;
    }
}
