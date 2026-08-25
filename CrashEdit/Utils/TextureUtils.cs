using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit;

public class TextureUtils
{
    public static bool ProcessTextureInfoC2(long texture_frame, int in_tex_id, bool animated, IList<ModelTexture> textures, IList<ModelExtendedTexture> animated_textures, out ModelTexture tex)
    {
        if (in_tex_id != 0 || animated)
        {
            int tex_id = in_tex_id - 1;
            if (animated)
            {
                if (++tex_id >= animated_textures.Count)
                {
                    tex = default;
                    return false;
                }
                var anim = animated_textures[tex_id];
                // check if it's an untextured polygon
                if (anim.Offset != 0)
                {
                    tex_id = anim.Offset - 1;
                    if (anim.IsLOD)
                    {
                        tex_id += anim.LOD0; // we only render closest LOD for now
                    }
                    else
                    {
                        tex_id += (int)((texture_frame / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                        if (anim.Leap)
                        {
                            anim = animated_textures[++tex_id];
                            tex_id = anim.Offset - 1 + anim.LOD0;
                        }
                    }
                    if (tex_id >= textures.Count)
                    {
                        tex = default;
                        return false;
                    }
                    tex = textures[tex_id];
                }
                else
                {
                    tex = default;
                }
            }
            else
            {
                if (tex_id >= textures.Count)
                {
                    tex = default;
                    return false;
                }
                tex = textures[tex_id];
            }
            return true;
        }
        tex = default;
        return true;
    }
    
    public static Vector2 TextureSize (int colorMode)
    {
        return colorMode switch
        {
            0 => new Vector2 (1024, 128),
            1 => new Vector2 (512, 128),
            _ => new Vector2 (256, 128)
        };
    }
}