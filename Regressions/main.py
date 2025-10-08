import matplotlib.pyplot as plt
import numpy as np
import PolarCoordinates as pc
import Sockets.Client as client
#points = [(1,0), (2,0), (3,0), (4,0), (5,0), (6,0), (7,0)]


inputData = [(10,np.pi),(10,1),(5,np.pi*3/4),(2,-2),(15,3),(10,2),(5,0)]
origin = (0,1)



x = []
y = []
for i in inputData:
    out = pc.convert(i, origin)
    x.append(out[0])
    y.append(out[1])
x_ = np.mean(x)
y_ = np.mean(y)
m = 0
c = 0

up = 0
down = 0
for i in range(len(x)):
    up += (x[i] - x_)*(y[i] - y_)
    down += (x[i] - x_)**2

m = up/down

c = (y_ - m*x_)

min = min(x)
max = max(x)

fig, ax = plt.subplots() 
# for i in points:
#     ax.scatter(i[0], i[1])

ax.scatter(x,y)
ax.plot([min, max],[m*min + c, m*max + c])
ax.scatter(origin[0], origin[1])


m = np.sum(x - np.mean(x))
print(m)

plt.show()