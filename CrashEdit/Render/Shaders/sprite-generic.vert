#version 430 core

in vec3 position;
in vec4 color;
in vec2 uv;
in vec2 normal;

uniform mat4 PVM;
uniform vec3 viewColumn0;
uniform vec3 viewColumn1;

out vec4 p_Color;
out vec2 p_ST;

void main()
{
    gl_Position = PVM * vec4(position + viewColumn0 * normal.x
                                      + viewColumn1 * normal.y
                                      , 1.0);
    p_Color = color;
    p_ST = uv;
}
