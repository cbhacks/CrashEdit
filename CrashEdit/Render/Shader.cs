using OpenTK.Graphics.OpenGL4;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed partial class ShaderInfo
    {
        internal static readonly Dictionary<string, ShaderInfo> Infos = new()
        {
            { "test", new ShaderInfo("test.vert", "default4.frag", func: RenderTest) },
            { "axes", new ShaderInfo("axes.vert", "default4.frag") },
            { "line", new ShaderInfo("line-static.vert", "default4.frag") },
            { "line-model", new ShaderInfo("line-model.vert", "default4.frag", func: RenderLineModel) },
            { "crash1", new ShaderInfo("crash1-generic.vert", "crash1-generic.frag") },
            { "line-usercolor", new ShaderInfo("line-usercolor.vert", "default4.frag") },
            { "box-model", new ShaderInfo("box-model.vert", "default4.frag") },
            { "sprite-debug", new ShaderInfo("sprite.vert", "sprite.frag") },
            { "sprite", new ShaderInfo("sprite-generic.vert", "sprite.frag", func: RenderSprite) },
            { "generic", new ShaderInfo("generic.vert", "sprite.frag") }
        };

        public string VertShaderName { get; }
        public string FragShaderName { get; }

        public ShaderRenderFunc PreRenderFunc { get; }
        public ShaderRenderFunc RenderFunc { get; }

        internal ShaderInfo(string vert, string frag, ShaderRenderFunc func = null, ShaderRenderFunc prefunc = null)
        {
            VertShaderName = vert;
            FragShaderName = frag;
            RenderFunc = func;
            PreRenderFunc = prefunc;
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
                if (!vertshaders.ContainsKey(info.Value.VertShaderName))
                {
                    var id = GL.CreateShader(ShaderType.VertexShader);
                    GL.ShaderSource(id, ResourceLoad.LoadTextFile("Render/Shaders/" + info.Value.VertShaderName));
                    GL.CompileShader(id);

                    GL.GetShader(id, ShaderParameter.CompileStatus, out int s);
                    if (s == 0)
                    {
                        Console.WriteLine(GL.GetShaderInfoLog(id));
                    }

                    vertshaders.Add(info.Value.VertShaderName, id);
                }
                if (!fragshaders.ContainsKey(info.Value.FragShaderName))
                {
                    var id = GL.CreateShader(ShaderType.FragmentShader);
                    GL.ShaderSource(id, ResourceLoad.LoadTextFile("Render/Shaders/" + info.Value.FragShaderName));
                    GL.CompileShader(id);

                    GL.GetShader(id, ShaderParameter.CompileStatus, out int s);
                    if (s == 0)
                    {
                        Console.WriteLine(GL.GetShaderInfoLog(id));
                    }

                    fragshaders.Add(info.Value.FragShaderName, id);
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
        }

        public void Dispose()
        {
            GL.DeleteProgram(ID);
        }

        public void UniformMat4(string name, ref Matrix4 mat) => GL.UniformMatrix4(GL.GetUniformLocation(ID, name), false, ref mat);
        public void UniformMat3(string name, ref Matrix3 mat) => GL.UniformMatrix3(GL.GetUniformLocation(ID, name), false, ref mat);
        public void UniformVec3(string name, ref Vector3 vec) => GL.Uniform3(GL.GetUniformLocation(ID, name), vec.X, vec.Y, vec.Z);
        public void UniformVec4(string name, ref Vector4 vec) => GL.Uniform4(GL.GetUniformLocation(ID, name), vec.X, vec.Y, vec.Z, vec.W);
        public void UniformVec4(string name, ref Color4 col) => GL.Uniform4(GL.GetUniformLocation(ID, name), col.R, col.G, col.B, col.A);
        public void UniformFloat(string name, float val) => GL.Uniform1(GL.GetUniformLocation(ID, name), val);
        public void UniformInt(string name, int val) => GL.Uniform1(GL.GetUniformLocation(ID, name), val);
        public void UniformBool(string name, bool val) => GL.Uniform1(GL.GetUniformLocation(ID, name), Convert.ToInt32(val));

        public void Render(RenderInfo ri, VAO vao)
        {
            GL.UseProgram(ID);
            if (Info.PreRenderFunc == null)
                ShaderInfo.PreRenderDefault(this, ri, vao);
            else
                Info.PreRenderFunc(this, ri, vao);
            if (Info.RenderFunc == null)
                ShaderInfo.RenderDefault(this, ri, vao);
            else
                Info.RenderFunc(this, ri, vao);
        }
    }
}
