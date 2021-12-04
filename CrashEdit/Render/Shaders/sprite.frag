#version 430 core

layout(binding=1) uniform usampler2D sprites;

in vec4 p_Color;
in vec2 p_ST;

out vec4 f_Color;

void main()
{
    f_Color = p_Color;

    uvec4 texel = texelFetch(sprites, ivec2(p_ST.x, p_ST.y), 0);
    if (texel.a == 0) discard;
    f_Color *= vec4(texel) / 255.0;
}
