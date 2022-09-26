namespace PowerLevel.Server;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth;
using Infrastructure;
using LinqToDB;
using Xdxd.DotNet.Rpc;

public class QuizHandler
{
    private readonly QuizService quizService;
    private readonly IDbService db;

    public QuizHandler(QuizService quizService, IDbService db)
    {
        this.quizService = quizService;
        this.db = db;
    }

    public class SearchQuizzesRequest
    {
        public string Query { get; set; }
    }

    public class SearchQuizzesResponse
    {
        public QuizPoco[] Items { get; set; }
    }

    [RpcBind(typeof(SearchQuizzesRequest), typeof(SearchQuizzesResponse))]
    public async Task<SearchQuizzesResponse> SearchQuizzes(SearchQuizzesRequest req, AuthResult authResult)
    {
        var items = await this.quizService.Search(req.Query, authResult.ProfileID);

        Array.Sort(items, QuizModel.IdComparer);

        return new SearchQuizzesResponse
        {
            Items = items,
        };
    }

    public class GetQuizRequest
    {
        public int Id { get; set; }
    }

    public class GetQuizResponse
    {
        public QuizModel Item { get; set; }
    }

    [RpcBind(typeof(GetQuizRequest), typeof(GetQuizResponse))]
    public async Task<GetQuizResponse> GetQuiz(GetQuizRequest req, AuthResult authResult)
    {
        return new GetQuizResponse
        {
            Item = await this.quizService.Get(req.Id, authResult.ProfileID),
        };
    }

    public class DeleteQuizRequest
    {
        public int Id { get; set; }
    }

    public class DeleteQuizResponse { }

    [RpcBind(typeof(DeleteQuizRequest), typeof(DeleteQuizResponse))]
    public async Task<ApiResult<DeleteQuizResponse>> DeleteQuiz(DeleteQuizRequest req, AuthResult authResult)
    {
        await using (var tx = await this.db.BeginTransaction())
        {
            bool deleted = await this.quizService.Delete(req.Id, authResult.ProfileID);

            await tx.CommitAsync();

            if (!deleted)
            {
                return $"Quiz with id=${req.Id} not found.";
            }

            return new DeleteQuizResponse();
        }
    }

    public class SaveQuizRequest
    {
        public QuizModel Item { get; set; }
    }

    public class SaveQuizResponse { }

    [RpcBind(typeof(SaveQuizRequest), typeof(SaveQuizResponse))]
    public async Task<ApiResult<SaveQuizResponse>> SaveQuiz(SaveQuizRequest req, AuthResult authResult)
    {
        await using (var tx = await this.db.BeginTransaction())
        {
            req.Item.UserProfileID = authResult.ProfileID;

            if (req.Item.IsNew())
            {
                await this.quizService.Insert(req.Item);
            }
            else
            {
                var oldQuiz = await this.quizService.Get(req.Item.QuizID, authResult.ProfileID);

                if (oldQuiz == null)
                {
                    return "No quiz found for quizId=" + req.Item.QuizID;
                }

                await this.quizService.Update(oldQuiz, req.Item);
            }

            await tx.CommitAsync();

            return new SaveQuizResponse();
        }
    }
}

public interface QuizService
{
    Task<QuizPoco[]> Search(string query, int userProfileId);

    Task<QuizModel> Get(int quizId, int userProfileId);

    Task<bool> Delete(int quizId, int userProfileId);

    Task Insert(QuizModel newQuiz);

    Task Update(QuizModel oldQuiz, QuizModel newQuiz);
}

public class QuizServiceImpl : QuizService
{
    private readonly IDbService db;

    public QuizServiceImpl(IDbService db)
    {
        this.db = db;
    }

    public Task<QuizPoco[]> Search(string query, int userProfileId)
    {
        var pb = PredicateBuilder.True<QuizPoco>();

        pb = pb.And(x => x.UserProfileID == userProfileId);

        if (!string.IsNullOrWhiteSpace(query))
        {
            pb = pb.And(x => x.QuizName.Contains(query));
        }

        return this.db.Poco.Quizzes.Where(pb).ToArrayAsync();
    }

