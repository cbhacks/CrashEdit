#version 430 core

layout(binding=0) uniform usampler2D vram8;

uniform int modeCull;
uniform int blendmask;
uniform int enableTex;
uniform int disableDepth;

in vec3 p_Color;
in vec2 p_UV;
in flat int p_Tex;

out vec4 f_col;

int get_texel_bpp4(int u, int v) {
    uint b = texelFetch(vram8, ivec2(u/2, v), 0).r;
    if ((u & 0x1) == 0) {
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
    gl_FragDepth = gl_FragCoord.z;
    if (disableDepth == 1) {
        gl_FragDepth = mix(0.995, intBitsToFloat(floatBitsToInt(1.0)-1), gl_FragDepth);
    }
    int enable = p_Tex & 0x1;
    if (modeCull != 0) {
        // crash 1 anim culling
        int nocull = (p_Tex >> 16) & 0x1;
        if (nocull == 0) {
            if (modeCull == 1 && !gl_FrontFacing) discard;
            if (modeCull == 2 && gl_FrontFacing) discard;
        }
    }
    f_col = vec4(p_Color, 1.0);
    if (enable == 1 && enableTex != 0) {
        int tpage = p_Tex >> 17;
        int cmode = (p_Tex >> 1) & 0x3;
        int bmode = (p_Tex >> 3) & 0x3;
        // discard wrong blend modes, but always let textures pass on solid render
        if (bmode != blendmask && blendmask != 3) discard;
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

        float texel_alpha = 1.0;
        if (texel.r == 0 && texel.g == 0 && texel.b == 0 && texel.a == 0) {
            discard;
        } else if (texel.a == 1 && blendmask == 3 && bmode != 3) {
            // blend texel during solid render pass
            discard;
        } else if (bmode == 0 && texel.a == 1) {
            texel_alpha = 0.5;
        }

        vec3 texel_color = vec3(texel)/31.0;
        f_col *= vec4(2 * texel_color, texel_alpha);
    }
}
