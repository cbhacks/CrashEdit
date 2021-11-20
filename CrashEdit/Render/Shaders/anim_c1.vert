﻿#version 330 core

layout(location = 0) in vec4 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec4 color;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

uniform vec3 trans;
uniform vec3 modelScale;

out vec4 pass_Color;

void main(void)
{
    gl_Position = projectionMatrix * viewMatrix * vec4( (vec3(position)+trans)*modelScale, 1.0 );
    pass_Color = color;
}
