using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;
using libsvm;

namespace HamburgersBlog.Models
{
    public class RestuarantRecomandationByNLP
    {
        private static readonly RestuarantRecomandationByNLP m_instance = null;
        private static Dictionary<int, string> _predictionDictionary;
        private static IReadOnlyList<string> vocabulary;
        private static C_SVC model;
        public static RestuarantRecomandationByNLP Instance
        {
            get
            {
                return m_instance;
            }
        }
        static RestuarantRecomandationByNLP()
        {
            m_instance = new RestuarantRecomandationByNLP();
        }
        private RestuarantRecomandationByNLP()
        {
            string dataFilePath = HttpContext.Current.Server.MapPath("~/App_Data/TrainingForIsPositiveAlgo.csv");
            var dataTable = DataAccess.DataTable.New.ReadCsv(dataFilePath);
            List<string> x = dataTable.Rows.Select(row => row["Text"]).ToList();
            double[] y = dataTable.Rows.Select(row => double.Parse(row["IsPositive"]))
                                       .ToArray();
            vocabulary = x.SelectMany(GetWords).Distinct().OrderBy(word => word).ToList();
            var problemBuilder = new TextClassificationProblemBuilder();
            var problem = problemBuilder.CreateProblem(x, y, vocabulary.ToList());
            const int C = 1;
            model = new C_SVC(problem, KernelHelper.LinearKernel(), C);
            _predictionDictionary = new Dictionary<int, string> { { -1, "Bad" }, { 1, "Good" } };
        }
        internal bool IsPositiveReview(HttpRequestBase request, HttpResponseBase response, string review)
        {
            var newX = TextClassificationProblemBuilder.CreateNode(review, vocabulary);
            var predictedY = model.Predict(newX);
            return predictedY > 0 ? true : false;
        }
        private static IEnumerable<string> GetWords(string x)
        {
            return x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}