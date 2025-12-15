import socket
#import Regressions.main as main
import Translator as trans

HOST = "192.168.137.40"
#HOST = "192.168.4.1"

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, 7777))
    for i in range(10000):
        data = s.recv(1)
        if not data:
            break
        #print(f"Received {data}")
        trans.inputData.append(bin(int.from_bytes(data)))

#main.plot()
trans.output()