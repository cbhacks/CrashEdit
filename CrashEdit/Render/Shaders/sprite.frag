#version 430 core

layout(binding=1) uniform usampler2D sprites;

in vec4 p_Color;
in vec2 p_ST;

out vec4 f_Color;

void main()
{
    f_Color = p_Color;

    ivec2 st = ivec2(p_ST.x, p_ST.y);
    ivec2 sz = textureSize(sprites, 0);

    if (st.s < 0 || st.s >= sz.x || st.t < 0 || st.t >= sz.y) {
        f_Color *= vec4(1, 0, 1, 1);
    } else {
        uvec4 texel = texelFetch(sprites, st, 0);
        if (texel.a == 0) discard;
        f_Color *= vec4(texel) / 255.0;
    }
}
