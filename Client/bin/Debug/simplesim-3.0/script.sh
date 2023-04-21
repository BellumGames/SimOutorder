#!/bin/bash
chmod 777 script.sh
cd /home/timarc/Desktop/MareProiect/SimOutorder/Client/bin/Debug/simplesim-3.0
./sim-outorder -redir:sim results/gcc_1024.res -fastfwd 1000000 -max:inst 10000000 -lvpt:size 1024 benchmarks/applu.ss < benchmarks/applu.in