    public async Task<QuizModel> Get(int quizId, int userProfileId)
    {
        var dataset = await (
            from q in this.db.Poco.Quizzes
            join qq in this.db.Poco.QuizQuestions on q.QuizID equals qq.QuizID
            join qa in this.db.Poco.QuizAnswers on qq.QuestionID equals qa.QuestionID
            where q.QuizID == quizId && q.UserProfileID == userProfileId
            select new { quiz = q, quizQuestion = qq, quizAnswer = qa }
        ).ToArrayAsync();

        if (dataset.Length == 0)
        {
            return null;
        }

        var quiz = dataset[0].quiz;

        var quizModel = new QuizModel
        {
            QuizID = quiz.QuizID,
            QuizName = quiz.QuizName,
            UserProfileID = quiz.UserProfileID,
            Questions = new List<QuizQuestionModel>(),
        };

        var questionsById = new Dictionary<int, QuizQuestionModel>();
        var answerById = new Dictionary<int, QuizAnswerPoco>();

        foreach (var dataItem in dataset)
        {
            QuizQuestionModel questionModel;
            if (questionsById.ContainsKey(dataItem.quizQuestion.QuestionID))
            {
                questionModel = questionsById[dataItem.quizQuestion.QuestionID];
            }
            else
            {
                questionModel = new QuizQuestionModel
                {
                    QuestionID = dataItem.quizQuestion.QuestionID,
                    QuizID = dataItem.quizQuestion.QuizID,
                    QuestionName = dataItem.quizQuestion.QuestionName,
                    QuestionContent = dataItem.quizQuestion.QuestionContent,
                    Answers = new List<QuizAnswerPoco>(),
                };
                quizModel.Questions.Add(questionModel);
                questionsById.Add(questionModel.QuestionID, questionModel);
            }

            if (!answerById.ContainsKey(dataItem.quizAnswer.AnswerID))
            {
                var quizAnswerPoco = new QuizAnswerPoco
                {
                    AnswerID = dataItem.quizAnswer.AnswerID,
                    QuestionID = dataItem.quizAnswer.QuestionID,
                    AnswerContent = dataItem.quizAnswer.AnswerContent,
                    AnswerIsCorrect = dataItem.quizAnswer.AnswerIsCorrect,
                };
                questionModel.Answers.Add(quizAnswerPoco);
                answerById.Add(quizAnswerPoco.AnswerID, quizAnswerPoco);
            }
        }

        return quizModel;
    }

    public async Task<bool> Delete(int quizId, int userProfileId)
    {
        await (
            from qa in this.db.Poco.QuizAnswers
            join qq in this.db.Poco.QuizQuestions on qa.QuestionID equals qq.QuestionID
            join q in this.db.Poco.Quizzes on qq.QuizID equals q.QuizID
            where q.QuizID == quizId && q.UserProfileID == userProfileId
            select qa
        ).DeleteAsync();

        await (
            from qq in this.db.Poco.QuizQuestions
            join q in this.db.Poco.Quizzes on qq.QuizID equals q.QuizID
            where q.QuizID == quizId && q.UserProfileID == userProfileId
            select qq
        ).DeleteAsync();

        int deletedRecords = await (
            from q in this.db.Poco.Quizzes
            where q.QuizID == quizId && q.UserProfileID == userProfileId
            select q
        ).DeleteAsync();

        return deletedRecords > 0;
    }

    public async Task Insert(QuizModel newQuiz)
    {
        await this.db.Insert((QuizPoco)newQuiz);

        foreach (var question in newQuiz.Questions)
        {
            question.QuestionID = 0;
            question.QuizID = newQuiz.QuizID;
            await this.db.Insert((QuizQuestionPoco)question);

            foreach (var answer in question.Answers)
            {
                answer.AnswerID = 0;
                answer.QuestionID = question.QuestionID;
                await this.db.Insert(answer);
            }
        }
    }

