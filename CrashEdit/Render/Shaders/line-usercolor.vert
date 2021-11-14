#version 330 core

layout(location = 0) in vec4 position;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

uniform vec4 userColor1;

out vec4 pass_Color;

void main(void)
{
    gl_Position = projectionMatrix * viewMatrix * position;
    pass_Color = userColor1;
}
