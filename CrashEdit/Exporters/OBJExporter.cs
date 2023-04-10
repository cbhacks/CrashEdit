using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using OpenTK;

namespace CrashEdit.Exporters;

public class OBJExporter
{
    private const string DEFAULT_MATERIAL = "default";
    
    class Material
    {
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;
        public float highlight;
        public Bitmap texture;
    }

    class Face
    {
        public int V1;
        public int V2;
        public int V3;
        public int? V4;
        public string material;
        public int? UV1;
        public int? UV2;
        public int? UV3;
        public int? UV4;
    }

    class Vertex
    {
        public Vector3 position;
        public Vector3 color;
    }
    
    private Dictionary <string, Material> materials = new Dictionary <string, Material> ();
    private List <Vertex> vertices = new List <Vertex> ();
    private List <Face> faces = new List <Face> ();
    private List <Vector2> uvs = new List <Vector2> ();

    public OBJExporter ()
    {
        // create a default material for everything that is not textured
        this.materials[DEFAULT_MATERIAL] = new Material
        {
            ambient = Vector3.One,
            diffuse = Vector3.One,
            highlight = 0.0f,
            specular = Vector3.Zero,
            texture = null
        };
    }

    /// <summary>
    /// Adds a texture with the given name to the obj
    /// </summary>
    /// <param name="name"></param>
    /// <param name="texture">Texture data</param>
    /// <returns>The identifier for the texture in the obj export</returns>
    public string AddTexture (string name, Bitmap texture)
    {
        string identifier = $"tex{name}";

        this.materials [identifier] = new Material
        {
            ambient = Vector3.One,
            diffuse = Vector3.One,
            highlight = 0.0f,
            specular = Vector3.Zero,
            texture = texture
        };

        return identifier;
    }

    /// <summary>
    /// Adds a new vertex to the output
    /// </summary>
    /// <param name="position"></param>
    /// <param name="color"></param>
    public void AddVertex (Vector3 position, Vector3 color)
    {
        this.vertices.Add (
            new Vertex
            {
                position = position,
                color = color
            }
        );
    }

    /// <summary>
    /// Adds a simple face using the given vertices
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    /// <param name="material"></param>
    /// <param name="uv1"></param>
    /// <param name="uv2"></param>
    /// <param name="uv3"></param>
    public void AddFace (int v1, int v2, int v3, string material = null, Vector2? uv1 = null, Vector2? uv2 = null, Vector2? uv3 = null)
    {
        // add uv coordinates to the lists first
        int? uv1id = null;
        int? uv2id = null;
        int? uv3id = null;

        if (uv1 != uv2 || uv1 != uv3)
            throw new InvalidDataException ("UVs must all be null or all have values");

        if (uv1 is not null)
        {
            uv1id = this.uvs.Count;
            uv2id = this.uvs.Count + 1;
            uv3id = this.uvs.Count + 2;

            this.uvs.Add (uv1.Value);
            this.uvs.Add (uv2.Value);
            this.uvs.Add (uv3.Value);
        }
        
        this.faces.Add (
            new Face
            {
                material = material ?? DEFAULT_MATERIAL,
                V1 = v1,
                V2 = v2,
                V3 = v3,
                UV1 = uv1id,
                UV2 = uv2id,
                UV3 = uv3id
            }
        );
    }

    /// <summary>
    /// Adds a simple face using the given vertices
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    /// <param name="material"></param>
    /// <param name="uv1"></param>
    /// <param name="uv2"></param>
    /// <param name="uv3"></param>
    public void AddFace (int v1, int v2, int v3, int v4, string material = null, Vector2? uv1 = null, Vector2? uv2 = null, Vector2? uv3 = null, Vector2? uv4 = null)
    {
        // add uv coordinates to the lists first
        int? uv1id = null;
        int? uv2id = null;
        int? uv3id = null;
        int? uv4id = null;

        if (
            (uv1 is null && (uv2 is not null || uv3 is not null || uv4 is not null)) ||
            (uv2 is null && (uv1 is not null || uv3 is not null || uv4 is not null)) ||
            (uv3 is null && (uv1 is not null || uv2 is not null || uv4 is not null)) ||
            (uv4 is null && (uv1 is not null || uv2 is not null || uv3 is not null))
        )
            throw new InvalidDataException ("UVs must all be null or all have values");

        if (uv1 is not null)
        {
            uv1id = this.uvs.Count;
            uv2id = this.uvs.Count + 1;
            uv3id = this.uvs.Count + 2;
            uv4id = this.uvs.Count + 3;

            this.uvs.Add (uv1.Value);
            this.uvs.Add (uv2.Value);
            this.uvs.Add (uv3.Value);
            this.uvs.Add (uv4.Value);
        }
        
        this.faces.Add (
            new Face
            {
                material = material ?? DEFAULT_MATERIAL,
                V1 = v1,
                V2 = v2,
                V3 = v3,
                V4 = v4,
                UV1 = uv1id,
                UV2 = uv2id,
                UV3 = uv3id,
                UV4 = uv4id
            }
        );
    }
    
