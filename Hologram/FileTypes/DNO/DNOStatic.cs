using ModLib;
using OpenTK.Mathematics;
using Hologram.Objects;
using System.Text;

namespace Hologram.FileTypes.DNO
{
    /// <summary>
    /// Physics collision FileType
    /// </summary>
    public partial class DNO
    {
        public static DNO Parse(string fileLocation, uint tempFileOffset)
        {
            DNO dno = new DNO();
            using (ModFile file = ModFile.Open(fileLocation))
            {
                uint fileLength = file.ReadUint(true);

                if (file.ReadString(11) != "SERIALISED") { throw new Exception("Does not appear to be a DNO file!"); }

                uint version = file.ReadUint(true);

                ushort nameCount = file.ReadUshort(true); // Pretty sure

                uint notSure = file.ReadUint(true);

                for (int i = 0; i < nameCount; i++)
                {
                    Logger.Log(new LogSeg(file.ReadString(file.ReadInt(true)), ConsoleColor.Gray));
                }

                ushort unknown = file.ReadUshort(true); // Not sure, (2)

                uint one = file.ReadUint(true); // Not sure, (1)

                for (int i = 0; i < unknown; i++)
                {
                    file.Seek(0x40, SeekOrigin.Current);
                }

                ushort numItems = file.ReadUshort(true); // Not sure, (2)

                uint vectorCount = file.ReadUint(true); // Not sure, (F)

                uint unknownVal = file.ReadUint(true);
                for (int i = 0; i < numItems; i++)
                { // LinearDamping, AngularDamping, TransformIndex, MotionType, AddToScene, PostAnimState, mMassMode, HasController, Type, IntertiaMode, TerrainLayer, Inertia[0], Inertia[1], Inertia[2], numLockedVerts, 
                    float linearDamping = file.ReadFloat(true);
                    float angularDamping = file.ReadFloat(true);
                    ushort nameIndex = file.ReadUshort(true);
                    ushort transformIndex = file.ReadUshort(true);
                    byte motionType = file.ReadByte();
                    byte addToScene = file.ReadByte();
                    byte postAnimState = file.ReadByte();
                    byte massMode = file.ReadByte();
                    byte hasController = file.ReadByte();
                    byte thisType = file.ReadByte();
                    byte inertiaMode = file.ReadByte();
                    byte terrainLayer = file.ReadByte();
                    Vector3 inertia = new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                    uint anotherUnknown = file.ReadUint(true);
                    uint numLockedVerts = file.ReadUint(true);
                    for (int lockedCount = 0; lockedCount < numLockedVerts; lockedCount++)
                    {
                        // Looks like I need to seek back to "anotherUnknown" and read the values from there, but I think this means there can only be 2 lockedVerts???
                        //ushort lockedIndex = file.ReadUshort(true); // Not sure
                    }
                    byte stopRotation = file.ReadByte();
                    byte scalable = file.ReadByte();
                    byte isPhantom = file.ReadByte();
                    byte floatHack = file.ReadByte();
                    byte aiAvoidable = file.ReadByte();
                    ushort customCom = file.ReadUshort(true);
                    ushort localCom = file.ReadUshort(true);
                    uint unknownXY = file.ReadUint(true); // None of this seems to be referenced
                    uint unknownXY1 = file.ReadUint(true);
                    ushort unknownXY2 = file.ReadUshort(true);
                    uint platformMask = file.ReadUint(true);
                    uint collisionLayers = file.ReadUint(true);
                    uint collisionLayersFilter = file.ReadUint(true);
                    ushort clothId = file.ReadUshort(true);
                    ushort fluidType = file.ReadUshort(true);
                }

                //uint aZero = file.ReadUint(true);
                ushort aTwo = file.ReadUshort(true);
                uint aZero2 = file.ReadUint(true);
                ushort aEight = file.ReadUshort(true);
                ushort thisCount = file.ReadUshort(true);
                uint aTwo2 = file.ReadUint(true);

                for (int i = 0; i < thisCount; i++)
                { // 34 bytes
                    Vector3 extents = new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                    float shellRadius = file.ReadFloat(true);
                    int meshDepth = file.ReadInt(true); // Should be applied conditionally
                    ushort bodyIndex = file.ReadUshort(true);
                    short transformIndex = file.ReadShort(true);
                    ushort meshIndex = file.ReadUshort(true);
                    ushort terrainExtra = file.ReadUshort(true);
                    byte terrainType = file.ReadByte();
                    byte geometryFlags = file.ReadByte();
                    byte material = file.ReadByte();
                    byte unknownXXX = file.ReadByte();
                    byte isInstance = file.ReadByte();
                    byte isSecondaryShape = file.ReadByte();
                    Console.WriteLine(extents);
                }

                uint aZeroA = file.ReadUint(true);
                ushort aOneA = file.ReadUshort(true);
                uint aZeroB = file.ReadUint(true);
                ushort aOneB = file.ReadUshort(true);
                uint aZeroC = file.ReadUint(true);
                ushort aOneC = file.ReadUshort(true);

                ushort type = file.ReadUshort(true);

                uint ROAR = file.ReadUint(true);

                if (type == 9) { throw new Exception("Type == 9"); }

                ushort mainMeshCount = file.ReadUshort(true); // It's either this one that's the main count or "something2"
                dno.Meshes = new DMesh[mainMeshCount];
                for (int meshId = 0; meshId < mainMeshCount; meshId++)
                {
                    DMesh thisMesh = new DMesh();
                    ushort something1 = file.ReadUshort(true);
                    ushort something2 = file.ReadUshort(true);

                    int vertexCount = file.ReadInt(true);
                    uint nodeCount = file.ReadUint(true);
                    int quadCount = file.ReadInt(true);
                    uint bucketCount = file.ReadUint(true);
                    uint materialCount = file.ReadUint(true);

                    StringBuilder str = new StringBuilder();

                    thisMesh.Vertices = new Vector3[vertexCount];
                    for (int i = 0; i < vertexCount; i++)
                    {
                        thisMesh.Vertices[i] = new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                    }

                    thisMesh.Quads = new DQuad[quadCount];
                    for (int i = 0; i < quadCount; i++)
                    {
                        DQuad quad = new DQuad();

                        quad.Quad.vert1 = file.ReadUshort(true);
                        quad.Quad.vert2 = file.ReadUshort(true);
                        quad.Quad.vert3 = file.ReadUshort(true);
                        quad.Quad.vert4 = file.ReadUshort(true);

                        quad.MinMaxFlags = file.ReadUshort(true);

                        thisMesh.Quads[i] = quad;
                    }

                    thisMesh.Buckets = new DBucket[bucketCount];
                    for (int i = 0; i < bucketCount; i++)
                    {
                        DBucket bucket = new DBucket();
                        bucket.Min = new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                        bucket.MinInt = file.ReadInt(true);
                        bucket.Max = new Vector3(file.ReadFloat(true), file.ReadFloat(true), file.ReadFloat(true));
                        bucket.MaxInt = file.ReadInt(true);
                        thisMesh.Buckets[i] = bucket;
                    }

                    for (int i = 0; i < quadCount; i++)
                    {
                        ushort triA = file.ReadUshort(true);
                        ushort triB = file.ReadUshort(true);
                    }

                    for (int i = 0; i < quadCount; i++)
                    {
                        thisMesh.Quads[i].Material = file.ReadByte();
                    }

                    thisMesh.Nodes = new DNode[nodeCount];
                    for (int i = 0; i < nodeCount; i++)
                    {
                        DNode node = new DNode();

                        node.FrontPlane = file.ReadFloat(true);
                        node.BackPlane = file.ReadFloat(true);
                        node.BackIndex = file.ReadUshort(true);
                        node.NormalIndex = file.ReadByte();

                        thisMesh.Nodes[i] = node;
                    }

                    thisMesh.Materials = new DMaterial[materialCount];
                    for (int i = 0; i < materialCount; i++)
                    {
                        DMaterial material = new DMaterial();
                        material.Material = file.ReadByte();
                        material.TerrainType = file.ReadByte();
                        material.TerrainExtra = file.ReadUshort(true);
                        material.AiData = file.ReadByte();
                    }

                    dno.Meshes[meshId] = thisMesh;
                }

                //File.WriteAllText(@"A:\1wizardofoza_secondary.obj", str.ToString());

                Console.WriteLine("Done reading at: {0}", file.Position);
            }
            return dno;
        }
    }
}
