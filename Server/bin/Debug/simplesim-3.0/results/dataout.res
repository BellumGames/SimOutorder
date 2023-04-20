sim-cache: SimpleScalar/PISA Tool Set version 3.0 of November, 2000.
Copyright (c) 1994-2000 by Todd M. Austin.  All Rights Reserved.
This version of SimpleScalar is licensed for academic non-commercial use only.

sim: command line: ./sim-cache -redir:sim results/dataout.res -max:inst 2400000 -cache:dl1 dl1:8192:32:4:l -cache:dl2 ul2:1024:64:4:l -cache:il1 il1:256:32:1:l -cache:il2 dl2 -tlb:dtlb dtlb:32:4096:4:l -tlb:itlb itlb:16:4096:1:l benchmarks/applu.ss 

sim: simulation started @ Fri May  6 19:50:37 2022, options follow:

sim-cache: This simulator implements a functional cache simulator.  Cache
statistics are generated for a user-selected cache and TLB configuration,
which may include up to two levels of instruction and data cache (with any
levels unified), and one level of instruction and data TLBs.  No timing
information is generated.

# -config                     # load configuration from a file
# -dumpconfig                 # dump configuration to a file
# -h                    false # print help message    
# -v                    false # verbose operation     
# -d                    false # enable debug message  
# -i                    false # start in Dlite debugger
-seed                       1 # random number generator seed (0 for timer seed)
# -q                    false # initialize and terminate immediately
# -chkpt               <null> # restore EIO trace execution from <fname>
# -redir:sim     results/dataout.res # redirect simulator output to file (non-interactive only)
# -redir:prog          <null> # redirect simulated program output to file
-nice                       0 # simulator scheduling priority
-max:inst             2400000 # maximum number of inst's to execute
-cache:dl1       dl1:8192:32:4:l # l1 data cache config, i.e., {<config>|none}
-cache:dl2       ul2:1024:64:4:l # l2 data cache config, i.e., {<config>|none}
-cache:il1       il1:256:32:1:l # l1 inst cache config, i.e., {<config>|dl1|dl2|none}
-cache:il2                dl2 # l2 instruction cache config, i.e., {<config>|dl2|none}
-tlb:itlb        itlb:16:4096:1:l # instruction TLB config, i.e., {<config>|none}
-tlb:dtlb        dtlb:32:4096:4:l # data TLB config, i.e., {<config>|none}
-flush                  false # flush caches on system calls
-cache:icompress        false # convert 64-bit inst addresses to 32-bit inst equivalents
# -pcstat              <null> # profile stat(s) against text addr's (mult uses ok)

  The cache config parameter <config> has the following format:

    <name>:<nsets>:<bsize>:<assoc>:<repl>

    <name>   - name of the cache being defined
    <nsets>  - number of sets in the cache
    <bsize>  - block size of the cache
    <assoc>  - associativity of the cache
    <repl>   - block replacement strategy, 'l'-LRU, 'f'-FIFO, 'r'-random

    Examples:   -cache:dl1 dl1:4096:32:1:l
                -dtlb dtlb:128:4096:32:r

  Cache levels can be unified by pointing a level of the instruction cache
  hierarchy at the data cache hiearchy using the "dl1" and "dl2" cache
  configuration arguments.  Most sensible combinations are supported, e.g.,

    A unified l2 cache (il2 is pointed at dl2):
      -cache:il1 il1:128:64:1:l -cache:il2 dl2
      -cache:dl1 dl1:256:32:1:l -cache:dl2 ul2:1024:64:2:l

    Or, a fully unified cache hierarchy (il1 pointed at dl1):
      -cache:il1 dl1
      -cache:dl1 ul1:256:32:1:l -cache:dl2 ul2:1024:64:2:l



sim: ** starting functional simulation w/ caches **
warning: syscall: sigvec ignored
warning: syscall: sigvec ignored
warning: syscall: sigvec ignored
warning: syscall: sigvec ignored
warning: syscall: sigvec ignored
warning: syscall: sigvec ignored

