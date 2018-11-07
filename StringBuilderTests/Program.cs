using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StringBuilderTests {
	[MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
	[MemoryDiagnoser]
	public class Program {
		private const string teststring = "aaaaaaaaaabbbbbbbbbbccccccccccddddddddddeeeeeeeeee";
		public string ConcatSpansStringBuilder(string longstring) {
			var span = longstring.AsSpan();
			var sb = new StringBuilder(longstring.Length);
			sb.Append(span.Slice(40, 10));
			sb.Append(span.Slice(30, 10));
			sb.Append(span.Slice(20, 10));
			sb.Append(span.Slice(10, 10));
			sb.Append(span.Slice(0, 10));
			return sb.ToString();
		}

		public string ConcatSpansCopyTo(string longstring) {
			var span = longstring.AsSpan();
			var targetspan = new Span<char>(new char[longstring.Length]);
			span.Slice(40, 10).CopyTo(targetspan.Slice(0, 10));
			span.Slice(30, 10).CopyTo(targetspan.Slice(10, 10));
			span.Slice(20, 10).CopyTo(targetspan.Slice(20, 10));
			span.Slice(10, 10).CopyTo(targetspan.Slice(30, 10));
			span.Slice(0, 10).CopyTo(targetspan.Slice(40, 10));
			return targetspan.ToString();
		}

		public unsafe string ConcatSpansFastStringBuilder(string longstring) {
			var span = longstring.AsSpan();
			var fsb = new FastStringBuilder(longstring.Length);
			fsb.Append(span.Slice(40, 10));
			fsb.Append(span.Slice(30, 10));
			fsb.Append(span.Slice(20, 10));
			fsb.Append(span.Slice(10, 10));
			fsb.Append(span.Slice(0, 10));
			return fsb.ToString();
		}

		public string ConcatSpansFastBufferStringBuilder(string longstring) {
			var span = longstring.AsSpan();
			Span<char> buf = stackalloc char[longstring.Length];
			var fsb = new FastBufferStringBuilder(buf, longstring.Length);
			fsb.Append(span.Slice(40, 10));
			fsb.Append(span.Slice(30, 10));
			fsb.Append(span.Slice(20, 10));
			fsb.Append(span.Slice(10, 10));
			fsb.Append(span.Slice(0, 10));
			return fsb.ToString();
		}

		public string ConcatSpansFastUnsafeStringBuilder(string longstring) {
			var span = longstring.AsSpan();
			var fsb = new FastUnsafeStringBuilder(longstring.Length);
			fsb.Append(span.Slice(40, 10));
			fsb.Append(span.Slice(30, 10));
			fsb.Append(span.Slice(20, 10));
			fsb.Append(span.Slice(10, 10));
			fsb.Append(span.Slice(0, 10));
			return fsb.ToString();
		}

		[Benchmark]
		public void ConcatSpansStringBuilderBenchmark() {
			ConcatSpansStringBuilder(teststring);
		}

		[Benchmark]
		public void ConcatSpansCopyToBenchmark() {
			ConcatSpansCopyTo(teststring);
		}

		[Benchmark]
		public void ConcatSpansFastStringBuilderBenchmark() {
			ConcatSpansFastStringBuilder(teststring);
		}

		[Benchmark]
		public void ConcatSpansFastBufferStringBuilderBenchmark() {
			ConcatSpansFastBufferStringBuilder(teststring);
		}

		[Benchmark]
		public void ConcatSpansFastUnsafeStringBuilderBenchmark() {
			ConcatSpansFastUnsafeStringBuilder(teststring);
		}

		public static void Main(string[] args) {
			Console.WriteLine(new Program().ConcatSpansFastBufferStringBuilder(teststring));
			var summary = BenchmarkRunner.Run<Program>();
		}
	}

	public ref struct FastStringBuilder {
		private Span<char> span;
		private int pos;

		public FastStringBuilder(int maxlength) {
			span = new Span<char>(new char[maxlength]);
			pos = 0;
		}

		public void Append(ReadOnlySpan<char> str) {
			if (pos + str.Length > span.Length) throw new IndexOutOfRangeException();
			str.CopyTo(span.Slice(pos));
			pos += str.Length;
		}

		public override string ToString() {
			return span.Slice(0, pos).ToString();
		}
	}

	public ref struct FastBufferStringBuilder {
		private Span<char> span;
		private int pos;

		public FastBufferStringBuilder(Span<char> buffer, int maxlength) {
			span = buffer;
			pos = 0;
		}

		public void Append(ReadOnlySpan<char> str) {
			if (pos + str.Length > span.Length) throw new IndexOutOfRangeException();
			str.CopyTo(span.Slice(pos));
			pos += str.Length;
		}

		public override string ToString() {
			return span.Slice(0, pos).ToString();
		}
	}

	public ref struct FastUnsafeStringBuilder {
		private string s;
		private int pos;

		public FastUnsafeStringBuilder(int maxlength) {
			s = new string('\0', maxlength);
			pos = 0;
		}

		public unsafe void Append(ReadOnlySpan<char> str) {
			if (pos + str.Length > s.Length) throw new IndexOutOfRangeException();
			fixed (char* source = str)
			fixed (char* target = s) {
				Unsafe.CopyBlock(target + pos, source, (uint)str.Length * sizeof(char));
			}
			pos += str.Length;
		}

		public override string ToString() {
			if (s.Length == pos) return s;
			return s.Substring(0, pos);
		}
	}
}
