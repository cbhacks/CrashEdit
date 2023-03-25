#version 330 core

layout(location = 0) in vec4 position;
layout(location = 1) in vec4 color;

uniform mat4 PVM;
uniform mat4 modelMatrix;

out vec4 p_Color;

void main(void)
{
    gl_Position = PVM * modelMatrix * position;
    p_Color = color;
}
