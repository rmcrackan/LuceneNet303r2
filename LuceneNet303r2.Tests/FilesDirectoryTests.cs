// do not use System.IO -- name conflicts with Lucene. Esp Directory
//     using System.IO;
using System.Linq;
using FluentAssertions;

using Lucene.Net.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LuceneNet303r2Tests
{
	[TestClass]
	public class FilesDirectoryTests : BaseDirectoryTests
	{
		private string directoryPath { get; }
			= System.IO.Path.Combine(System.IO.Path.GetTempPath(), "LuceneNet303r2_Tests_Index");

		public override Directory InitDirectory() => FSDirectory.Open(directoryPath);

		[TestInitialize]
		public override void InitEachTest()
		{
			System.IO.Directory.CreateDirectory(directoryPath);

			base.InitEachTest();
		}

		[TestCleanup]
		public override void CleanupEachTest()
		{
			base.CleanupEachTest();

			if (!System.IO.Directory.Exists(directoryPath))
				return;

			foreach (var file in System.IO.Directory.EnumerateFiles(directoryPath))
				System.IO.File.Delete(file);

			System.IO.Directory.Delete(directoryPath, true);
		}

		[TestMethod]
		public void VerifyDirectory()
		{
			System.IO.Directory.Exists(directoryPath).Should().BeTrue();
			System.IO.Directory.EnumerateDirectories(directoryPath).Count().Should().Be(0);
			System.IO.Directory.EnumerateFiles(directoryPath).Count().Should().Be(0);
		}
	}
}
