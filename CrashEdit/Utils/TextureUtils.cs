using System;
using System.Collections.Generic;
using Crash;

namespace CrashEdit;

public class TextureUtils
{
    public static Tuple<bool, ModelTexture?> ProcessTextureInfoC2(long currentFrame, int in_tex_id, bool animated, IList<ModelTexture> textures, IList<ModelExtendedTexture> animated_textures)
    {
        if (in_tex_id != 0 || animated)
        {
            ModelTexture? info_temp = null;
            int tex_id = in_tex_id - 1;
            if (animated)
            {
                if (++tex_id >= animated_textures.Count)
                {
                    return new(false, null);
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
                        tex_id += (int)((currentFrame / 2 / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                        if (anim.Leap)
                        {
                            anim = animated_textures[++tex_id];
                            tex_id = anim.Offset - 1 + anim.LOD0;
                        }
                    }
                    if (tex_id >= textures.Count)
                    {
                        return new(false, null);
                    }
                    info_temp = textures[tex_id];
                }
            }
            else
            {
                if (tex_id >= textures.Count)
                {
                    return new(false, null);
                }
                info_temp = textures[tex_id];
            }
            return new(true, info_temp);
        }
        return new(true, null);
    }
}