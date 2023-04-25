#!/bin/bash
chmod 777 script.sh
cd /home/timarc/Desktop/MareProiect/SimOutorder/Client/bin/Debug/simplesim-3.0/
./sim-outorder -redir:sim results/applu_simout.res -redir:prog results/applu_progout.res -max:inst 10000 -fastfwd 0 -fetch:ifqsize 4 -bpred:bimod 2048 -decode:width 4 -issue:width 4 -commit:width 4 -ruu:size 16 -lsq:size 8 -cache:dl1 dl1:128:32:4:l -mem:lat 18 2 -tlb:itlb itlb:16:4096:4:l -res:ialu 4 benchmarks/applu.ss < benchmarks/applu.in