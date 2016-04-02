using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Smurf.GlobalOffensive.BSP.Enums;
using Smurf.GlobalOffensive.BSP.Structs;
using Plane = Smurf.GlobalOffensive.BSP.Structs.Plane;


namespace Smurf.GlobalOffensive.BSP
{
    public class Bsp
    {
        #region VARIABLES
        private Header _header;
        private List<ushort[]> _edges;
        private Vector3[] _vertices;
        private Face[] _originalFaces;
        private Face[] _faces;
        private Plane[] _planes;
        private Brush[] _brushes;
        private Brushside[] _brushsides;
        private Node[] _nodes;
        private Leaf[] _leafs;
        private int[] _surfedges;
        private SurfFlag[] _textureInfo;
        private string _entityBuffer;

        //private World world;
        #endregion

        #region PROPERTIES
        public Header Header => _header;
        public List<ushort[]> Edges => _edges;
        public Vector3[] Vertices => _vertices;
        public Face[] OriginalFaces => _originalFaces;
        public Face[] Faces => _faces;
        public Plane[] Planes => _planes;
        public Brush[] Brushes => _brushes;
        public Brushside[] Brushsides => _brushsides;
        public Node[] Nodes => _nodes;
        public Leaf[] Leafs => _leafs;
        public int[] Surfedges => _surfedges;
        public SurfFlag[] TextureInfo => _textureInfo;
        public string EntityBuffer => _entityBuffer;

        #endregion

        #region CONSTRUCTORS
        public Bsp(Stream stream)
        {
            Load(stream);
        }
        public Bsp(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                Load(stream);
            }
        }
        #endregion

        #region METHODS - LOAD
        private void Load(Stream stream)
        {
            _header = GetHeader(stream);
            _edges = GetEdges(stream);
            _vertices = GetVertices(stream);
            _originalFaces = GetOriginalFaces(stream);
            _faces = GetFaces(stream);
            _planes = GetPlanes(stream);
            _surfedges = GetSurfedges(stream);
            _textureInfo = GetTextureInfo(stream);
            _brushes = GetBrushes(stream);
            _brushsides = GetBrushsides(stream);
            _entityBuffer = GetEntities(stream);
            _nodes = GetNodes(stream);
            _leafs = GetLeafs(stream);
            //LoadWorld();
        }
        private string GetEntities(Stream stream)
        {
            Lump lump = _header.lumps[(int)LumpType.LumpEntities];
            stream.Position = lump.Offset;
            byte[] data = UtilityReader.ReadBytes(stream, lump.Length);
            return System.Text.Encoding.ASCII.GetString(data);
        }
        private Header GetHeader(Stream stream)
        {
            Header header = new Header {ident = UtilityReader.ReadInt(stream)};

            UtilityReader.BigEndian = header.ident != 'V' + ('B' << 8) + ('S' << 16) + ('P' << 24);

            header.version = UtilityReader.ReadInt(stream);
            header.lumps = new Lump[64];
            for (int i = 0; i < header.lumps.Length; i++)
            {
                header.lumps[i] = new Lump
                {
                    Type = (LumpType) i,
                    Offset = UtilityReader.ReadInt(stream),
                    Length = UtilityReader.ReadInt(stream),
                    Version = UtilityReader.ReadInt(stream),
                    FourCc = UtilityReader.ReadInt(stream)
                };
            }
            header.mapRevision = UtilityReader.ReadInt(stream);
            return header;
        }
        private List<ushort[]> GetEdges(Stream stream)
        {
            List<ushort[]> edges = new List<ushort[]>();
            Lump lump = _header.lumps[(int)LumpType.LumpEdges];
            stream.Position = lump.Offset;

            for (int i = 0; i < (lump.Length / 2) / 2; i++)
            {
                ushort[] edge = new ushort[2];
                edge[0] = UtilityReader.ReadUShort(stream);
                edge[1] = UtilityReader.ReadUShort(stream);
                edges.Add(edge);
            }

            return edges;
        }
        private Vector3[] GetVertices(Stream stream)
        {
            Lump lump = _header.lumps[(int)LumpType.LumpVertexes];
            stream.Position = lump.Offset;
            Vector3[] vertices = new Vector3[(lump.Length / 3) / 4];

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3
                {
                    X = UtilityReader.ReadFloat(stream),
                    Y = UtilityReader.ReadFloat(stream),
                    Z = UtilityReader.ReadFloat(stream)
                };
            }

