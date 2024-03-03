#version 430 core

layout(binding=0) uniform usampler2D vram8;

uniform int modeCull;
uniform int blendmask;
uniform int enableTex;
uniform int disableDepth;

in vec4 p_Color;
in vec2 p_UV;
in flat int p_Tex;
in flat int p_TexPage;

out vec4 f_col;

uint get_texel_bpp4(uint u, uint v) {
    uint b = texelFetch(vram8, ivec2(u/2, v), 0).r;
    if ((u & 0x1) == 0) {
        return (b & 0xF);
    } else {
        return ((b >> 4) & 0xF);
    }
}

uint get_texel_bpp8(uint u, uint v) {
    return texelFetch(vram8, ivec2(u, v), 0).r;
}

uvec4 get_texel_bpp16(uint u, uint v) {
    uint lo = texelFetch(vram8, ivec2(u*2, v), 0).r;
    uint hi = texelFetch(vram8, ivec2(u*2+1, v), 0).r;
    uint t = lo | (hi << 8);
    return uvec4(t & 0x1F, (t>>5) & 0x1F, (t>>10) & 0x1F, (t>>15) & 0x1);
}

void main()
{
    f_col = p_Color;
    gl_FragDepth = gl_FragCoord.z;
    if (disableDepth == 1) {
        gl_FragDepth = mix(0.995, intBitsToFloat(floatBitsToInt(1.0)-1), gl_FragDepth);
    }
    if (modeCull != 0) {
        // crash 1 anim culling
        int nocull = (p_Tex >> 15) & 0x1;
        if (nocull == 0) {
            if (modeCull == 1 && !gl_FrontFacing) discard;
            if (modeCull == 2 && gl_FrontFacing) discard;
        }
    }
    if (p_TexPage >= 0 && enableTex != 0) {
        int cmode = (p_Tex >> 0) & 0x3;
        int bmode = (p_Tex >> 2) & 0x3;
        uint cx = ((p_Tex >> 4) & 0xF) * 16;
        uint cy = ((p_Tex >> 8) & 0x7F) + p_TexPage * 128;
        uint u = uint(floor(p_UV.x));
        uint v = uint(floor(p_UV.y)) + p_TexPage * 128;
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