sim: ** simulation statistics **
sim_num_insn                2400000 # total number of instructions executed
sim_num_refs                 493462 # total number of loads and stores executed
sim_elapsed_time                  1 # total simulation time in seconds
sim_inst_rate          2400000.0000 # simulation speed (in insts/sec)
il1.accesses                2400000 # total number of accesses
il1.hits                    2355438 # total number of hits
il1.misses                    44562 # total number of misses
il1.replacements              44307 # total number of replacements
il1.writebacks                    0 # total number of writebacks
il1.invalidations                 0 # total number of invalidations
il1.miss_rate                0.0186 # miss rate (i.e., misses/ref)
il1.repl_rate                0.0185 # replacement rate (i.e., repls/ref)
il1.wb_rate                  0.0000 # writeback rate (i.e., wrbks/ref)
il1.inv_rate                 0.0000 # invalidation rate (i.e., invs/ref)
dl1.accesses                 913636 # total number of accesses
dl1.hits                     906525 # total number of hits
dl1.misses                     7111 # total number of misses
dl1.replacements                 61 # total number of replacements
dl1.writebacks                   60 # total number of writebacks
dl1.invalidations                 0 # total number of invalidations
dl1.miss_rate                0.0078 # miss rate (i.e., misses/ref)
dl1.repl_rate                0.0001 # replacement rate (i.e., repls/ref)
dl1.wb_rate                  0.0001 # writeback rate (i.e., wrbks/ref)
dl1.inv_rate                 0.0000 # invalidation rate (i.e., invs/ref)
ul2.accesses                  51733 # total number of accesses
ul2.hits                      47264 # total number of hits
ul2.misses                     4469 # total number of misses
ul2.replacements               1114 # total number of replacements
ul2.writebacks                    0 # total number of writebacks
ul2.invalidations                 0 # total number of invalidations
ul2.miss_rate                0.0864 # miss rate (i.e., misses/ref)
ul2.repl_rate                0.0215 # replacement rate (i.e., repls/ref)
ul2.wb_rate                  0.0000 # writeback rate (i.e., wrbks/ref)
ul2.inv_rate                 0.0000 # invalidation rate (i.e., invs/ref)
itlb.accesses               2400000 # total number of accesses
itlb.hits                   2399808 # total number of hits
itlb.misses                     192 # total number of misses
itlb.replacements               177 # total number of replacements
itlb.writebacks                   0 # total number of writebacks
itlb.invalidations                0 # total number of invalidations
itlb.miss_rate               0.0001 # miss rate (i.e., misses/ref)
itlb.repl_rate               0.0001 # replacement rate (i.e., repls/ref)
itlb.wb_rate                 0.0000 # writeback rate (i.e., wrbks/ref)
itlb.inv_rate                0.0000 # invalidation rate (i.e., invs/ref)
dtlb.accesses                913636 # total number of accesses
dtlb.hits                    913414 # total number of hits
dtlb.misses                     222 # total number of misses
dtlb.replacements                94 # total number of replacements
dtlb.writebacks                  90 # total number of writebacks
dtlb.invalidations                0 # total number of invalidations
dtlb.miss_rate               0.0002 # miss rate (i.e., misses/ref)
dtlb.repl_rate               0.0001 # replacement rate (i.e., repls/ref)
dtlb.wb_rate                 0.0001 # writeback rate (i.e., wrbks/ref)
dtlb.inv_rate                0.0000 # invalidation rate (i.e., invs/ref)
ld_text_base             0x00400000 # program text (code) segment base
ld_text_size                 234912 # program text (code) size in bytes
ld_data_base             0x10000000 # program initialized data segment base
ld_data_size               33097472 # program init'ed `.data' and uninit'ed `.bss' size in bytes
ld_stack_base            0x7fffc000 # program stack segment base (highest address in stack)
ld_stack_size                 16384 # program initial stack size
ld_prog_entry            0x00400140 # program entry point (initial PC)
ld_environ_base          0x7fff8000 # program environment base address address
ld_target_big_endian              0 # target executable endian-ness, non-zero if big endian
mem.page_count                  259 # total number of pages allocated
mem.page_mem                  1036k # total size of memory pages allocated
mem.ptab_misses                 261 # total first level page table misses
mem.ptab_accesses          12885720 # total page table accesses
mem.ptab_miss_rate           0.0000 # first level page table miss rate

