#version 430 core

in vec3 position;
in vec3 normal;
in vec4 color;
in vec2 uv;
in int tex;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

uniform int art;

uniform vec3 trans;
uniform vec3 scale;
uniform float scaleScalar;

out vec3 p_Color;
out vec2 p_UV;
out int p_Tex;

void main(void)
{
    switch (art) {
        case 0: // crash 1 worlds
            gl_Position = projectionMatrix * viewMatrix * vec4( (position+trans)/scaleScalar, 1.0 );
            break;
        case 1: // crash 1 anims
            gl_Position = projectionMatrix * viewMatrix * vec4( (position+trans-128.0)*scale, 1.0 );
            break;
    }
    p_Color = vec3(color);
    p_UV = uv;
    p_Tex = tex;
}
