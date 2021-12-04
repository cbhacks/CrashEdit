#version 430 core

in vec2 position;
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
    gl_Position = projectionMatrix * viewMatrix * vec4(trans + vec3(viewMatrix[0][0], viewMatrix[1][0], viewMatrix[2][0]) * position.x * scale.x
                                                             + vec3(viewMatrix[0][1], viewMatrix[1][1], viewMatrix[2][1]) * position.y * scale.y
                                                             , 1.0);
    p_Color = userColor1;
    p_ST = uv;
}