            return vertices;
        }
        private Face[] GetOriginalFaces(Stream stream)
        {
            Lump lump = _header.lumps[(int)LumpType.LumpOriginalfaces];
            stream.Position = lump.Offset;
            Face[] faces = new Face[lump.Length / 56];

            for (int i = 0; i < faces.Length; i++)
            {
                faces[i] = new Face
                {
                    planeNumber = UtilityReader.ReadUShort(stream),
                    side = UtilityReader.ReadByte(stream),
                    onNode = UtilityReader.ReadByte(stream),
                    firstEdge = UtilityReader.ReadInt(stream),
                    numEdges = UtilityReader.ReadShort(stream),
                    texinfo = UtilityReader.ReadShort(stream),
                    dispinfo = UtilityReader.ReadShort(stream),
                    surfaceFogVolumeID = UtilityReader.ReadShort(stream),
                    styles = new byte[4]
                };
                faces[i].styles[0] = UtilityReader.ReadByte(stream);
                faces[i].styles[1] = UtilityReader.ReadByte(stream);
                faces[i].styles[2] = UtilityReader.ReadByte(stream);
                faces[i].styles[3] = UtilityReader.ReadByte(stream);
                faces[i].lightOffset = UtilityReader.ReadInt(stream);
                faces[i].area = UtilityReader.ReadFloat(stream);
                faces[i].LightmapTextureMinsInLuxels = new int[2];
                faces[i].LightmapTextureMinsInLuxels[0] = UtilityReader.ReadInt(stream);
                faces[i].LightmapTextureMinsInLuxels[1] = UtilityReader.ReadInt(stream);
                faces[i].LightmapTextureSizeInLuxels = new int[2];
                faces[i].LightmapTextureSizeInLuxels[0] = UtilityReader.ReadInt(stream);
                faces[i].LightmapTextureSizeInLuxels[1] = UtilityReader.ReadInt(stream);
                faces[i].originalFace = UtilityReader.ReadInt(stream);
                faces[i].numPrims = UtilityReader.ReadUShort(stream);
                faces[i].firstPrimID = UtilityReader.ReadUShort(stream);
                faces[i].smoothingGroups = UtilityReader.ReadUInt(stream);
            }

            return faces;
        }
        private Face[] GetFaces(Stream stream)
        {
            Lump lump = _header.lumps[(int)LumpType.LumpFaces];
            stream.Position = lump.Offset;
            Face[] faces = new Face[lump.Length / 56];

            for (int i = 0; i < faces.Length; i++)
            {
                faces[i] = new Face
                {
                    planeNumber = UtilityReader.ReadUShort(stream),
                    side = UtilityReader.ReadByte(stream),
                    onNode = UtilityReader.ReadByte(stream),
                    firstEdge = UtilityReader.ReadInt(stream),
                    numEdges = UtilityReader.ReadShort(stream),
                    texinfo = UtilityReader.ReadShort(stream),
                    dispinfo = UtilityReader.ReadShort(stream),
                    surfaceFogVolumeID = UtilityReader.ReadShort(stream),
                    styles = new byte[4]
                };
                faces[i].styles[0] = UtilityReader.ReadByte(stream);
                faces[i].styles[1] = UtilityReader.ReadByte(stream);
                faces[i].styles[2] = UtilityReader.ReadByte(stream);
                faces[i].styles[3] = UtilityReader.ReadByte(stream);
                faces[i].lightOffset = UtilityReader.ReadInt(stream);
                faces[i].area = UtilityReader.ReadFloat(stream);
                faces[i].LightmapTextureMinsInLuxels = new int[2];
                faces[i].LightmapTextureMinsInLuxels[0] = UtilityReader.ReadInt(stream);
                faces[i].LightmapTextureMinsInLuxels[1] = UtilityReader.ReadInt(stream);
                faces[i].LightmapTextureSizeInLuxels = new int[2];
                faces[i].LightmapTextureSizeInLuxels[0] = UtilityReader.ReadInt(stream);
                faces[i].LightmapTextureSizeInLuxels[1] = UtilityReader.ReadInt(stream);
                faces[i].originalFace = UtilityReader.ReadInt(stream);
                faces[i].numPrims = UtilityReader.ReadUShort(stream);
                faces[i].firstPrimID = UtilityReader.ReadUShort(stream);
                faces[i].smoothingGroups = UtilityReader.ReadUInt(stream);
            }

            return faces;
        }
        private Plane[] GetPlanes(Stream stream)
        {
            Lump lump = _header.lumps[(int)LumpType.LumpPlanes];
            Plane[] planes = new Plane[lump.Length / 20];
            stream.Position = lump.Offset;

            for (int i = 0; i < planes.Length; i++)
            {
                planes[i] = new Plane();

                Vector3 normal = new Vector3
                {
                    X = UtilityReader.ReadFloat(stream),
                    Y = UtilityReader.ReadFloat(stream),
                    Z = UtilityReader.ReadFloat(stream)
                };

                planes[i].normal = normal;
                planes[i].distance = UtilityReader.ReadFloat(stream);
                planes[i].type = UtilityReader.ReadInt(stream);
            }

            return planes;
        }
        private Brush[] GetBrushes(Stream stream)
        {
            Lump lump = _header.lumps[(int)LumpType.LumpBrushes];
            Brush[] brushes = new Brush[lump.Length / 12];
            stream.Position = lump.Offset;

            for (int i = 0; i < brushes.Length; i++)
            {
                brushes[i] = new Brush
                {
                    firstside = UtilityReader.ReadInt(stream),
                    numsides = UtilityReader.ReadInt(stream),
                    contents = (ContentsFlag) UtilityReader.ReadInt(stream)
                };

            }

            return brushes;
        }
        private Brushside[] GetBrushsides(Stream stream)
        {
            Lump lump = _header.lumps[(int)LumpType.LumpBrushes];
            Brushside[] brushsides = new Brushside[lump.Length / 8];
            stream.Position = lump.Offset;

            for (int i = 0; i < brushsides.Length; i++)
            {
                brushsides[i] = new Brushside
                {
                    planenum = UtilityReader.ReadUShort(stream),
                    texinfo = UtilityReader.ReadShort(stream),
                    dispinfo = UtilityReader.ReadShort(stream),
                    bevel = UtilityReader.ReadShort(stream)
                };

            }

            return brushsides;
        }
        private int[] GetSurfedges(Stream stream)
        {

            Lump lump = _header.lumps[(int)LumpType.LumpSurfedges];
            int[] surfedges = new int[lump.Length / 4];
            stream.Position = lump.Offset;

            for (int i = 0; i < lump.Length / 4; i++)
            {
                surfedges[i] = UtilityReader.ReadInt(stream);
            }

            return surfedges;
        }
        private SurfFlag[] GetTextureInfo(Stream stream)
        {
            Lump lump = _header.lumps[(int)LumpType.LumpTexinfo];
            SurfFlag[] textureData = new SurfFlag[lump.Length / 72];
            stream.Position = lump.Offset;

            for (int i = 0; i < textureData.Length; i++)
            {
                stream.Position += 64;
                textureData[i] = (SurfFlag)UtilityReader.ReadInt(stream);
                stream.Position += 4;
            }

            return textureData;
        }
        private Node[] GetNodes(Stream stream)
        {
            Lump lump = _header.lumps[(int)LumpType.LumpNodes];
            Node[] nodesData = new Node[lump.Length / 32];
            stream.Position = lump.Offset;

            for (int i = 0; i < nodesData.Length; i++)
            {
                nodesData[i] = new Node
                {
                    planenum = UtilityReader.ReadInt(stream),
                    children = new int[2]
                };
                nodesData[i].children[0] = UtilityReader.ReadInt(stream);
                nodesData[i].children[1] = UtilityReader.ReadInt(stream);
                nodesData[i].mins = new short[3];
                nodesData[i].mins[0] = UtilityReader.ReadShort(stream);
                nodesData[i].mins[1] = UtilityReader.ReadShort(stream);
                nodesData[i].mins[2] = UtilityReader.ReadShort(stream);
                nodesData[i].maxs = new short[3];
                nodesData[i].maxs[0] = UtilityReader.ReadShort(stream);
                nodesData[i].maxs[1] = UtilityReader.ReadShort(stream);
                nodesData[i].maxs[2] = UtilityReader.ReadShort(stream);
                nodesData[i].firstface = UtilityReader.ReadUShort(stream);
                nodesData[i].numfaces = UtilityReader.ReadUShort(stream);
                nodesData[i].area = UtilityReader.ReadShort(stream);
                nodesData[i].paddding = UtilityReader.ReadShort(stream);
            }

            return nodesData;
        }
        private Leaf[] GetLeafs(Stream stream)
        {
            Lump lump = _header.lumps[(int)LumpType.LumpLeafs];
            Leaf[] leafData = new Leaf[lump.Length / 56];
            stream.Position = lump.Offset;

            for (int i = 0; i < leafData.Length; i++)
            {
                leafData[i] = new Leaf
                {
                    contents = (ContentsFlag) UtilityReader.ReadInt(stream),
                    cluster = UtilityReader.ReadShort(stream),
                    area = UtilityReader.ReadShort(stream),
                    flags = UtilityReader.ReadShort(stream),
                    mins = new short[3]
                };
                leafData[i].mins[0] = UtilityReader.ReadShort(stream);
                leafData[i].mins[1] = UtilityReader.ReadShort(stream);
                leafData[i].mins[2] = UtilityReader.ReadShort(stream);
                leafData[i].maxs = new short[3];
                leafData[i].maxs[0] = UtilityReader.ReadShort(stream);
                leafData[i].maxs[1] = UtilityReader.ReadShort(stream);
                leafData[i].maxs[2] = UtilityReader.ReadShort(stream);
                leafData[i].firstleafface = UtilityReader.ReadUShort(stream);
                leafData[i].numleaffaces = UtilityReader.ReadUShort(stream);
                leafData[i].firstleafbrush = UtilityReader.ReadUShort(stream);
                leafData[i].numleafbrushes = UtilityReader.ReadUShort(stream);
                leafData[i].leafWaterDataID = UtilityReader.ReadShort(stream);
            }

            return leafData;
        }
        #endregion

        #region METHODS - VISIBILITY
        public Leaf GetLeafForPoint(Vector3 point)
        {
            int node = 0;

            while (node >= 0)
            {
                var pNode = _nodes[node];
                var pPlane = _planes[pNode.planenum];

                var d = Vector3.Dot(point, pPlane.normal) - pPlane.distance;

                node = d > 0 ? pNode.children[0] : pNode.children[1];
            }

            return (
                (-node - 1) >= 0 && -node - 1 < _leafs.Length ?
                _leafs[-node - 1] :
                new Leaf() { area = -1, contents = ContentsFlag.ContentsEmpty }
            );
        }
        public bool IsVisible(Vector3 start, Vector3 end)
        {
            Vector3 vDirection = end - start;
            Vector3 vPoint = start;

            int iStepCount = (int)vDirection.Length();

            vDirection /= iStepCount;

            Leaf pLeaf = new Leaf() { area = -1 };

            while (iStepCount > 0)
            {
                vPoint += vDirection;

                pLeaf = GetLeafForPoint(vPoint);

                if (pLeaf.area != -1)
                {
                    if (
                        (pLeaf.contents & ContentsFlag.ContentsSolid) == ContentsFlag.ContentsSolid ||
                        (pLeaf.contents & ContentsFlag.ContentsDetail) == ContentsFlag.ContentsDetail)
                    {
                        break;
                    }
                }

                iStepCount--;
            }
            return (pLeaf.contents & ContentsFlag.ContentsSolid) != ContentsFlag.ContentsSolid;
        }
        #endregion
    }
}
