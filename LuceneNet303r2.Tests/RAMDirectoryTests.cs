using Lucene.Net.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LuceneNet303r2Tests
{
	[TestClass]
	public class RAMDirectoryTests : BaseDirectoryTests
	{
		public override Directory InitDirectory() => new RAMDirectory();

		[TestInitialize]
		public override void InitEachTest() => base.InitEachTest();

		[TestCleanup]
		public override void CleanupEachTest() => base.CleanupEachTest();
	}
}
