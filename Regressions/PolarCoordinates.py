import numpy as np

#input = (10,np.pi)
#origin = (0,0)



def convert(input, origin):
    x = input[0] * np.cos(input[1])
    y = input[0] * np.sin(input[1])
    x += origin[0]
    y += origin[1]
    return (x,y)