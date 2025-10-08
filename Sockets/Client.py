import socket
#import Regressions.main as main
import Translator as trans

HOST = "10.171.218.1"
#HOST = "192.168.4.1"

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, 7777))
    for i in range(1000):
        data = s.recv(1)
        if not data:
            break
        #print(f"Received {data}")
        trans.inputData.append(data)

#main.plot()
trans.output()