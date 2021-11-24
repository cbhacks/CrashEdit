#version 430 core

in vec4 position;
in vec3 normal;
in vec3 color;
in vec3 uvc;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

uniform vec3 trans;
uniform vec3 modelScale;

out vec3 pass_Color;
out vec3 pass_UVC;

void main(void)
{
    gl_Position = projectionMatrix * viewMatrix * vec4( (vec3(position)+trans)*modelScale, 1.0 );
    pass_Color = color;
    pass_UVC = uvc;
}
