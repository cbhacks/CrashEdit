#version 430 core

in vec3 position;
in vec4 color;
in vec2 uv;
in vec4 misc;

uniform mat4 PVM;
uniform mat4 viewMatrix;

out vec4 p_Color;
out vec2 p_ST;

void main()
{
    gl_Position = PVM * vec4(position + misc.x * vec3(viewMatrix[0].x, viewMatrix[1].x, viewMatrix[2].x)
                                      + misc.y * vec3(viewMatrix[0].y, viewMatrix[1].y, viewMatrix[2].y)
                                      , 1.0);
    p_Color = color;
    p_ST = uv;
}
