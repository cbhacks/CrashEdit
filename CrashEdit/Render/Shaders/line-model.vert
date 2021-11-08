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
uniform int colorMode;

out vec4 pass_Color;

vec4 lerp(vec4 from, vec4 to, float amt)
{
    return from + (to - from) * amt;
}

void main(void)
{
    gl_Position = (projectionMatrix * viewMatrix * ((modelMatrix * ((vec4(scale, 1.0) * position)) + vec4(trans*2, 1.0))));
    if (colorMode == 0) {
        pass_Color = color;
    } else if (colorMode == 1) {
        pass_Color = lerp(userColor1, userColor2, (position.y+1)/2);
    } else if (colorMode == 2) {
        pass_Color = userColor1;
    }
}
