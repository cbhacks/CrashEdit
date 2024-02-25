#version 430 core

in vec4 position;
in vec4 color;

uniform mat4 PVM;

out vec4 p_Color;

void main()
{
    gl_Position = PVM * position;
    p_Color = color;
}
