#version 330 core

layout(location = 0) in vec4 position;
layout(location = 1) in vec4 color;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;
uniform vec3 trans;
uniform vec3 scale;

uniform vec4 userColor1;
uniform vec4 userColor2;
uniform int modeColor;

out vec4 p_Color;

void main()
{
    gl_Position = (projectionMatrix * viewMatrix * ((modelMatrix * ((vec4(scale, 1.0) * position)) + vec4(trans*2, 1.0))));
    if (modeColor == 0) {
        p_Color = color;
    } else if (modeColor == 1) {
        p_Color = mix(userColor1, userColor2, (position.y+1)/2);
    } else if (modeColor == 2) {
        p_Color = userColor1;
    }
}
