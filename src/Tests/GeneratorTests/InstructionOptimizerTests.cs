﻿using Synfron.Staxe.Executor.Instructions;
using Synfron.Staxe.Generator;
using System.Collections.Generic;
using Xunit;
using G = Synfron.Staxe.Executor.GroupState;

namespace GeneratorTests
{
	public class InstructionOptimizerTests
	{
		[Fact]
		public void InstructionOptimizer_Optimize_Assignments()
		{
			List<Instruction<G>> input = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 8 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.SPR, new object[] { 1 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.PHR, default, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.SPR, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.RR, default, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 8 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};
			List<Instruction<G>> expectedResults = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 5 }, default, default, default),
				InstructionProvider<G>.GetInstruction(InstructionCode.SPSP, new object[] { 2, 1 }, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 5 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};

			InstructionOptimizer<G> sut = new InstructionOptimizer<G>();
			IList<Instruction<G>> results = sut.Optimize(input);

			Assert.Equal(expectedResults, results, new InstructionComparer<G>());
		}

		[Fact]
		public void InstructionOptimizer_Optimize_Opposing()
		{
			List<Instruction<G>> input = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 6 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.B, default, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.BE, default, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 6 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};
			List<Instruction<G>> expectedResults = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 4 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 4 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};

			InstructionOptimizer<G> sut = new InstructionOptimizer<G>();
			IList<Instruction<G>> results = sut.Optimize(input);

			Assert.Equal(expectedResults, results, new InstructionComparer<G>());
		}

		[Fact]
		public void InstructionOptimizer_Optimize_CopyPush()
		{
			List<Instruction<G>> input = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 7 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.VR, new object[] { "value" }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.PHR, default, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.VR, new object[] { "value" }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 7 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};
			List<Instruction<G>> expectedResults = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 6 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.VR, new object[] { "value" }, default, default, default),
				InstructionProvider<G>.GetInstruction(InstructionCode.CPHR, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 6 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};

			InstructionOptimizer<G> sut = new InstructionOptimizer<G>();
			IList<Instruction<G>> results = sut.Optimize(input);

			Assert.Equal(expectedResults, results, new InstructionComparer<G>());
		}

		[Fact]
		public void InstructionOptimizer_Optimize_Overriding()
		{
			List<Instruction<G>> input = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 6 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.SPR, new object[] { "value1" }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.SPR, new object[] { "value2" }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 6 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};
			List<Instruction<G>> expectedResults = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 5 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.SPR, new object[] { "value2" }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 5 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};

			InstructionOptimizer<G> sut = new InstructionOptimizer<G>();
			IList<Instruction<G>> results = sut.Optimize(input);

			Assert.Equal(expectedResults, results, new InstructionComparer<G>());
		}

		[Fact]
		public void InstructionOptimizer_Optimize_Duplicates()
		{
			List<Instruction<G>> input = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 6 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.MP, new object[] { "value" }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.MP, new object[] { "value" }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 6 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};
			List<Instruction<G>> expectedResults = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 5 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.MP, new object[] { "value" }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 5 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};

			InstructionOptimizer<G> sut = new InstructionOptimizer<G>();
			IList<Instruction<G>> results = sut.Optimize(input);

			Assert.Equal(expectedResults, results, new InstructionComparer<G>());
		}

		[Fact]
		public void InstructionOptimizer_Optimize_MultipleGetPointers()
		{
			List<Instruction<G>> input = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 7 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.SPR, new object[] { 1 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.PHR, default, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.SPR, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 7 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};
			List<Instruction<G>> expectedResults = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 5 }, default, default, default),
				InstructionProvider<G>.GetInstruction(InstructionCode.CPR, new object[] { -1, 2, InstructionCode.SPR, 1, InstructionCode.SPR, 2 }, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 5 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};

			InstructionOptimizer<G> sut = new InstructionOptimizer<G>();
			IList<Instruction<G>> results = sut.Optimize(input);

			Assert.Equal(expectedResults, results, new InstructionComparer<G>());
		}

		[Fact]
		public void InstructionOptimizer_Optimize_MultipleRounds()
		{
			List<Instruction<G>> input = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 8 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.B, default, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.B, default, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.BE, default, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.BE, default, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 8 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};
			List<Instruction<G>> expectedResults = new List<Instruction<G>>
			{
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 4 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.US, new object[] { 2 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.J, new object[] { 4 }, default, default, default),
				TestInstructionProvider<G>.GetInstruction(InstructionCode.NON, new object[] { 8 }, default, default, default)
			};

			InstructionOptimizer<G> sut = new InstructionOptimizer<G>();
			IList<Instruction<G>> results = sut.Optimize(input);

			Assert.Equal(expectedResults, results, new InstructionComparer<G>());
		}
	}
}
