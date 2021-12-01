#version 330 core

layout(location = 0) in vec4 position;
layout(location = 1) in vec4 color;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

uniform vec4 userColor1;
uniform vec4 userColor2;
uniform int colorMode;

out vec4 p_Color;

void main()
{
    gl_Position = projectionMatrix * viewMatrix * position;
    if (colorMode == 0) {
        p_Color = color;
    } else if (colorMode == 2) {
        p_Color = userColor1;
    }
}
