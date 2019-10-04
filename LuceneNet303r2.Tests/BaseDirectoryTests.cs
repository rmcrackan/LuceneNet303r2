using System;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace LuceneNet303r2Tests
{
	public abstract class BaseDirectoryTests
	{
		public abstract Directory InitDirectory();
		protected Directory Dir;
		public virtual void InitEachTest() => Dir = InitDirectory();
		public virtual void CleanupEachTest() => Dir?.Dispose();

		private void CreateIndex(Action<IndexWriter> action)
		{
			var createNewIndex = true;

			// location of index/create the index
			var index = Dir;

			// analyzer for tokenizing text. same analyzer should be used for indexing and searching
			using var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
			using var ixWriter = new IndexWriter(index, analyzer, createNewIndex, IndexWriter.MaxFieldLength.UNLIMITED);

			action(ixWriter);
			// don't optimize. deprecated: ixWriter.Optimize();
			// ixWriter.Commit(); not needed if we're about to dispose of writer anyway. could be needed within the using() block
		}

		private void Search(Action<Searcher> searchFn)
		{
			var index = Dir;

			// index searcher performs the search
			// new IndexSearcher(index) is a shortcut. if more access to the reader is needed, explicitly cascate index > reader > searcher
			//     using var ixReader = IndexReader.Open(index, true);
			//     using var searcher = new IndexSearcher(ixReader);
			using var searcher = new IndexSearcher(index);
			searchFn(searcher);
		}

		[TestMethod]
		public void CodeProject()
		{
			var source = new codeproject();

			CreateIndex(source.CreateIndex);
			Search(source.Search);
		}

		[TestMethod]
		public void Codewrecks()
		{
			var source = new codewrecks();

			CreateIndex(source.CreateIndex);
			Search(source.Search);
		}

		[TestMethod]
		public void LuceneTutorial()
		{
			var source = new lucenetutorial();

			CreateIndex(source.CreateIndex);
			Search(source.Search);
		}
	}
}
