﻿#version 430 core

in vec3 position;

uniform mat4 PVM;

uniform vec3 trans;
uniform vec3 scale;
uniform vec4 userColor1;

out vec4 p_Color;

void main()
{
    gl_Position = PVM * vec4((scale * position) + trans, 1.0);
    p_Color = userColor1;
}