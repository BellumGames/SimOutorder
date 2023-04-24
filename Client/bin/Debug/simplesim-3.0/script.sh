#!/bin/bash
chmod 777 script.sh
cd /home/bellum/Projects/SimOutorder/Client/bin/Debug/simplesim-3.0/
./sim-outorder -max:inst 10000 -fastfwd 0 -bpred:bimod 512 -cache:dl1 dl1:128:32:4:l -cache:dl1lat 1 -cache:dl2 dl2:1024:64:4:l -cache:dl2lat 6 -cache:il1 il1:512:32:1:l -cache:il1lat 1 -cache:il2 il2:1024:64:4:l -cache:il2lat 6 -tlb:dtlb dtlb:32:4096:4:l -tlb:itlb itlb:16:4096:4:l -tlb:lat 30 -redir:sim results/applu_simout.res benchmarks/applu.ss < benchmarks/applu.in && exit