    /// <summary>
    /// Creates a new face with it's own vertices and uv coordinates
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="c3"></param>
    /// <param name="material"></param>
    /// <param name="uv2"></param>
    /// <param name="uv3"></param>
    /// <param name="uv1"></param>
    public void AddFace (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 c1, Vector3 c2, Vector3 c3, string material = null, Vector2? uv1 = null, Vector2? uv2 = null, Vector2? uv3 = null)
    {
        int v1id = this.vertices.Count;
        int v2id = this.vertices.Count + 1;
        int v3id = this.vertices.Count + 2;
        int? uv1id = null;
        int? uv2id = null;
        int? uv3id = null;

        if (
            (uv1 is null && (uv2 is not null || uv3 is not null)) ||
            (uv2 is null && (uv1 is not null || uv3 is not null)) ||
            (uv3 is null && (uv1 is not null || uv2 is not null))
        )
            throw new InvalidDataException ("UVs must all be null or all have values");

        if (uv1 is not null)
        {
            uv1id = this.uvs.Count;
            uv2id = this.uvs.Count + 1;
            uv3id = this.uvs.Count + 2;

            this.uvs.Add (uv1.Value);
            this.uvs.Add (uv2.Value);
            this.uvs.Add (uv3.Value);
        }

        this.vertices.Add (
            new Vertex
            {
                position = v1,
                color = c1
            }
        );
        this.vertices.Add (
            new Vertex
            {
                position = v2,
                color = c2
            }
        );
        this.vertices.Add (
            new Vertex
            {
                position = v3,
                color = c3
            }
        );
        
        this.faces.Add (
            new Face
            {
                material = material,
                V1 = v1id,
                V2 = v2id,
                V3 = v3id,
                UV1 = uv1id,
                UV2 = uv2id,
                UV3 = uv3id
            }
        );
    }

    /// <summary>
    /// Creates a new face with it's own vertices and uv coordinates
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    /// <param name="v4"></param>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="c3"></param>
    /// <param name="c4"></param>
    /// <param name="material"></param>
    /// <param name="uv2"></param>
    /// <param name="uv3"></param>
    /// <param name="uv1"></param>
    /// <param name="uv4"></param>
    public void AddFace (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 c1, Vector3 c2, Vector3 c3, Vector3 c4, string material = null, Vector2? uv1 = null, Vector2? uv2 = null, Vector2? uv3 = null, Vector2? uv4 = null)
    {
        int v1id = this.vertices.Count;
        int v2id = this.vertices.Count + 1;
        int v3id = this.vertices.Count + 2;
        int v4id = this.vertices.Count + 3;
        int? uv1id = null;
        int? uv2id = null;
        int? uv3id = null;
        int? uv4id = null;

        if (
            (uv1 is null && (uv2 is not null || uv3 is not null || uv4 is not null)) ||
            (uv2 is null && (uv1 is not null || uv3 is not null || uv4 is not null)) ||
            (uv3 is null && (uv1 is not null || uv2 is not null || uv4 is not null)) ||
            (uv4 is null && (uv1 is not null || uv2 is not null || uv3 is not null))
        )
            throw new InvalidDataException ("UVs must all be null or all have values");

        if (uv1 is not null)
        {
            uv1id = this.uvs.Count;
            uv2id = this.uvs.Count + 1;
            uv3id = this.uvs.Count + 2;
            uv4id = this.uvs.Count + 3;

            this.uvs.Add (uv1.Value);
            this.uvs.Add (uv2.Value);
            this.uvs.Add (uv3.Value);
            this.uvs.Add (uv4.Value);
        }

        this.vertices.Add (
            new Vertex
            {
                position = v1,
                color = c1
            }
        );
        this.vertices.Add (
            new Vertex
            {
                position = v2,
                color = c2
            }
        );
        this.vertices.Add (
            new Vertex
            {
                position = v3,
                color = c3
            }
        );
        this.vertices.Add (
            new Vertex
            {
                position = v4,
                color = c4
            }
        );
        
        this.faces.Add (
            new Face
            {
                material = material,
                V1 = v1id,
                V2 = v2id,
                V3 = v3id,
                V4 = v4id,
                UV1 = uv1id,
                UV2 = uv2id,
                UV3 = uv3id,
                UV4 = uv4id
            }
        );
    }
    
