#version 330 core

in vec4 pass_Color;
out vec4 outputColor;

void main(void)
{
    outputColor = pass_Color;
}
