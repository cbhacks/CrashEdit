#version 430 core

layout(binding=0) uniform usampler2D vram8;

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
    if (p_Tex < 0) {
        f_col = vec4(p_Color, 1.0);
    } else {
        int tpage = (p_Tex >> 0) & 0x7;
        int cmode = (p_Tex >> 3) & 0x3;
        int bmode = (p_Tex >> 5) & 0x3;
        int cx = ((p_Tex >> 7) & 0xF) * 16;
        int cy = (p_Tex >> 11) & 0x7F;
        int u = int(p_UV.x);
        int v = int(p_UV.y) + tpage * 128;
        vec3 texel_color;
        if (cmode == 0) {
            // 4 bit
            uvec4 t = get_texel_bpp16(cx+get_texel_bpp4(u, v), cy);
            texel_color = vec3(t)/31.0;
        } else if (cmode == 1) {
            // 8 bit
            uvec4 t = get_texel_bpp16(cx+get_texel_bpp8(u, v), cy);
            texel_color = vec3(t)/31.0;
        } else {
            // 16 bit
            uvec4 t = get_texel_bpp16(u, v);
            texel_color = vec3(t)/31.0;
        }
        f_col = vec4(p_Color*2*texel_color, 1.0);
    }
}