    private void ExportMaterials (string path, string modelname)
    {
        // first write all the textures to disk
        // then write the mtl file
        using MemoryStream stream = new MemoryStream ();
        using StreamWriter writer = new StreamWriter (stream);

        writer.WriteLine ("# CrashEdit exported material");
        
        // write all the materials
        foreach (KeyValuePair <string, Material> material in this.materials)
        {
            writer.WriteLine ("newmtl {0}", material.Key);
            writer.WriteLine (
                "Ka {0} {1} {2}",
                material.Value.ambient.X.ToString(CultureInfo.InvariantCulture),
                material.Value.ambient.Y.ToString(CultureInfo.InvariantCulture),
                material.Value.ambient.Z.ToString(CultureInfo.InvariantCulture)
            );
            writer.WriteLine (
                "Kd {0} {1} {2}",
                material.Value.diffuse.X.ToString(CultureInfo.InvariantCulture),
                material.Value.diffuse.Y.ToString(CultureInfo.InvariantCulture),
                material.Value.diffuse.Z.ToString(CultureInfo.InvariantCulture)
            );
            writer.WriteLine (
                "Ks {0} {1} {2}",
                material.Value.specular.X.ToString(CultureInfo.InvariantCulture),
                material.Value.specular.Y.ToString(CultureInfo.InvariantCulture),
                material.Value.specular.Z.ToString(CultureInfo.InvariantCulture)
            );
            writer.WriteLine (
                "Ns {0}",
                material.Value.highlight.ToString(CultureInfo.InvariantCulture)
            );

            if (material.Value.texture is null)
                continue;
            
            writer.WriteLine(
                "map_Kd {0}.bmp",
                material.Key
            );
            
            // write the bitmap to a file too
            material.Value.texture.Save (path + Path.DirectorySeparatorChar + material.Key + ".bmp");
        }

        writer.Flush ();
        
        // material file finally written, save it to disk too
        File.WriteAllBytes (path + Path.DirectorySeparatorChar + modelname + ".mtl", stream.ToArray ());
    }

    public void Export (string path, string modelname)
    {
        // first write the material file
        ExportMaterials (path, modelname);
        
        using MemoryStream stream = new MemoryStream();
        using StreamWriter writer = new StreamWriter (stream);

        writer.WriteLine ("# CrashEdit exported model");
        writer.WriteLine ("mtllib {0}.mtl", modelname);
        writer.WriteLine ("# Vertices");

        foreach (Vertex vertex in vertices)
        {
            writer.WriteLine (
                "v {0} {1} {2} {3} {4} {5}",
                vertex.position.X.ToString(CultureInfo.InvariantCulture),
                vertex.position.Y.ToString(CultureInfo.InvariantCulture),
                vertex.position.Z.ToString(CultureInfo.InvariantCulture),
                vertex.color.X.ToString(CultureInfo.InvariantCulture),
                vertex.color.Y.ToString(CultureInfo.InvariantCulture),
                vertex.color.Z.ToString(CultureInfo.InvariantCulture)
            );
        }
        
        // write any uvs we have
        writer.WriteLine ();
        writer.WriteLine ("# UVs");

        foreach (Vector2 uv in uvs)
        {
            writer.WriteLine(
                "vt {0} {1}",
                uv.X.ToString(CultureInfo.InvariantCulture), uv.Y.ToString(CultureInfo.InvariantCulture)
            );
        }
        
        // finally write the faces
        writer.WriteLine ();
        writer.WriteLine ("# Faces with textures");

        string lastmaterial = null;

        // by default use the default material
        writer.WriteLine ("usemtl {0}", DEFAULT_MATERIAL);

        foreach (Face face in faces.OrderBy (x => x.material))
        {
            if (lastmaterial != face.material)
            {
                writer.WriteLine ("usemtl {0}", face.material);

                lastmaterial = face.material;
            }
            
            // write face information, UVs must all be null or have value
            // at the same time, so this check is safe
            if (face.UV1 is null)
            {
                if (face.V4 is null)
                {
                    writer.WriteLine (
                        "f {0} {1} {2}",
                        face.V1 + 1,
                        face.V2 + 1,
                        face.V3 + 1
                    );
                }
                else
                {
                    writer.WriteLine (
                        "f {0} {1} {2} {3}",
                        face.V1 + 1,
                        face.V2 + 1,
                        face.V3 + 1,
                        face.V4 + 1
                    );
                }
            }
            else
            {
                if (face.V4 is null)
                {
                    writer.WriteLine (
                        "f {0}/{3} {1}/{4} {2}/{5}",
                        face.V1 + 1,
                        face.V2 + 1,
                        face.V3 + 1,
                        face.UV1 + 1,
                        face.UV2 + 1,
                        face.UV3 + 1
                    );
                }
                else
                {
                    writer.WriteLine (
                        "f {0}/{4} {1}/{5} {2}/{6} {3}/{7}",
                        face.V1 + 1,
                        face.V2 + 1,
                        face.V3 + 1,
                        face.V4 + 1,
                        face.UV1 + 1,
                        face.UV2 + 1,
                        face.UV3 + 1,
                        face.UV4 + 1
                    );
                }
            }
        }
        
        writer.Flush ();

        // obj file ready, write to the destination
        File.WriteAllBytes (path + Path.DirectorySeparatorChar + modelname + ".obj", stream.ToArray ());
    }
}