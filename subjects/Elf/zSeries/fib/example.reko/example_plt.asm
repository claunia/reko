;;; Segment .plt (000005A0)
000005A0 E3 10 F0 38 00 24 C0 10 00 00 0D 2D D2 07 F0 30 ...8.$.....-...0
000005B0 10 08 E3 10 10 10 00 04 07 F1 07 00 07 00 07 00 ................

;; __cxa_finalize: 000005C0
;;   Called from:
;;     00000702 (in __do_global_dtors_aux)
__cxa_finalize proc
	larl	r1,00002018
	lg	r1,(r1)
	br	r1
000005CE                                           0D 10               ..
000005D0 E3 10 10 0C 00 14 C0 F4 FF FF FF E5 00 00 00 00 ................

;; __libc_start_main: 000005E0
;;   Called from:
;;     0000063E (in _start)
__libc_start_main proc
	larl	r1,00002020
	lg	r1,(r1)
	br	r1
000005EE                                           0D 10               ..
000005F0 E3 10 10 0C 00 14 C0 F4 FF FF FF D5 00 00 00 18 ................
