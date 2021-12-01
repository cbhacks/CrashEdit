#version 430 core

in vec3 position;
in vec4 color;
in vec2 uv;
in int tex;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

uniform vec3 trans;

out vec3 p_Color;
out vec2 p_UV;
out int p_Tex;

void main(void)
{
    gl_Position = projectionMatrix * viewMatrix * vec4( (position+trans)/400.0, 1.0 );
    p_Color = vec3(color);
    p_UV = uv;
    p_Tex = tex;
}
