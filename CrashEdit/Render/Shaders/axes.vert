#version 430 core

in vec3 position;

uniform mat4 PVM;

uniform vec3 trans;

out vec4 p_Color;

void main()
{
    gl_Position = PVM * vec4(position - trans, 1.0);
    p_Color = vec4(normalize(abs(position)), 1.0);
}
