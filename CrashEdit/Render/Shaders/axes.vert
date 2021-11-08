#version 330 core

layout(location = 0) in vec4 position;
layout(location = 1) in vec4 color;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform vec3 viewTrans;

uniform vec4 trans;

out vec4 pass_Color;

void main(void)
{
    gl_Position = projectionMatrix * viewMatrix * (position + trans - vec4(viewTrans, 1.0));
    pass_Color = color;
}
