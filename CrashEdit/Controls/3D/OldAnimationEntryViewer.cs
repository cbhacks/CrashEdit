using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldAnimationEntryViewer : GLViewer
    {
        private NSF nsf;

        private int frame_id;
        private int cur_frame;
        private int eid_anim;
        private int interi;
        private int interp = 2;
        private bool colored;
        private float r, g, b;
        private bool collisionenabled;
        private bool texturesenabled = true;
        private bool normalsenabled = true;
        private bool interp_startend = false;
        private int cullmode = 0;

        private VAO vaoModel;
        private Vector4[] buf_vtx;
        private Vector3[] buf_nor;
        private Color4[] buf_col;
        private int buf_idx;

        protected override bool UseGrid => true;

        public OldAnimationEntryViewer(NSF nsf, int anim_eid, int frame, bool colored, Dictionary<int, int> texturechunks)
        {
            this.nsf = nsf;
            collisionenabled = Settings.Default.DisplayFrameCollision;
            eid_anim = anim_eid;
            frame_id = frame;
            // this.texturechunks = texturechunks;
            this.colored = colored;
            cur_frame = 0;
        }

        public OldAnimationEntryViewer(NSF nsf, int anim_eid, bool colored, Dictionary<int, int> texturechunks)
        {
            this.nsf = nsf;
            collisionenabled = Settings.Default.DisplayFrameCollision;
            eid_anim = anim_eid;
            frame_id = -1;
            // this.texturechunks = texturechunks;
            this.colored = colored;
            cur_frame = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            vaoModel = new VAO(render.ShaderContext, "anim_c1", PrimitiveType.Triangles);
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                /*
                int mdlX = 0x1000;
                int mdlY = 0x1000;
                int mdlZ = 0x1000;
                if (model != null)
                {
                    mdlX = model.ScaleX;
                    mdlY = model.ScaleY;
                    mdlZ = model.ScaleZ;
                }
                yield return new Position(0,0,0);
                foreach (OldFrame frame in frames)
                {
                    foreach (OldFrameVertex vertex in frame.Vertices)
                    {
                        int x = vertex.X-128 + frame.XOffset;
                        int y = vertex.Y-128 + frame.YOffset;
                        int z = vertex.Z-128 + frame.ZOffset;
                        x = x*mdlX>>10;
                        y = y*mdlY>>10;
                        z = z*mdlZ>>10;
                        yield return new Position(x,y,z);
                    }
                }*/
                yield return new Position(0, 0, 0);
            }
        }

        protected override void Render()
        {
            base.Render();

            buf_idx = 0;

            var anim = nsf.GetEntry<OldAnimationEntry>(eid_anim);
            if (anim != null)
            {
                cur_frame = frame_id == -1 ? (int)(render.CurrentFrame / 2 % anim.Frames.Count) : frame_id;
                RenderFrame(nsf.GetEntry<OldAnimationEntry>(eid_anim).Frames[cur_frame]);
            }
        }

        protected override void ActualRunLogic()
        {
            base.ActualRunLogic();
            if (KPress(Keys.C)) collisionenabled = !collisionenabled;
            if (KPress(Keys.N)) normalsenabled = !normalsenabled;
            if (KPress(Keys.T)) texturesenabled = !texturesenabled;
            if (KPress(Keys.U)) cullmode = ++cullmode % 3;
        }

        private void RenderFrame(OldFrame frame, OldFrame frame2 = null)
        {
            var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
            if (model != null)
            {
                if (frame2 != null && (frame2.ModelEID != frame.ModelEID || frame.Vertices.Count != frame2.Vertices.Count)) frame2 = null;
                render.Projection.UserMat3.Row0 = new Vector3(-4096, 2048, 4096) / 0x1000;
                render.Projection.UserMat3.Row1 = new Vector3(-3563, 2048, 4096) / 0x1000;
                render.Projection.UserMat3.Row2 = new Vector3(4096, -2048, 0) / 0x1000;
                render.Projection.UserColorAmb = new Vector3(614, 614, 614) / 0x200;
                buf_vtx = new Vector4[model.Polygons.Count * 3];
                buf_nor = new Vector3[model.Polygons.Count * 3];
                buf_col = new Color4[model.Polygons.Count * 3];
                foreach (OldModelPolygon polygon in model.Polygons)
                {
                    OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                    if (str is OldModelTexture tex)
                    {
                        buf_col[buf_idx + 0] = new Color4(tex.R, tex.G, tex.B, 255);
                        buf_col[buf_idx + 1] = buf_col[buf_idx];
                        buf_col[buf_idx + 2] = buf_col[buf_idx];
                        RenderVertex(frame, frame2, polygon.VertexA / 6);
                        RenderVertex(frame, frame2, polygon.VertexC / 6);
                        RenderVertex(frame, frame2, polygon.VertexB / 6);
                    }
                    else
                    {
                        OldSceneryColor col = (OldSceneryColor)str;
                        buf_col[buf_idx + 0] = new Color4(col.R, col.G, col.B, 255);
                        buf_col[buf_idx + 1] = buf_col[buf_idx];
                        buf_col[buf_idx + 2] = buf_col[buf_idx];
                        RenderVertex(frame, frame2, polygon.VertexA / 6);
                        RenderVertex(frame, frame2, polygon.VertexC / 6);
                        RenderVertex(frame, frame2, polygon.VertexB / 6);
                    }
                }
                render.Projection.UserTrans = new Vector3(frame.XOffset, frame.YOffset, frame.ZOffset);
                render.Projection.UserScale = new Vector3(model.ScaleX, model.ScaleY, model.ScaleZ);
                vaoModel.UpdatePositions(buf_vtx);
                vaoModel.UpdateNormals(buf_nor);
                vaoModel.UpdateColors(buf_col);
                vaoModel.Render(render);
            }
        }

        private void RenderVertex(OldFrame f1, OldFrame f2, int id)
        {
            if (f2 == null)
            {
                OldFrameVertex v = f1.Vertices[id];
                float x = v.X-128;
                float y = v.Y-128;
                float z = v.Z-128;
                buf_vtx[buf_idx] = new Vector4(x, y, z, 1);
                float nx = v.NormalX;
                float ny = v.NormalY;
                float nz = v.NormalZ;
                buf_nor[buf_idx] = new Vector3(nx, ny, nz);
                var test = render.Projection.UserMat3 * (Vector3.Normalize(buf_nor[buf_idx]));
                buf_idx++;
            }
            else
            {
            }
        }
        /*
        private void RenderCollision(OldFrame frame)
        {
            GL.DepthMask(false);
            GL.Color4(0f, 1f, 0f, 0.2f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            RenderCollisionBox(frame);
            GL.Color4(0f, 1f, 0f, 1f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            RenderCollisionBox(frame);
            GL.DepthMask(true);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        private void RenderCollisionBox(OldFrame frame)
        {
            int xcol1 = frame.X1;
            int xcol2 = frame.X2;
            int ycol1 = frame.Y1;
            int ycol2 = frame.Y2;
            int zcol1 = frame.Z1;
            int zcol2 = frame.Z2;
            GL.PushMatrix();
            GL.Scale(new Vector3(1/256F));
            GL.Translate(frame.XGlobal,frame.YGlobal,frame.ZGlobal);
            GL.Begin(PrimitiveType.QuadStrip);
            GL.Vertex3(xcol1,ycol1,zcol1);
            GL.Vertex3(xcol1,ycol2,zcol1);
            GL.Vertex3(xcol2,ycol1,zcol1);
            GL.Vertex3(xcol2,ycol2,zcol1);
            GL.Vertex3(xcol2,ycol1,zcol2);
            GL.Vertex3(xcol2,ycol2,zcol2);
            GL.Vertex3(xcol1,ycol1,zcol2);
            GL.Vertex3(xcol1,ycol2,zcol2);
            GL.Vertex3(xcol1,ycol1,zcol1);
            GL.Vertex3(xcol1,ycol2,zcol1);
            GL.End();
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(xcol1,ycol1,zcol1);
            GL.Vertex3(xcol2,ycol1,zcol1);
            GL.Vertex3(xcol2,ycol1,zcol2);
            GL.Vertex3(xcol1,ycol1,zcol2);

            GL.Vertex3(xcol1,ycol2,zcol1);
            GL.Vertex3(xcol2,ycol2,zcol1);
            GL.Vertex3(xcol2,ycol2,zcol2);
            GL.Vertex3(xcol1,ycol2,zcol2);
            GL.End();
            GL.PopMatrix();
        }*/
    }
}
