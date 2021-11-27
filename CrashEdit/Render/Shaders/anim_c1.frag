#version 430 core

layout(binding=0) uniform usampler2D vram8;

uniform int cullmode;

in vec3 p_Color;
in vec2 p_UV;
in flat int p_Tex;

out vec4 f_col;

int get_texel_bpp4(int u, int v) {
    uint b = texelFetch(vram8, ivec2(u/2, v), 0).r;
    if ((u % 2) == 0) {
        return int(b & 0xF);
    } else {
        return int((b >> 4) & 0xF);
    }
}

int get_texel_bpp8(int u, int v) {
    uint b = texelFetch(vram8, ivec2(u, v), 0).r;
    return int(b);
}

uvec4 get_texel_bpp16(int u, int v) {
    uint lo = texelFetch(vram8, ivec2(u*2, v), 0).r;
    uint hi = texelFetch(vram8, ivec2(u*2+1, v), 0).r;
    uint t = lo | (hi << 8);
    return uvec4(t & 0x1F, (t>>5) & 0x1F, (t>>10) & 0x1F, (t>>15) & 0x1);
}

void main()
{
    int enable = p_Tex & 0x1;
    int cull = (p_Tex >> 16) & 0x1;
    if (cull == 0) {
        if (cullmode == 0 && !gl_FrontFacing) discard;
        if (cullmode == 1 && gl_FrontFacing) discard;
    }
    f_col = vec4(p_Color, 1.0);
    if (enable == 1) {
        int tpage = p_Tex >> 17;
        int cmode = (p_Tex >> 1) & 0x3;
        int bmode = (p_Tex >> 3) & 0x3;
        int cx = ((p_Tex >> 5) & 0xF) * 16;
        int cy = ((p_Tex >> 9) & 0x7F) + tpage * 128;
        int u = int(p_UV.x);
        int v = int(p_UV.y) + tpage * 128;
        uvec4 texel;
        if (cmode == 0) {
            texel = get_texel_bpp16(cx+get_texel_bpp4(u, v), cy); // 4 bit
        } else if (cmode == 1) {
            texel = get_texel_bpp16(cx+get_texel_bpp8(u, v), cy); // 8 bit
        } else {
            texel = get_texel_bpp16(u, v); // 16 bit
        }
        if (texel.x == 0 && texel.y == 0 && texel.z == 0 && texel.w == 0) discard; // useless texel

        vec3 texel_color = vec3(texel)/31.0;
        float texel_alpha = 1.0;
        if (bmode == 0 && texel.w == 1) {
            texel_alpha = 0.5;
        }
        f_col *= vec4(2 * texel_color, texel_alpha);
    }
}
