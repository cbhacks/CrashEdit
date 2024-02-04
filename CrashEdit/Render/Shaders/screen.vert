#version 430 core

in vec2 position;
in vec4 color;
in vec2 uv;

uniform vec3 scale;

out vec4 p_Color;
out vec2 p_ST;

void main()
{
    gl_Position = vec4((position * 2 / scale.xy) - 1.0, 0.0, 1.0);
    gl_Position.y = -gl_Position.y;
    p_Color = color;
    p_ST = uv;
}
