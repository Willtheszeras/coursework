import matplotlib.pyplot as plt
import numpy as np
#points = [(1,0), (2,0), (3,0), (4,0), (5,0), (6,0), (7,0)]
x = [1,2,3,4,5,6,7]
y = [0,0,0,0,0,0,0]
m = 1
c = 0
E = 0
fig, ax = plt.subplots() 
# for i in points:
#     ax.scatter(i[0], i[1])

ax.scatter(x,y)


m = np.sum(x - np.mean(x))
print(m)

plt.show()