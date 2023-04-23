/* register simulator-specific options */
void
sim_reg_options(struct opt_odb_t *odb)
{
  opt_reg_header(odb,
"sim-outorder: This simulator implements a very detailed out-of-order issue\n"
"superscalar processor with a two-level memory system and speculative\n"
"execution support.  This simulator is a performance simulator, tracking the\n"
"latency of all pipeline operations.\n"
		 );

  /* instruction limit */

  opt_reg_uint(odb, "-max:inst", "maximum number of inst's to execute",
	       &max_insts, /* default */0,
	       /* print */TRUE, /* format */NULL);

  /* trace options */

  opt_reg_int(odb, "-fastfwd", "number of insts skipped before timing starts",
	      &fastfwd_count, /* default */0,
	      /* print */TRUE, /* format */NULL);
  opt_reg_string_list(odb, "-ptrace",
	      "generate pipetrace, i.e., <fname|stdout|stderr> <range>",
	      ptrace_opts, /* arr_sz */2, &ptrace_nelt, /* default */NULL,
	      /* !print */FALSE, /* format */NULL, /* !accrue */FALSE);

  opt_reg_note(odb,
"  Pipetrace range arguments are formatted as follows:\n"
"\n"
"    {{@|#}<start>}:{{@|#|+}<end>}\n"
"\n"
"  Both ends of the range are optional, if neither are specified, the entire\n"
"  execution is traced.  Ranges that start with a `@' designate an address\n"
"  range to be traced, those that start with an `#' designate a cycle count\n"
"  range.  All other range values represent an instruction count range.  The\n"
"  second argument, if specified with a `+', indicates a value relative\n"
"  to the first argument, e.g., 1000:+100 == 1000:1100.  Program symbols may\n"
"  be used in all contexts.\n"
"\n"
"    Examples:   -ptrace FOO.trc #0:#1000\n"
"                -ptrace BAR.trc @2000:\n"
"                -ptrace BLAH.trc :1500\n"
"                -ptrace UXXE.trc :\n"
"                -ptrace FOOBAR.trc @main:+278\n"
	       );

  /* ifetch options */

  opt_reg_int(odb, "-fetch:ifqsize", "instruction fetch queue size (in insts)",
	      &ruu_ifq_size, /* default */4,
	      /* print */TRUE, /* format */NULL);

  opt_reg_int(odb, "-fetch:mplat", "extra branch mis-prediction latency",
	      &ruu_branch_penalty, /* default */3,
	      /* print */TRUE, /* format */NULL);

  opt_reg_int(odb, "-fetch:speed",
	      "speed of front-end of machine relative to execution core",
	      &fetch_speed, /* default */1,
	      /* print */TRUE, /* format */NULL);

  /* branch predictor options */

  opt_reg_note(odb,
"  Branch predictor configuration examples for 2-level predictor:\n"
"    Configurations:   N, M, W, X\n"
"      N   # entries in first level (# of shift register(s))\n"
"      W   width of shift register(s)\n"
"      M   # entries in 2nd level (# of counters, or other FSM)\n"
"      X   (yes-1/no-0) xor history and address for 2nd level index\n"
"    Sample predictors:\n"
"      GAg     : 1, W, 2^W, 0\n"
"      GAp     : 1, W, M (M > 2^W), 0\n"
"      PAg     : N, W, 2^W, 0\n"
"      PAp     : N, W, M (M == 2^(N+W)), 0\n"
"      gshare  : 1, W, 2^W, 1\n"
"  Predictor `comb' combines a bimodal and a 2-level predictor.\n"
               );

  opt_reg_string(odb, "-bpred",
		 "branch predictor type {nottaken|taken|perfect|bimod|2lev|comb}",
                 &pred_type, /* default */"bimod",
                 /* print */TRUE, /* format */NULL);

  opt_reg_int_list(odb, "-bpred:bimod",
		   "bimodal predictor config (<table size>)",
		   bimod_config, bimod_nelt, &bimod_nelt,
		   /* default */bimod_config,
		   /* print */TRUE, /* format */NULL, /* !accrue */FALSE);

  opt_reg_int_list(odb, "-bpred:2lev",
                   "2-level predictor config "
		   "(<l1size> <l2size> <hist_size> <xor>)",
                   twolev_config, twolev_nelt, &twolev_nelt,
		   /* default */twolev_config,
                   /* print */TRUE, /* format */NULL, /* !accrue */FALSE);

  opt_reg_int_list(odb, "-bpred:comb",
		   "combining predictor config (<meta_table_size>)",
		   comb_config, comb_nelt, &comb_nelt,
		   /* default */comb_config,
		   /* print */TRUE, /* format */NULL, /* !accrue */FALSE);

  opt_reg_int(odb, "-bpred:ras",
              "return address stack size (0 for no return stack)",
              &ras_size, /* default */ras_size,
              /* print */TRUE, /* format */NULL);

  opt_reg_int_list(odb, "-bpred:btb",
		   "BTB config (<num_sets> <associativity>)",
		   btb_config, btb_nelt, &btb_nelt,
		   /* default */btb_config,
		   /* print */TRUE, /* format */NULL, /* !accrue */FALSE);

  opt_reg_string(odb, "-bpred:spec_update",
		 "speculative predictors update in {ID|WB} (default non-spec)",
		 &bpred_spec_opt, /* default */NULL,
		 /* print */TRUE, /* format */NULL);

  /* decode options */

  opt_reg_int(odb, "-decode:width",
	      "instruction decode B/W (insts/cycle)",
	      &ruu_decode_width, /* default */4,
	      /* print */TRUE, /* format */NULL);

  /* issue options */

  opt_reg_int(odb, "-issue:width",
	      "instruction issue B/W (insts/cycle)",
	      &ruu_issue_width, /* default */4,
	      /* print */TRUE, /* format */NULL);

  opt_reg_flag(odb, "-issue:inorder", "run pipeline with in-order issue",
	       &ruu_inorder_issue, /* default */FALSE,
	       /* print */TRUE, /* format */NULL);

  opt_reg_flag(odb, "-issue:wrongpath",
	       "issue instructions down wrong execution paths",
	       &ruu_include_spec, /* default */TRUE,
	       /* print */TRUE, /* format */NULL);

  /* commit options */

  opt_reg_int(odb, "-commit:width",
	      "instruction commit B/W (insts/cycle)",
	      &ruu_commit_width, /* default */4,
	      /* print */TRUE, /* format */NULL);

  /* register scheduler options */

  opt_reg_int(odb, "-ruu:size",
	      "register update unit (RUU) size",
	      &RUU_size, /* default */16,
	      /* print */TRUE, /* format */NULL);

  /* memory scheduler options  */

  opt_reg_int(odb, "-lsq:size",
	      "load/store queue (LSQ) size",
	      &LSQ_size, /* default */8,
	      /* print */TRUE, /* format */NULL);

  /* cache options */

  opt_reg_string(odb, "-cache:dl1",
		 "l1 data cache config, i.e., {<config>|none}",
		 &cache_dl1_opt, "dl1:128:32:4:l",
		 /* print */TRUE, NULL);

  opt_reg_note(odb,
"  The cache config parameter <config> has the following format:\n"
"\n"
"    <name>:<nsets>:<bsize>:<assoc>:<repl>\n"
"\n"
"    <name>   - name of the cache being defined\n"
"    <nsets>  - number of sets in the cache\n"
"    <bsize>  - block size of the cache\n"
"    <assoc>  - associativity of the cache\n"
"    <repl>   - block replacement strategy, 'l'-LRU, 'f'-FIFO, 'r'-random\n"
"\n"
"    Examples:   -cache:dl1 dl1:4096:32:1:l\n"
"                -dtlb dtlb:128:4096:32:r\n"
	       );

  opt_reg_int(odb, "-cache:dl1lat",
	      "l1 data cache hit latency (in cycles)",
	      &cache_dl1_lat, /* default */1,
	      /* print */TRUE, /* format */NULL);

  opt_reg_string(odb, "-cache:dl2",
		 "l2 data cache config, i.e., {<config>|none}",
		 &cache_dl2_opt, "ul2:1024:64:4:l",
		 /* print */TRUE, NULL);

  opt_reg_int(odb, "-cache:dl2lat",
	      "l2 data cache hit latency (in cycles)",
	      &cache_dl2_lat, /* default */6,
	      /* print */TRUE, /* format */NULL);

  opt_reg_string(odb, "-cache:il1",
		 "l1 inst cache config, i.e., {<config>|dl1|dl2|none}",
		 &cache_il1_opt, "il1:512:32:1:l",
		 /* print */TRUE, NULL);

  opt_reg_note(odb,
"  Cache levels can be unified by pointing a level of the instruction cache\n"
"  hierarchy at the data cache hiearchy using the \"dl1\" and \"dl2\" cache\n"
"  configuration arguments.  Most sensible combinations are supported, e.g.,\n"
"\n"
"    A unified l2 cache (il2 is pointed at dl2):\n"
"      -cache:il1 il1:128:64:1:l -cache:il2 dl2\n"
"      -cache:dl1 dl1:256:32:1:l -cache:dl2 ul2:1024:64:2:l\n"
"\n"
"    Or, a fully unified cache hierarchy (il1 pointed at dl1):\n"
"      -cache:il1 dl1\n"
"      -cache:dl1 ul1:256:32:1:l -cache:dl2 ul2:1024:64:2:l\n"
	       );

  opt_reg_int(odb, "-cache:il1lat",
	      "l1 instruction cache hit latency (in cycles)",
	      &cache_il1_lat, /* default */1,
	      /* print */TRUE, /* format */NULL);

  opt_reg_string(odb, "-cache:il2",
		 "l2 instruction cache config, i.e., {<config>|dl2|none}",
		 &cache_il2_opt, "dl2",
		 /* print */TRUE, NULL);

  opt_reg_int(odb, "-cache:il2lat",
	      "l2 instruction cache hit latency (in cycles)",
	      &cache_il2_lat, /* default */6,
	      /* print */TRUE, /* format */NULL);

  opt_reg_flag(odb, "-cache:flush", "flush caches on system calls",
	       &flush_on_syscalls, /* default */FALSE, /* print */TRUE, NULL);

  opt_reg_flag(odb, "-cache:icompress",
	       "convert 64-bit inst addresses to 32-bit inst equivalents",
	       &compress_icache_addrs, /* default */FALSE,
	       /* print */TRUE, NULL);

  /* mem options */
  opt_reg_int_list(odb, "-mem:lat",
		   "memory access latency (<first_chunk> <inter_chunk>)",
		   mem_lat, mem_nelt, &mem_nelt, mem_lat,
		   /* print */TRUE, /* format */NULL, /* !accrue */FALSE);

  opt_reg_int(odb, "-mem:width", "memory access bus width (in bytes)",
	      &mem_bus_width, /* default */8,
	      /* print */TRUE, /* format */NULL);

  /* TLB options */

  opt_reg_string(odb, "-tlb:itlb",
		 "instruction TLB config, i.e., {<config>|none}",
		 &itlb_opt, "itlb:16:4096:4:l", /* print */TRUE, NULL);

  opt_reg_string(odb, "-tlb:dtlb",
		 "data TLB config, i.e., {<config>|none}",
		 &dtlb_opt, "dtlb:32:4096:4:l", /* print */TRUE, NULL);

  opt_reg_int(odb, "-tlb:lat",
	      "inst/data TLB miss latency (in cycles)",
	      &tlb_miss_lat, /* default */30,
	      /* print */TRUE, /* format */NULL);

  /* resource configuration */

  opt_reg_int(odb, "-res:ialu",
	      "total number of integer ALU's available",
	      &res_ialu, /* default */fu_config[FU_IALU_INDEX].quantity,
	      /* print */TRUE, /* format */NULL);

  opt_reg_int(odb, "-res:imult",
	      "total number of integer multiplier/dividers available",
	      &res_imult, /* default */fu_config[FU_IMULT_INDEX].quantity,
	      /* print */TRUE, /* format */NULL);

  opt_reg_int(odb, "-res:memport",
	      "total number of memory system ports available (to CPU)",
	      &res_memport, /* default */fu_config[FU_MEMPORT_INDEX].quantity,
	      /* print */TRUE, /* format */NULL);

  opt_reg_int(odb, "-res:fpalu",
	      "total number of floating point ALU's available",
	      &res_fpalu, /* default */fu_config[FU_FPALU_INDEX].quantity,
	      /* print */TRUE, /* format */NULL);

  opt_reg_int(odb, "-res:fpmult",
	      "total number of floating point multiplier/dividers available",
	      &res_fpmult, /* default */fu_config[FU_FPMULT_INDEX].quantity,
	      /* print */TRUE, /* format */NULL);

  opt_reg_string_list(odb, "-pcstat",
		      "profile stat(s) against text addr's (mult uses ok)",
		      pcstat_vars, MAX_PCSTAT_VARS, &pcstat_nelt, NULL,
		      /* !print */FALSE, /* format */NULL, /* accrue */TRUE);

  opt_reg_flag(odb, "-bugcompat",
	       "operate in backward-compatible bugs mode (for testing only)",
	       &bugcompat_mode, /* default */FALSE, /* print */TRUE, NULL);
}

/* check simulator-specific option values */