    public async Task Update(QuizModel oldQuiz, QuizModel newQuiz)
    {
        await this.db.Update((QuizPoco)newQuiz);

        // Remove old records
        var removeQuestions = oldQuiz.Questions.ExceptBy(newQuiz.Questions, x => x, QuestionIdentityComparer.Instance);
        int[] removeQuestionIds = removeQuestions.Select(x => x.QuestionID).ToArray();
        await this.db.Poco.QuizAnswers.DeleteAsync(x => removeQuestionIds.Contains(x.QuestionID));
        await this.db.Poco.QuizQuestions.DeleteAsync(x => removeQuestionIds.Contains(x.QuestionID));

        // Add new records
        var addQuestions = newQuiz.Questions.ExceptBy(oldQuiz.Questions, x => x, QuestionIdentityComparer.Instance);
        foreach (var question in addQuestions)
        {
            question.QuestionID = 0;
            question.QuizID = newQuiz.QuizID;
            await this.db.Insert((QuizQuestionPoco)question);

            foreach (var answer in question.Answers)
            {
                answer.AnswerID = 0;
                answer.QuestionID = question.QuestionID;
                await this.db.Insert(answer);
            }
        }

        // Update existing records
        var newUpdateQuestions = newQuiz.Questions.IntersectBy(oldQuiz.Questions, x => x, QuestionIdentityComparer.Instance);
        var oldUpdateQuestionsById = oldQuiz.Questions.IntersectBy(newQuiz.Questions, x => x, QuestionIdentityComparer.Instance)
            .ToDictionary(x => x.QuestionID);

        foreach (var newQuestion in newUpdateQuestions)
        {
            newQuestion.QuizID = newQuiz.QuizID;
            await this.db.Update((QuizQuestionPoco)newQuestion);

            var oldQuestion = oldUpdateQuestionsById[newQuestion.QuestionID];

            // Remove old records
            var removeAnswers = oldQuestion.Answers.ExceptBy(newQuestion.Answers, x => x, AnswerIdentityComparer.Instance);
            int[] removeAnswersIds = removeAnswers.Select(x => x.AnswerID).ToArray();
            await this.db.Poco.QuizAnswers.DeleteAsync(x => removeAnswersIds.Contains(x.AnswerID));

            // Add new records
            var addAnswers = newQuestion.Answers.ExceptBy(oldQuestion.Answers, x => x, AnswerIdentityComparer.Instance);
            foreach (var answer in addAnswers)
            {
                answer.AnswerID = 0;
                answer.QuestionID = newQuestion.QuestionID;
                await this.db.Insert(answer);
            }

            // Update existing records
            var updateAnswers = newQuestion.Answers.IntersectBy(oldQuestion.Answers, x => x, AnswerIdentityComparer.Instance);
            foreach (var answer in updateAnswers)
            {
                answer.QuestionID = newQuestion.QuestionID;
                await this.db.Update(answer);
            }
        }
    }

    private class QuestionIdentityComparer : IEqualityComparer<QuizQuestionModel>
    {
        public static IEqualityComparer<QuizQuestionModel> Instance { get; } = new QuestionIdentityComparer();

        public bool Equals(QuizQuestionModel x, QuizQuestionModel y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.QuestionID == y.QuestionID;
        }

        public int GetHashCode(QuizQuestionModel obj)
        {
            return obj.QuestionID;
        }
    }

    private class AnswerIdentityComparer : IEqualityComparer<QuizAnswerPoco>
    {
        public static IEqualityComparer<QuizAnswerPoco> Instance { get; } = new AnswerIdentityComparer();

        public bool Equals(QuizAnswerPoco x, QuizAnswerPoco y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.AnswerID == y.AnswerID;
        }

        public int GetHashCode(QuizAnswerPoco obj)
        {
            return obj.AnswerID;
        }
    }
}

public class QuizQuestionModel : QuizQuestionPoco
{
    public List<QuizAnswerPoco> Answers { get; set; }
}

public class QuizModel : QuizPoco
{
    public List<QuizQuestionModel> Questions { get; set; }

    private static QuizModelComparer idComparerInstance;

    public static QuizModelComparer IdComparer => idComparerInstance ??= new QuizModelComparer();

    public class QuizModelComparer : IComparer<QuizModel>, IComparer
    {
        public int Compare(QuizModel x, QuizModel y)
        {
            if (x == null)
            {
                return 1;
            }

            if (y == null)
            {
                return -1;
            }

            return x!.QuizID.CompareTo(y!.QuizID);
        }

        public int Compare(object x, object y)
        {
            return this.Compare((QuizModel)x, (QuizModel)x);
        }
    }
}
