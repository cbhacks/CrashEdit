#version 430 core

layout(binding=2) uniform sampler2D font;

in vec4 p_Color;
in vec2 p_ST;

out vec4 f_Color;

void main()
{
    f_Color = p_Color;
    f_Color.a *= texture(font, p_ST).r;
}
