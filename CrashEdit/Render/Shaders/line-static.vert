#version 430 core

in vec4 position;
in vec4 color;

uniform mat4 PVM;

uniform vec4 userColor1;
uniform int modeColor;

out vec4 p_Color;

void main()
{
    gl_Position = PVM * position;
    if (modeColor == 0) {
        p_Color = color;
    } else if (modeColor == 2) {
        p_Color = userColor1;
    }
}
