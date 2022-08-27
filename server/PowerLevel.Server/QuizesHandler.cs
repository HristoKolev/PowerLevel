namespace PowerLevel.Server;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using LinqToDB;
using Xdxd.DotNet.Rpc;

[RpcAuth]
public class QuizHandler
{
    private readonly QuizService quizService;

    public QuizHandler(QuizService quizService)
    {
        this.quizService = quizService;
    }

    [RpcBind(typeof(ListQuizzesRequest), typeof(ListQuizzesResponse))]
    public async Task<ListQuizzesResponse> ListQuizzes(ListQuizzesRequest req)
    {
        return new ListQuizzesResponse
        {
            Items = await this.quizService.GetQuizzes(req.Query),
        };
    }
}

public class ListQuizzesRequest
{
    public string Query { get; set; }
}

public class ListQuizzesResponse
{
    public List<QuizPoco> Items { get; set; }
}

public interface QuizService
{
    Task<List<QuizPoco>> GetQuizzes(string query);
}

public class QuizServiceImpl : QuizService
{
    private readonly IDbService db;

    public QuizServiceImpl(IDbService db)
    {
        this.db = db;
    }

    public Task<List<QuizPoco>> GetQuizzes(string query)
    {
        var pb = PredicateBuilder.True<QuizPoco>();

        if (!string.IsNullOrWhiteSpace(query))
        {
            pb = pb.And(x => x.QuizName.Contains(query));
        }

        return this.db.Poco.Quizzes.Where(x => x.QuizName.Contains(query)).ToListAsync();
    }
}
