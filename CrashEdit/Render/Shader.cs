using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    public sealed partial class ShaderInfo
    {
        internal static readonly Dictionary<string, ShaderInfo> Infos = new()
        {
            { "line",       new ShaderInfo("line-static.vert", "default4.frag") },
            { "crash1",     new ShaderInfo("crash1-generic.vert", "crash1-generic.frag") },
            { "sprite",     new ShaderInfo("sprite-generic.vert", "sprite.frag") },
            { "generic",    new ShaderInfo("generic.vert", "sprite.frag") },
            { "screen",     new ShaderInfo("screen.vert", "screen.frag") },
            { "octree",     new ShaderInfo("octree.vert", "octree.frag", func: RenderOctree) },
        };

        public string VertShaderName { get; }
        public string FragShaderName { get; }

        public ShaderRenderFunc PreRenderFunc { get; }
        public ShaderRenderFunc RenderFunc { get; }

        internal ShaderInfo(string vert, string frag, ShaderRenderFunc? func = null, ShaderRenderFunc? prefunc = null)
        {
            VertShaderName = vert;
            FragShaderName = frag;
            RenderFunc = func ?? RenderDefault;
            PreRenderFunc = prefunc ?? PreRenderDefault;
        }
    }

    public partial class ShaderContext
    {
        public Shader GetShader(string name) => shaders[name];
        public int GetVertShader(string name) => vertshaders[name];
        public int GetFragShader(string name) => fragshaders[name];

        private readonly Dictionary<string, Shader> shaders = new();
        private readonly Dictionary<string, int> vertshaders = new();
        private readonly Dictionary<string, int> fragshaders = new();

        // init shaders. Needs a GL context to be active.
        // TODO see if shaders can be reused across contexts
        public ShaderContext()
        {
            if (shaders.Count != 0)
            {
                throw new Exception("Tried to re-init shaders.");
            }

            foreach (var info in ShaderInfo.Infos)
            {
                string vert = info.Value.VertShaderName;
                string frag = info.Value.FragShaderName;
                if (!vertshaders.ContainsKey(vert))
                {
                    var id = GL.CreateShader(ShaderType.VertexShader);
                    GL.ShaderSource(id, ResourceLoad.LoadTextFile("Render/Shaders/" + vert));
                    GL.CompileShader(id);

                    GL.GetShader(id, ShaderParameter.CompileStatus, out int s);
                    string log = GL.GetShaderInfoLog(id);
                    if (s == 0)
                        Console.WriteLine($"Error compiling shader {vert}:\n{log}");
                    else if (string.IsNullOrEmpty(log))
                        Console.WriteLine($"Shader {vert} compiled successfully.");
                    else
                        Console.WriteLine($"Shader {vert} compiled successfully:\n {log}");

                    vertshaders.Add(vert, id);
                }
                if (!fragshaders.ContainsKey(frag))
                {
                    var id = GL.CreateShader(ShaderType.FragmentShader);
                    GL.ShaderSource(id, ResourceLoad.LoadTextFile("Render/Shaders/" + frag));
                    GL.CompileShader(id);

                    GL.GetShader(id, ShaderParameter.CompileStatus, out int s);
                    string log = GL.GetShaderInfoLog(id);
                    if (s == 0)
                        Console.WriteLine($"Error compiling shader {frag}:\n{log}");
                    else if (string.IsNullOrEmpty(log))
                        Console.WriteLine($"Shader {frag} compiled successfully.");
                    else
                        Console.WriteLine($"Shader {frag} compiled successfully:\n {log}");

                    fragshaders.Add(frag, id);
                }
                var shader = new Shader(this, info.Key);
                shaders.Add(shader.Name, shader);
            }
        }

        public void KillShaders()
        {
            foreach (var shader in shaders.Values)
                shader.Dispose();
            foreach (var shader in vertshaders.Values)
                GL.DeleteShader(shader);
            foreach (var shader in fragshaders.Values)
                GL.DeleteShader(shader);
            shaders.Clear();
            vertshaders.Clear();
            fragshaders.Clear();
        }
    }

    public class Shader : IDisposable
    {
        public ShaderInfo Info { get; }
        public string Name { get; }
        public int VertShaderID { get; }
        public int FragShaderID { get; }

        public int ID { get; }

        // Create new shader program.
        public Shader(ShaderContext context, string name)
        {
            Name = name;
            Info = ShaderInfo.Infos[name];

            VertShaderID = context.GetVertShader(Info.VertShaderName);
            FragShaderID = context.GetFragShader(Info.FragShaderName);

            // Create the shader program, attach the vertex and fragment shaders and link the program.
            ID = GL.CreateProgram();
            GL.AttachShader(ID, VertShaderID);
            GL.AttachShader(ID, FragShaderID);
            GL.LinkProgram(ID);
            string log = GL.GetProgramInfoLog(ID);
            if (!string.IsNullOrEmpty(log))
                Console.WriteLine($"When linking {name}:\n {log}");
        }

        public void Dispose()
        {
            GL.DeleteProgram(ID);
            GC.SuppressFinalize(this);
        }

        private int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(ID, name);
        }

        public void UniformMat4(string name, ref Matrix4 mat) => GL.UniformMatrix4(GetUniformLocation(name), false, ref mat);
        public void UniformMat3(string name, ref Matrix3 mat) => GL.UniformMatrix3(GetUniformLocation(name), false, ref mat);
        public void UniformVec3(string name, ref Vector3 vec) => GL.Uniform3(GetUniformLocation(name), ref vec);
        public void UniformVec4(string name, ref Vector4 vec) => GL.Uniform4(GetUniformLocation(name), ref vec);
        public void UniformVec4(string name, ref Color4 col) => GL.Uniform4(GetUniformLocation(name), col.R, col.G, col.B, col.A);
        public void UniformFloat(string name, float val) => GL.Uniform1(GetUniformLocation(name), val);
        public void UniformInt(string name, int val) => GL.Uniform1(GetUniformLocation(name), val);
        public void UniformBool(string name, bool val) => GL.Uniform1(GetUniformLocation(name), Convert.ToInt32(val));

        public void Render(RenderInfo ri, VAO vao)
        {
            GL.ValidateProgram(ID);
            string log = GL.GetProgramInfoLog(ID);
            if (!string.IsNullOrEmpty(log))
                Console.WriteLine($"When validating {Name}:\n {log}");
            GL.UseProgram(ID);
            Info.PreRenderFunc(this, ri, vao);
            Info.RenderFunc(this, ri, vao);
        }
    }
}
