#version 430 core

in vec3 position;
in vec4 misc;

uniform mat4 PVM;

out vec3 pNode;

void main()
{
    gl_Position = PVM * vec4(position, 1.0);
    pNode = misc.xyz;
}
