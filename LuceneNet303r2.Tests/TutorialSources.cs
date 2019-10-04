using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

namespace LuceneNet303r2Tests
{
	public abstract class tutorialSource
	{
		public abstract void CreateIndex(IndexWriter ixWriter);
		public abstract void Search(Searcher searcher);

		/// <summary>use "Lucene query syntax" in QueryParser when search string is user supplied</summary>
		/// <param name="defaultField">field to use when no field is explicitly specified in the query</param>
		public SearchResults QuerySyntax(Searcher searcher, string defaultField, string searchString)
		{
			// create an analyzer to process query syntax
			using var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
			var queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, defaultField, analyzer);
			var query = queryParser.Parse(searchString);
			return SearchAndDisplay(query, searcher);
		}

		/// <summary>directly use the search API within code</summary>
		public SearchResults SearchAPI(Searcher searcher, string field, string text)
		{
			var query = new TermQuery(new Term(field, text));
			return SearchAndDisplay(query, searcher);
		}

		public SearchResults SearchAndDisplay(Query query, Searcher searcher)
		{
			int hitsPerPage = 10;

			// execute the query. do search
			var docs = searcher.Search(query, hitsPerPage);
			var hits = docs.ScoreDocs;

			var resultHits = new List<ResultHit>();

			for (int i = 0; i < hits.Length; i++)
			{
				var score = hits[i].Score;

				var docId = hits[i].Doc;
				var doc = searcher.Doc(docId);
				var allFields = doc.GetFields();

				resultHits.Add(
					new ResultHit(
						score,
						allFields.Select(f => (f.Name, f.StringValue)).ToList()));
			}

			return new SearchResults(hits.Length, resultHits);
		}
	}

	// https://www.codeproject.com/Articles/29755/Introducing-Lucene-Net
	public class codeproject : tutorialSource
	{
		public override void CreateIndex(IndexWriter ixWriter)
		{
			// create 1st document. add a field. write doc to index
			var doc1 = new Document();
			var fldContent = new Field("content", "The quick brown fox jumps over the lazy dog", Field.Store.YES, Field.Index.ANALYZED);
			doc1.Add(fldContent);
			ixWriter.AddDocument(doc1);
		}
		public override void Search(Searcher searcher)
		{
			var searchApiResult = SearchAPI(searcher, "content", "fox");

			var expected1 = new SearchResults(
				1,
				new List<ResultHit>
				{
					new ResultHit(0.115069807f, new List<(string name, string value)> { ("content", "The quick brown fox jumps over the lazy dog") }),
				});

			searchApiResult.AssertEquiv(expected1);


			var querySyntaxResult = QuerySyntax(searcher, "content", "fox");

			var expected2 = new SearchResults(
				1,
				new List<ResultHit>
				{
					new ResultHit(0.115069807f, new List<(string name, string value)> { ("content", "The quick brown fox jumps over the lazy dog") }),
				});

			querySyntaxResult.AssertEquiv(expected2);
		}
	}

	// http://www.codewrecks.com/blog/index.php/2012/06/20/getting-started-with-lucene-net/
	public class codewrecks : tutorialSource
	{
		public override void CreateIndex(IndexWriter ixWriter)
		{
			addStringsWithHashes(ixWriter, "la marianna la va in campagna......");
			addStringsWithHashes(ixWriter, "Lorem Ipsum è un testo segnaposto .....");
		}
		private void addStringsWithHashes(IndexWriter ixWriter, string str)
		{
			var doc = new Document();
			doc.Add(new Field("id", str.GetDeterministicHashCode().ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field("content", str, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
			ixWriter.AddDocument(doc);
		}

		public override void Search(Searcher searcher)
		{
			var results = QuerySyntax(searcher, "content", "Lorem");

			var expected = new SearchResults(
				1,
				new List<ResultHit>
				{
					new ResultHit(0.375f, new List<(string name, string value)> { ("id", "-1446676466"), ("content", "Lorem Ipsum è un testo segnaposto .....") }),
				});

			results.AssertEquiv(expected);
		}
	}

	// http://www.lucenetutorial.com/lucene-in-5-minutes.html
	public class lucenetutorial : tutorialSource
	{
		public override void CreateIndex(IndexWriter ixWriter)
		{
			addBook(ixWriter, "Lucene in Action", "193398817");
			addBook(ixWriter, "Lucene for Dummies", "55320055Z");
			addBook(ixWriter, "Managing Gigabytes", "55063554A");
			addBook(ixWriter, "The Art of Computer Science", "9900333X");
		}
		private void addBook(IndexWriter ixWriter, string title, string isbn)
		{
			var doc = new Document();
			doc.Add(new Field("title", title, Field.Store.YES, Field.Index.ANALYZED)); // ANALYZED == java.TextField
																					   // use a string field for isbn because we don't want it tokenized
			doc.Add(new Field("isbn", isbn, Field.Store.YES, Field.Index.NOT_ANALYZED)); // NOT_ANALYZED == java.StringField
			ixWriter.AddDocument(doc);
		}

		public override void Search(Searcher searcher)
		{
			var results = QuerySyntax(searcher, "title", "lucene");

			var expected = new SearchResults(
				2,
				new List<ResultHit>
				{
					new ResultHit(0.8048013f, new List<(string name, string value)> { ("title", "Lucene in Action"), ("isbn", "193398817") }),
					new ResultHit(0.8048013f, new List<(string name, string value)> { ("title", "Lucene for Dummies"), ("isbn", "55320055Z") })
				});

			results.AssertEquiv(expected);
		}
	}
}
