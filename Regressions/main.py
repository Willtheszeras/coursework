import matplotlib.pyplot as plt
import numpy as np
#points = [(1,0), (2,0), (3,0), (4,0), (5,0), (6,0), (7,0)]
x = [1,2,3,4,5,6,7]
x_ = np.mean(x)
y = [1,2,3,3,5,6,8]
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


m = np.sum(x - np.mean(x))
print(m)

plt.show()