#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;

out vec4 vColor;

uniform mat4 transform;


void main()
{
    vColor = aColor;
    gl_Position = vec4(aPosition, 1.0f) * transform;
}