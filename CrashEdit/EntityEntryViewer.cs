using Crash;
using Crash.Game;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class EntityEntryViewer : GLControl
    {
        private EntityEntry entry;
        private int midx;
        private int midy;
        private int midz;
        private int range;
        private int rotx;
        private int roty;
        private bool mouseleft;
        private bool mouseright;
        private int mousex;
        private int mousey;

        public EntityEntryViewer(EntityEntry entry)
        {
            this.entry = entry;
            ZoomAll();
            this.mouseleft = false;
            this.mouseright = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    mouseleft = true;
                    break;
                case MouseButtons.Right:
                    mouseright = true;
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    mouseleft = false;
                    break;
                case MouseButtons.Right:
                    mouseright = false;
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseleft)
            {
                rotx += e.X - mousex;
                roty += e.Y - mousey;
                rotx %= 360;
                if (roty > 90)
                    roty = 90;
                if (roty < -90)
                    roty = -90;
                Invalidate();
            }
            else if (mouseright)
            {
                range -= e.Y - mousey;
                if (range < 1)
                    range = 1;
                Invalidate();
            }
            mousex = e.X;
            mousey = e.Y;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.A:
                case Keys.Z:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            const int speed = 16;
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    midz -= speed;
                    break;
                case Keys.Down:
                    midz += speed;
                    break;
                case Keys.Left:
                    midx -= speed;
                    break;
                case Keys.Right:
                    midx += speed;
                    break;
                case Keys.A:
                    midy += speed;
                    break;
                case Keys.Z:
                    midy -= speed;
                    break;
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            MakeCurrent();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.AlphaTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.AlphaFunc(AlphaFunction.Greater,0);
            GL.Viewport(Location,Size);
            GL.ClearColor(Color.Black);
            GL.ClearDepth(short.MaxValue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-0.0001,+0.0001,-0.0001,+0.0001,0.0001,short.MaxValue);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0,0,-1);
            GL.Scale(1.0 / range,1.0 / range,1.0 / range);
            GL.Rotate(roty,1,0,0);
            GL.Rotate(rotx,0,1,0);
            GL.Translate(-midx,-midy,-midz);
            foreach (Entity entity in entry.Entities)
            {
                if (entity.Name != null)
                {
                    RenderEntity(entity);
                }
            }
            SwapBuffers();
        }

        private void RenderEntity(Entity entity)
        {
            if (entity.Positions.Count == 1)
            {
                EntityPosition position = entity.Positions[0];
                GL.PushMatrix();
                GL.Translate(position.X,position.Y,position.Z);
                switch (entity.Type)
                {
                    case 0x22:
                        RenderBox(entity.Subtype.Value);
                        break;
                    case null:
                        break;
                    default:
                        GL.PointSize(5);
                        GL.Color3(Color.White);
                        GL.Begin(BeginMode.Points);
                        GL.Vertex3(0,0,0);
                        GL.End();
                        break;
                }
                GL.PopMatrix();
            }
            else
            {
                GL.LineWidth(3);
                GL.Color3(Color.Blue);
                GL.Begin(BeginMode.LineStrip);
                foreach (EntityPosition position in entity.Positions)
                {
                    GL.Vertex3(position.X,position.Y,position.Z);
                }
                GL.End();
            }
        }

        private void RenderBox(int subtype)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Color3(Color.White);
            LoadBoxSideTexture(subtype);
            GL.PushMatrix();
            RenderBoxFace();
            GL.Rotate(90,0,1,0);
            RenderBoxFace();
            GL.Rotate(90,0,1,0);
            RenderBoxFace();
            GL.Rotate(90,0,1,0);
            RenderBoxFace();
            GL.PopMatrix();
            LoadBoxTopTexture(subtype);
            GL.PushMatrix();
            RenderBoxFace();
            GL.Rotate(90,1,0,0);
            RenderBoxFace();
            GL.Rotate(-180,1,0,0);
            RenderBoxFace();
            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        private void RenderBoxFace()
        {
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0,0);
            GL.Vertex3(-50,+50,50);
            GL.TexCoord2(1,0);
            GL.Vertex3(+50,+50,50);
            GL.TexCoord2(1,1);
            GL.Vertex3(+50,-50,50);
            GL.TexCoord2(0,1);
            GL.Vertex3(-50,-50,50);
            GL.End();
        }

        private void LoadTexture(Bitmap image)
        {
            BitmapData data = image.LockBits(new Rectangle(Point.Empty,image.Size),ImageLockMode.ReadOnly,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            try
            {
                GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,image.Width,image.Height,0,OpenTK.Graphics.OpenGL.PixelFormat.Bgra,PixelType.UnsignedByte,data.Scan0);
                GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)TextureMinFilter.Nearest);
            }
            finally
            {
                image.UnlockBits(data);
            }
        }

        private void LoadBoxTopTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // TNT
                    LoadTexture(Resources.TNTTopTexture);
                    break;
                case 2: // Normal
                case 3: // Arrow
                case 4: // Checkpoint
                case 6: // Apple
                case 8: // Life
                case 9: // Mask
                case 10: // Question Mark
                    LoadTexture(Resources.BoxTexture);
                    break;
                case 5: // Iron
                case 7: // Activator
                case 15: // Iron Arrow
                    LoadTexture(Resources.IronBoxTexture);
                    break;
                case 18: // Nitro
                    LoadTexture(Resources.NitroTopTexture);
                    break;
                case 23: // Bodyslam
                    LoadTexture(Resources.BodyslamBoxTexture);
                    break;
                case 24: // Detonator
                    LoadTexture(Resources.DetonatorBoxTopTexture);
                    break;
                default:
                    LoadTexture(Resources.UnknownBoxTopTexture);
                    break;
            }
        }

        private void LoadBoxSideTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // TNT
                    LoadTexture(Resources.TNTTexture);
                    break;
                case 2: // Normal
                    LoadTexture(Resources.BoxTexture);
                    break;
                case 3: // Arrow
                    LoadTexture(Resources.ArrowBoxTexture);
                    break;
                case 4: // Checkpoint
                    LoadTexture(Resources.CheckpointTexture);
                    break;
                case 5: // Iron
                    LoadTexture(Resources.IronBoxTexture);
                    break;
                case 6: // Apple
                    LoadTexture(Resources.AppleBoxTexture);
                    break;
                case 7: // Activator
                    LoadTexture(Resources.ActivatorBoxTexture);
                    break;
                case 8: // Life
                    LoadTexture(Resources.LifeBoxTexture);
                    break;
                case 9: // Mask
                    LoadTexture(Resources.MaskBoxTexture);
                    break;
                case 10: // Question Mark
                    LoadTexture(Resources.QuestionMarkBoxTexture);
                    break;
                case 15: // Iron Arrow
                    LoadTexture(Resources.IronArrowBoxTexture);
                    break;
                case 18: // Nitro
                    LoadTexture(Resources.NitroTexture);
                    break;
                case 23: // Bodyslam
                    LoadTexture(Resources.BodyslamBoxTexture);
                    break;
                case 24: // Detonator
                    LoadTexture(Resources.DetonatorBoxTexture);
                    break;
                default:
                    LoadTexture(Resources.UnknownBoxTexture);
                    break;
            }
        }

        private void ZoomAll()
        {
            int minx = short.MaxValue;
            int miny = short.MaxValue;
            int minz = short.MaxValue;
            int maxx = short.MinValue;
            int maxy = short.MinValue;
            int maxz = short.MinValue;
            foreach (Entity entity in entry.Entities)
            {
                if (entity.Name == null)
                {
                    continue;
                }
                foreach (EntityPosition position in entity.Positions)
                {
                    minx = Math.Min(minx,position.X);
                    miny = Math.Min(miny,position.Y);
                    minz = Math.Min(minz,position.Z);
                    maxx = Math.Max(maxx,position.X);
                    maxy = Math.Max(maxy,position.Y);
                    maxz = Math.Max(maxz,position.Z);
                }
            }
            midx = (maxx + minx) / 2;
            midy = (maxy + miny) / 2;
            midz = (maxz + minz) / 2;
            range = 1;
            range = Math.Max(range,midx - minx);
            range = Math.Max(range,midy - miny);
            range = Math.Max(range,midz - minz);
            range += 400;
            rotx = 0;
            roty = 0;
        }
    }
}
