#version 430 core

layout(binding=2) uniform sampler2D font;

in vec4 p_Color;
in vec2 p_ST;

out vec4 f_Color;

void main()
{
    f_Color = vec4(p_Color.rgb, texture(font, p_ST).r);
}
