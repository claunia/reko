#region License
/* 
 * Copyright (C) 1999-2020 John Källén.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using Reko.Core;
using Reko.Core.Machine;
using System;
using System.Collections.Generic;
using static Reko.Arch.Sparc.SparcDisassembler;

namespace Reko.Arch.Sparc
{
    using Decoder = Decoder<SparcDisassembler, Mnemonic, SparcInstruction>;

    public class InstructionSet
    {
        private const InstrClass Transfer = InstrClass.Delay | InstrClass.Transfer;
        private const InstrClass CondTransfer = InstrClass.Delay | InstrClass.Transfer | InstrClass.Conditional;
        private const InstrClass LinkTransfer = InstrClass.Delay | InstrClass.Transfer | InstrClass.Call;

        private readonly bool is64Bit;

        public InstructionSet(bool is64Bit)
        {
            this.is64Bit = is64Bit;
        }

        public static Decoder Create32BitDecoder()
        {
            var iset = new InstructionSet(false);
            return iset.CreateDecoder();
        }

        public static Decoder Create64BitDecoder()
        {
            var iset = new InstructionSet(true);
            return iset.CreateDecoder();
        }

        internal static Decoder Instr(Mnemonic mnemonic, params Mutator<SparcDisassembler>[] mutators)
        {
            return new InstrDecoder<SparcDisassembler, Mnemonic, SparcInstruction>(InstrClass.Linear, mnemonic, mutators);
        }

        internal static Decoder Instr(Mnemonic mnemonic, InstrClass iclass, params Mutator<SparcDisassembler>[] mutators)
        {
            return new InstrDecoder<SparcDisassembler, Mnemonic, SparcInstruction>(iclass, mnemonic, mutators);
        }

        /// <summary>
        /// Build a 32- or 64-bit decoder depending on the current 64-bitness.
        /// </summary>
        private Decoder Instr64(Decoder decoder64, Decoder decoder32)
        {
            return this.is64Bit ? decoder64 : decoder32;
        }

        Decoder CreateDecoder()
        {
            Decoder invalid = Instr(Mnemonic.illegal, InstrClass.Invalid, nyi);

            var branchOps = new Decoder[]
            {
                // 00
                Instr(Mnemonic.bn, CondTransfer, J),
                Instr(Mnemonic.be, CondTransfer, J),
                Instr(Mnemonic.ble, CondTransfer, J),
                Instr(Mnemonic.bl, CondTransfer, J),
                Instr(Mnemonic.bleu, CondTransfer, J),
                Instr(Mnemonic.bcs, CondTransfer, J),
                Instr(Mnemonic.bneg, CondTransfer, J),
                Instr(Mnemonic.bvs, CondTransfer, J),

                Instr(Mnemonic.ba, CondTransfer, J),
                Instr(Mnemonic.bne, CondTransfer, J),
                Instr(Mnemonic.bg, CondTransfer, J),
                Instr(Mnemonic.bge, CondTransfer, J),
                Instr(Mnemonic.bgu, CondTransfer, J),
                Instr(Mnemonic.bcc, CondTransfer, J),
                Instr(Mnemonic.bpos, CondTransfer, J),
                Instr(Mnemonic.bvc, CondTransfer, J),

                // 10
                Instr(Mnemonic.fbn, CondTransfer, J),
                Instr(Mnemonic.fbne, CondTransfer, J),
                Instr(Mnemonic.fblg, CondTransfer, J),
                Instr(Mnemonic.fbul, CondTransfer, J),
                Instr(Mnemonic.fbug, CondTransfer, J),
                Instr(Mnemonic.fbg, CondTransfer, J),
                Instr(Mnemonic.fbu, CondTransfer, J),
                Instr(Mnemonic.fbug, CondTransfer, J),

                Instr(Mnemonic.fba, CondTransfer, J),
                Instr(Mnemonic.fbe, CondTransfer, J),
                Instr(Mnemonic.fbue, CondTransfer, J),
                Instr(Mnemonic.fbge, CondTransfer, J),
                Instr(Mnemonic.fbuge, CondTransfer, J),
                Instr(Mnemonic.fble, CondTransfer, J),
                Instr(Mnemonic.fbule, CondTransfer, J),
                Instr(Mnemonic.fbo, CondTransfer, J),

                // 20
                Instr(Mnemonic.cbn, J),
                Instr(Mnemonic.cb123, J),
                Instr(Mnemonic.cb12, J),
                Instr(Mnemonic.cb13, J),
                Instr(Mnemonic.cb1, J),
                Instr(Mnemonic.cb23, J),
                Instr(Mnemonic.cb2, J),
                Instr(Mnemonic.cb3, J),

                Instr(Mnemonic.cba, J),
                Instr(Mnemonic.cb0, J),
                Instr(Mnemonic.cb03, J),
                Instr(Mnemonic.cb02, J),
                Instr(Mnemonic.cb023, J),
                Instr(Mnemonic.cb01, J),
                Instr(Mnemonic.cb013, J),
                Instr(Mnemonic.cb012, J),

                // 30
                Instr(Mnemonic.tn, r14,T),
                Instr(Mnemonic.te, r14,T),
                Instr(Mnemonic.tle, r14,T),
                Instr(Mnemonic.tl, r14,T),
                Instr(Mnemonic.tleu, r14,T),
                Instr(Mnemonic.tcs, r14,T),
                Instr(Mnemonic.tneg, r14,T),
                Instr(Mnemonic.tvs, r14,T),

                Instr(Mnemonic.ta, r14,T),
                Instr(Mnemonic.tne, r14,T),
                Instr(Mnemonic.tg, r14,T),
                Instr(Mnemonic.tge, r14,T),
                Instr(Mnemonic.tgu, r14,T),
                Instr(Mnemonic.tcc, r14,T),
                Instr(Mnemonic.tpos, r14,T),
                Instr(Mnemonic.tvc, r14,T),
            };

            var decoders_0 = new Decoder[]
            {
                Instr(Mnemonic.unimp, InstrClass.Invalid),
                Instr(Mnemonic.illegal, InstrClass.Invalid),
                new BranchDecoder(branchOps, 0x00),
                Instr(Mnemonic.illegal, InstrClass.Invalid),

                Instr(Mnemonic.sethi, I,r25),
                Instr(Mnemonic.illegal, InstrClass.Invalid),
                new BranchDecoder(branchOps, 0x10),
                new BranchDecoder(branchOps, 0x20)
            };

            var fpDecoders = new (uint, Decoder)[]
            {
                // 00 
                (0x01, Instr(Mnemonic.fmovs, f0, f25)),
                (0x05, Instr(Mnemonic.fnegs, f0, f25)),
                (0x09, Instr(Mnemonic.fabss, f0, f25)),
                (0x29, Instr(Mnemonic.fsqrts, f0, f25)),
                (0x2A, Instr(Mnemonic.fsqrtd, d0, d25)),
                (0x2B, Instr(Mnemonic.fsqrtq, q0, q25)),

                (0x41, Instr(Mnemonic.fadds, f14, f0, f25)),
                (0x42, Instr(Mnemonic.faddd, d14, d0, d25)),
                (0x43, Instr(Mnemonic.faddq, q14, q0, q25)),
                (0x45, Instr(Mnemonic.fsubs, f14, f0, f25)),
                (0x46, Instr(Mnemonic.fsubd, d14, d0, d25)),
                (0x47, Instr(Mnemonic.fsubq, q14, q0, q25)),

                (0xC4, Instr(Mnemonic.fitos, f0, f25)),
                (0xC6, Instr(Mnemonic.fdtos, d0, f25)),
                (0xC7, Instr(Mnemonic.fqtos, q0, f25)),
                (0xC8, Instr(Mnemonic.fitod, f0, d25)),
                (0xC9, Instr(Mnemonic.fstod, f0, d25)),
                (0xCB, Instr(Mnemonic.fqtod, q0, d25)),
                (0xCC, Instr(Mnemonic.fitoq, f0, q25)),
                (0xCD, Instr(Mnemonic.fstoq, f0, q25)),
                (0xCE, Instr(Mnemonic.fdtoq, d0, q25)),
                (0xD1, Instr(Mnemonic.fstoi, f0, f25)),
                (0xD2, Instr(Mnemonic.fdtoi, d0, f25)),
                (0xD3, Instr(Mnemonic.fqtoi, q0, f25)),

                (0x49, Instr(Mnemonic.fmuls, f14, f0, f25)),
                (0x4A, Instr(Mnemonic.fmuld, d14, d0, d25)),
                (0x4B, Instr(Mnemonic.fmulq, q14, q0, q25)),
                (0x4D, Instr(Mnemonic.fdivs, f14, f0, f25)),
                (0x4E, Instr(Mnemonic.fdivd, d14, d0, d25)),
                (0x4F, Instr(Mnemonic.fdivq, q14, q0, q25)),

                (0x69, Instr(Mnemonic.fsmuld, f14, f0, d25)),
                (0x6E, Instr(Mnemonic.fdmulq, d14, d0, q25)),

                (0x51, Instr(Mnemonic.fcmps, f14, f0)),
                (0x52, Instr(Mnemonic.fcmpd, d14, d0)),
                (0x53, Instr(Mnemonic.fcmpq, q14, q0)),
                (0x55, Instr(Mnemonic.fcmpes, f14, f0)),
                (0x56, Instr(Mnemonic.fcmped, d14, d0)),
                (0x57, Instr(Mnemonic.fcmpeq, q14, q0))
            };

            var decoders_2 = new Decoder[]
            {
                // 00
                Instr(Mnemonic.add, r14,R0,r25),
                Instr(Mnemonic.and, r14,R0,r25),
                Instr(Mnemonic.or, r14,R0,r25),
                Instr(Mnemonic.xor, r14,R0,r25),
                Instr(Mnemonic.sub, r14,R0,r25),
                Instr(Mnemonic.andn, r14,R0,r25),
                Instr(Mnemonic.orn, r14,R0,r25),
                Instr(Mnemonic.xnor, r14,R0,r25),

                Instr64(
                    Instr(Mnemonic.addc, r14,R0,r25),
                    Instr(Mnemonic.addx, r14,R0,r25)),
                Instr64(
                    Instr(Mnemonic.mulx, r14,R0,r25),
                    invalid),
                Instr(Mnemonic.umul, r14,R0,r25),
                Instr(Mnemonic.smul, r14,R0,r25),
                Instr64(
                    Instr(Mnemonic.subc, r14,R0,r25),
                    Instr(Mnemonic.subx, r14,R0,r25)),
                Instr64(
                    Instr(Mnemonic.udivx, r14,R0,r25),
                    invalid),
                Instr(Mnemonic.udiv, r14,R0,r25),
                Instr(Mnemonic.sdiv, r14,R0,r25),

                // 10
                Instr(Mnemonic.addcc, r14,R0,r25),
                Instr(Mnemonic.andcc, r14,R0,r25),
                Instr(Mnemonic.orcc, r14,R0,r25),
                Instr(Mnemonic.xorcc, r14,R0,r25),
                Instr(Mnemonic.subcc, r14,R0,r25),
                Instr(Mnemonic.andncc, r14,R0,r25),
                Instr(Mnemonic.orncc, r14,R0,r25),
                Instr(Mnemonic.xnorcc, r14,R0,r25),

                Instr64(
                    Instr(Mnemonic.addccc, r14,R0,r25),
                    Instr(Mnemonic.addxcc, r14,R0,r25)),
                invalid,
                Instr(Mnemonic.umulcc, r14,R0,r25),
                Instr(Mnemonic.smulcc, r14,R0,r25),
                Instr64(
                    Instr(Mnemonic.subxcc, r14,R0,r25),
                    Instr(Mnemonic.subxcc, r14,R0,r25)),
                invalid,
                Instr(Mnemonic.udivcc, r14,R0,r25),
                Instr(Mnemonic.sdivcc, r14,R0,r25),

                // 20
                Instr(Mnemonic.taddcc, r14,R0,r25),
                Instr(Mnemonic.tsubcc, r14,R0,r25),
                Instr(Mnemonic.taddcctv, r14,R0,r25),
                Instr(Mnemonic.tsubcctv, r14,R0,r25),
                Instr(Mnemonic.mulscc, r14,R0,r25),
                Instr64(
                    SparcDisassembler.Mask(12, 1, "  sll",
                        Instr(Mnemonic.sll, r14,S,r25),
                        Instr(Mnemonic.sllx, r14,S,r25)),
                    Instr(Mnemonic.sll, r14,S,r25)),
                Instr64(
                    SparcDisassembler.Mask(12, 1, "  srl",
                        Instr(Mnemonic.srl, r14,S,r25),
                        Instr(Mnemonic.srlx, r14,S,r25)),
                    Instr(Mnemonic.srl, r14,S,r25)),
                Instr64(
                    SparcDisassembler.Mask(12, 1, "  sra",
                        Instr(Mnemonic.sra, r14,S,r25),
                        Instr(Mnemonic.srax, r14,S,r25)),
                    Instr(Mnemonic.sra, r14,S,r25)),

                Instr64(
                    Instr(Mnemonic.rd, ry,r25), // ****
                    Instr(Mnemonic.rd, ry,r25)),
                Instr64(
                    invalid,
                    Instr(Mnemonic.rdpsr, nyi)),
                Instr64(
                    Instr(Mnemonic.rdpr, InstrClass.System, nyi),
                    Instr(Mnemonic.rdtbr, nyi)),
                Instr64(
                    Instr(Mnemonic.flushw, nyi),
                    invalid),

                Instr64(
                    Instr(Mnemonic.movcc, nyi),
                    invalid),
                Instr64(
                    Instr(Mnemonic.sdivx, r14,R0,r25),
                    invalid),
                Instr64(
                    Instr(Mnemonic.popc, R0,r25),
                    invalid),
                Instr64(
                    Instr(Mnemonic.movr, nyi),
                    invalid),

                // 30
                Instr(Mnemonic.wrasr, nyi), // ****
                Instr64(
                    SparcDisassembler.Sparse(25, 5, "  31", invalid,
                        (0, Instr(Mnemonic.saved, nyi)),
                        (1, Instr(Mnemonic.restored, nyi))),
                    Instr(Mnemonic.wrpsr, nyi)),
                Instr64(
                    Instr(Mnemonic.wrpr, InstrClass.System, nyi),
                    Instr(Mnemonic.wrwim, nyi)),
                Instr64(
                    invalid,
                    Instr(Mnemonic.wrtbr, nyi)),

                SparcDisassembler.Sparse(5, 9, "  FOp1", invalid, fpDecoders),
                SparcDisassembler.Sparse(5, 9, "  FOp2", invalid, fpDecoders),
                SparcDisassembler.Sparse(4, 9, "  CPop1", invalid, fpDecoders),
                SparcDisassembler.Sparse(4, 9, "  CPop2", invalid, fpDecoders),

                Instr(Mnemonic.jmpl, r14,Rs,r25),
                Instr64(
                    Instr(Mnemonic.@return, nyi),
                    Instr(Mnemonic.rett, r14,Rs)),
                new BranchDecoder(branchOps, 0x30),
                Instr(Mnemonic.flush),
                Instr(Mnemonic.save, r14,R0,r25),
                Instr(Mnemonic.restore, r14,R0,r25),
                Instr64(
                    SparcDisassembler.Sparse(25, 5, "  31", invalid,
                        (0, Instr(Mnemonic.done, nyi)),
                        (1, Instr(Mnemonic.retry, nyi))),
                    invalid),
                invalid,
            };

            var decoders_3 = new Decoder[]
            {
                // 00
                Instr64(
                    Instr(Mnemonic.lduw, Mw,r25),
                    Instr(Mnemonic.ld, Mw,r25)),
                Instr(Mnemonic.ldub, Mb,r25),
                Instr(Mnemonic.lduh, Mh,r25),
                Instr(Mnemonic.ldd, Md,r25),
                Instr64(
                    Instr(Mnemonic.stw, r25,Mw),
                    Instr(Mnemonic.st, r25,Mw)),
                Instr(Mnemonic.stb, r25,Mb),
                Instr(Mnemonic.sth, r25,Mh),
                Instr(Mnemonic.std, r25,Md),

                Instr64(
                    Instr(Mnemonic.ldsw, Msw,r25),
                    invalid),
                Instr(Mnemonic.ldsb, Msb,r25),
                Instr(Mnemonic.ldsh, Msh,r25),
                Instr64(
                    Instr(Mnemonic.ldx, Md,r25),
                    invalid),
                invalid,
                Instr(Mnemonic.ldstub, nyi),
                Instr64(
                    Instr(Mnemonic.stx, r25,Md),
                    invalid),
                Instr(Mnemonic.swap, Mw,r25),

                // 10
                Instr64(
                    Instr(Mnemonic.ldua, Aw,r25),
                    Instr(Mnemonic.lda, Aw,r25)),
                Instr(Mnemonic.lduba, Ab,r25),
                Instr(Mnemonic.lduha, Ah,r25),
                Instr(Mnemonic.ldda, Ad,r25),
                Instr64(
                    Instr(Mnemonic.stwa, r25,Aw),
                    Instr(Mnemonic.sta, r25,Aw)),
                Instr(Mnemonic.stba, r25,Ab),
                Instr(Mnemonic.stha, r25,Ah),
                Instr(Mnemonic.stda, r25,Ad),

                Instr64(
                    Instr(Mnemonic.ldswa, r25,Asw),
                    invalid),
                Instr(Mnemonic.ldsba, r25,Asb),
                Instr(Mnemonic.ldsha, r25,Ash),
                Instr64(
                    Instr(Mnemonic.ldxa, r25,Ad),
                    invalid),
                
                invalid,
                Instr(Mnemonic.ldstuba, Ab,r25),
                Instr64(
                    Instr(Mnemonic.stxa, r25,Ad),
                    invalid),
                Instr(Mnemonic.swapa, Aw,r25),

                // 20
                Instr(Mnemonic.ldf,   Mw,f24),
                Instr64(
                    Instr(Mnemonic.ldxfsr, Md,rfsr),
                    Instr(Mnemonic.ldfsr, Mw,rfsr)),
                Instr64(
                    Instr(Mnemonic.ldqf, Mq,f24),
                    invalid),
                Instr(Mnemonic.lddf, Md,f24),
                Instr(Mnemonic.stf, f24,Mw),
                Instr64(
                    Instr(Mnemonic.stxfsr, rfsr,Md),
                    Instr(Mnemonic.stfsr, rfsr,Mw)),
                Instr64(
                    Instr(Mnemonic.stqf, f24,Mq),
                    Instr(Mnemonic.stdfq, nyi)),
                Instr(Mnemonic.stdf, f24,Md),

                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                Instr64(
                    Instr(Mnemonic.prefetch, nyi),
                    invalid),
                invalid,
                invalid,

                // 30
                Instr64(
                    Instr(Mnemonic.ldfa, nyi),
                    Instr(Mnemonic.ldc)),
                Instr64(
                    invalid,
                    Instr(Mnemonic.ldcsr)),
                Instr64(
                    Instr(Mnemonic.ldqfa, nyi),
                    invalid),
                Instr64(
                    Instr(Mnemonic.lddfa, nyi),
                    Instr(Mnemonic.lddc, nyi)),
                Instr64(
                    Instr(Mnemonic.stfa, nyi),
                    Instr(Mnemonic.stc, nyi)),
                Instr64(
                    invalid,
                    Instr(Mnemonic.stcsr, nyi)),
                Instr64(
                    Instr(Mnemonic.stqfa, nyi),
                    Instr(Mnemonic.stdcq, nyi)),
                Instr64(
                    Instr(Mnemonic.stdfa, nyi),
                    Instr(Mnemonic.stdc, nyi)),

                invalid,
                invalid,
                invalid,
                invalid,
                Instr64(
                    Instr(Mnemonic.casa, nyi),
                    invalid),
                Instr64(
                    Instr(Mnemonic.prefetcha, nyi),
                    invalid),
                Instr64(
                    Instr(Mnemonic.casxa, nyi),
                    invalid),
                invalid,
            };


            // Format 1 (op == 1)
            // +----+-------------------------------------------------------------+
            // | op | disp30                                                      |
            // +----+-------------------------------------------------------------+
            //
            // Format 2 (op == 0). SETHI and branches Bicc, FBcc, CBcc
            // +----+---+------+-----+--------------------------------------------+
            // | op | rd       | op2 | imm22                                      |
            // +----+---+------+-----+--------------------------------------------+
            // | op | a | cond | op2 | disp22                                     |
            // +----+---+------+-----+--------------------------------------------+
            // 31   29  28     24    21
            //
            // Format 3 (op = 2, 3)
            // +----+----------+--------+------+-----+--------------------+-------+
            // | op |    rd    |   op3  |  rs1 | i=0 |        asi         |  rs2  |
            // +----+----------+--------+------+-----+--------------------+-------+
            // | op |    rd    |   op3  |  rs1 | i=1 |        simm13              |
            // +----+----------+--------+------+-----+--------------------+-------+
            // | op |    rd    |   op3  |  rs1 |           opf            |  rs2  |
            // +----+----------+--------+------+--------------------------+-------+
            // 31   29         24       18     13    12                   4

            return SparcDisassembler.Mask(30, 2, "SPARC",
                SparcDisassembler.Mask(22, 3, "  Format 0", decoders_0),
                Instr(Mnemonic.call, LinkTransfer, JJ),
                SparcDisassembler.Mask(19, 6, "  Format 2", decoders_2),
                SparcDisassembler.Mask(19, 6, "  Format 3", decoders_3));
        }

        private class BranchDecoder : Decoder
        {
            private readonly Decoder[] branchOps;
            private readonly uint offset;

            public BranchDecoder(Decoder[] branchOps, uint offset)
            {
                this.branchOps = branchOps;
                this.offset = offset;
            }

            public override SparcInstruction Decode(uint wInstr, SparcDisassembler dasm)
            {
                uint i = ((wInstr >> 25) & 0xF) + offset;
                SparcInstruction instr = branchOps[i].Decode(wInstr, dasm);
                instr.InstructionClass |= ((wInstr & (1u << 29)) != 0) ? InstrClass.Annul : 0;
                return instr;
            }
        }

    }


}
