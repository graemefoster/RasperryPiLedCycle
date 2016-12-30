scp ./MonoPiWolf2/Dockerfile pi@192.168.1.16:~/
scp ./MonoPiWolf2/bin/debug/* pi@192.168.1.16:~/monopiwolf2
ssh pi@192.168.1.16 'docker build -t monopiwolf2 .'
ssh pi@192.168.1.16 'docker run --cap-add SYS_RAWIO --device /dev/ttyAMA0:/dev/ttyAMA0 --device /dev/mem:/dev/mem --privileged  monopiwolf2'

