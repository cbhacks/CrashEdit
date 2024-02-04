#version 430 core

in vec2 position;
in vec2 uv;

uniform mat4 PVM;
uniform vec3 viewColumn0;
uniform vec3 viewColumn1;

uniform vec3 trans;
uniform vec3 scale;
uniform vec4 userColor1;

out vec4 p_Color;
out vec2 p_ST;

void main()
{
    gl_Position = PVM * vec4(trans + viewColumn0 * position.x * scale.x
                                   + viewColumn1 * position.y * scale.y
                                   , 1.0);
    p_Color = userColor1;
    p_ST = uv;
}
