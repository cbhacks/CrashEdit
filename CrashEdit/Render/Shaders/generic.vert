#version 430 core

in vec3 position;
in vec4 color;
in vec2 uv;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

uniform vec3 trans;
uniform vec3 scale;
uniform vec4 userColor1;

out vec4 p_Color;
out vec2 p_ST;

void main()
{
    gl_Position = projectionMatrix * viewMatrix * vec4(position, 1.0);
    p_Color = color;
    p_ST = uv;
}
