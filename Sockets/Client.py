import socket

HOST = "10.160.81.1"

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, 7777))
    while True:
        data = s.recv(1024)
        if not data:
            break
        print(f"Received {data}")

